using System;
using System.IO;
using System.Linq;

using log4net;
using log4net.Config;

using UnityEditor;
using UnityEngine;

using Codice.AssetsProcessor;
using Codice.Client.BaseCommands;
using Codice.Client.BaseCommands.EventTracking;
using Codice.Client.Common;
using Codice.Client.Common.Encryption;
using Codice.Client.Common.EventTracking;
using Codice.Client.Common.FsNodeReaders;
using Codice.Client.Common.Threading;
using Codice.Client.Common.WebApi;
using Codice.CM.Common;
using Codice.CM.ConfigureHelper;
using Codice.Configuration;
using Codice.Tool;
using Codice.UI;
using Codice.UI.Progress;
using Codice.Views.CreateWorkspace;
using CodiceApp.EventTracking;
using GluonGui;
using PlasticGui;
using PlasticGui.Gluon;
using PlasticPipe.Certificates;

using GluonNewIncomingChangesUpdater = PlasticGui.Gluon.WorkspaceWindow.NewIncomingChangesUpdater;
using GluonCheckIncomingChanges = PlasticGui.Gluon.WorkspaceWindow.CheckIncomingChanges;
using EventTracking = PlasticGui.EventTracking.EventTracking;

namespace Codice
{
    public class PlasticWindow : EditorWindow,
        PlasticGui.WorkspaceWindow.CheckIncomingChanges.IAutoRefreshIncomingChangesView,
        GluonCheckIncomingChanges.IAutoRefreshIncomingChangesView,
        CreateWorkspaceView.ICreateWorkspaceListener
    {
        public PlasticGUIClient PlasticClientForTesting { get { return mPlasticClient; } }
        public ViewSwitcher ViewSwitcherForTesting { get { return mViewSwitcher; } }
        public IPlasticAPI PlasticApiForTesting { get { return mPlasticAPI; } }

        void PlasticGui.WorkspaceWindow.CheckIncomingChanges.IAutoRefreshIncomingChangesView.IfVisible()
        {
            mViewSwitcher.AutoRefreshIncomingChangesView();
        }

        void GluonCheckIncomingChanges.IAutoRefreshIncomingChangesView.IfVisible()
        {
            mViewSwitcher.AutoRefreshIncomingChangesView();
        }

        void CreateWorkspaceView.ICreateWorkspaceListener.OnWorkspaceCreated(
            WorkspaceInfo wkInfo, bool isGluonMode)
        {
            mWkInfo = wkInfo;
            mIsGluonMode = isGluonMode;
            mCreateWorkspaceView = null;

            if (mIsGluonMode)
                ConfigurePartialWorkspace.AsFullyChecked(mWkInfo);

            InitializePlastic();
            Repaint();
        }

        void OnEnable()
        {
            if (mException != null)
                return;

            GuiMessage.Initialize(new UnityPlasticGuiMessage(this));

            ConfigureLogging();

            RegisterExceptionHandlers();
            RegisterApplicationFocusHandlers(this);

            InitLocalization();

            ThreadWaiter.Initialize(new UnityThreadWaiterBuilder());
            ServicePointConfigurator.ConfigureServicePoint();
            CertificateUi.RegisterHandler(new ChannelCertificateUiImpl());
            CredentialsUIRegistrar.RegisterCredentialsUI(
                new CredentialsUiImpl(this));
            ClientEncryptionServiceProvider.SetEncryptionPasswordProvider(
                new MissingEncryptionPasswordPromptHandler(this));
            DisableFsWatcherIfNeeded();
            EditionManager.Get().DisableCapability(
                EnumEditionCapabilities.Extensions);

            mPlasticAPI = new PlasticAPI();
            ClientHandlers.Register();

            PlasticGuiConfig.SetConfigFile(PLASTIC_GUI_CONFIG_FILE);

            mPingEventLoop = new PingEventLoop();
            mEventSenderRestApi = new SimpleEventSenderRestApi(
                PlasticWebApiUris.GetBaseUri());
            mEventSenderScheduler = EventTracking.Configure(
                mEventSenderRestApi,
                ApplicationIdentifier.UnityPackage,
                IdentifyEventPlatform.Get());

            if (mEventSenderScheduler != null)
                mPingEventLoop.Start();

            InitializePlastic();
        }

        void OnDisable()
        {
            AssetsProcessors.Disable();

            if (mWkInfo != null)
                WorkspaceFsNodeReaderCachesCleaner.CleanWorkspaceFsNodeReader(mWkInfo);

            if (mException != null)
                return;

            if (mWkInfo == null)
            {
                ClosePlasticWindow(this);
                return;
            }

            if (mViewSwitcher != null)
                mViewSwitcher.OnDisable();

            ClosePlasticWindow(this);
        }

        void OnDestroy()
        {
            if (mException != null)
                return;

            if (mWkInfo == null)
                return;

            if (!mPlasticClient.IsOperationInProgress())
                return;

            bool bCloseWindow = GuiMessage.ShowQuestion(
                PlasticLocalization.GetString(PlasticLocalization.Name.OperationRunning),
                PlasticLocalization.GetString(PlasticLocalization.Name.ConfirmClosingRunningOperation),
                PlasticLocalization.GetString(PlasticLocalization.Name.YesButton));

            if (bCloseWindow)
                return;

            mForceToOpen = true;
            ShowPlasticWindow(this);
        }

        void OnFocus()
        {
            if (mException != null)
                return;

            if (mWkInfo == null)
                return;

            if (mViewSwitcher != null)
            {
                mViewSwitcher.AutoRefreshPendingChangesView();
                mViewSwitcher.AutoRefreshIncomingChangesView();
            }
        }

        void OnGUI()
        {
            if (mException != null)
            {
                DoExceptionErrorArea();
                return;
            }

            try
            {
                if (!IsExeAvailable.ForMode(mIsGluonMode))
                {
                    DrawToolNotAvailableNotification.
                        ForMode(position.width, mIsGluonMode);
                    return;
                }

                if (UnityConfigurationChecker.NeedsConfiguration())
                {
                    DoNoConfigAvailableArea(mIsGluonMode);
                    return;
                }

                if (mWkInfo == null)
                {
                    GetCreateWorkspaceView().OnGUI();
                    return;
                }

                DoHeader(
                    mWkInfo,
                    mPlasticClient,
                    mViewSwitcher,
                    mViewSwitcher,
                    mIsGluonMode,
                    mIncomingChangesNotificationPanel);

                DoTabToolbar(
                    mWkInfo,
                    mPlasticClient,
                    mViewSwitcher,
                    mIsGluonMode);

                mViewSwitcher.TabViewGUI();

                if (mPlasticClient.IsOperationInProgress())
                    DrawProgressForOperations.For(
                        mPlasticClient, mPlasticClient.Progress,
                        position.width);
            }
            catch (Exception ex)
            {
                if (IsExitGUIException(ex))
                    throw;

                mException = ex;

                GUI.enabled = true;

                DoExceptionErrorArea();

                ExceptionsHandler.HandleException("OnGUI", ex);
            }
        }

        void Update()
        {
            if (mException != null)
                return;

            if (mWkInfo == null)
                return;

            try
            {
                double currentUpdateTime = EditorApplication.timeSinceStartup;
                double elapsedSeconds = currentUpdateTime - mLastUpdateTime;

                if (mViewSwitcher != null)
                    mViewSwitcher.Update();

                if (mPlasticClient != null)
                    mPlasticClient.OnParentUpdated(elapsedSeconds);

                if (mCreateWorkspaceView != null)
                    mCreateWorkspaceView.Update();

                mLastUpdateTime = currentUpdateTime;
            }
            catch (Exception ex)
            {
                mException = ex;

                ExceptionsHandler.HandleException("Update", ex);
            }
        }

        void InitializePlastic()
        {
            if (mForceToOpen)
            {
                mForceToOpen = false;
                return;
            }

            try
            {
                if (UnityConfigurationChecker.NeedsConfiguration())
                    return;

                mWkInfo = FindWorkspace.InfoForApplicationPath(
                    Application.dataPath, mPlasticAPI);

                if (mWkInfo == null)
                    return;

                AssetsProcessors.Enable(mPlasticAPI);

                mIsGluonMode = mPlasticAPI.IsGluonWorkspace(mWkInfo);

                mPingEventLoop.SetWorkspace(mWkInfo);
                mEventSenderRestApi.SetToken(BuildToken.FromServerProfile(
                    ClientConfig.Get().GetDefaultProfile()));

                InitializeNewIncomingChanges(mWkInfo, mIsGluonMode);

                ViewHost viewHost = new ViewHost();

                PlasticGui.WorkspaceWindow.PendingChanges.PendingChanges pendingChanges =
                    new PlasticGui.WorkspaceWindow.PendingChanges.PendingChanges(mWkInfo);

                mViewSwitcher = new ViewSwitcher(
                    mWkInfo,
                    viewHost,
                    mIsGluonMode,
                    pendingChanges,
                    mDeveloperNewIncomingChangesUpdater,
                    mGluonNewIncomingChangesUpdater,
                    mIncomingChangesNotificationPanel,
                    this);

                mPlasticClient = new PlasticGUIClient(
                    mWkInfo,
                    mViewSwitcher,
                    mViewSwitcher,
                    viewHost,
                    pendingChanges,
                    mDeveloperNewIncomingChangesUpdater,
                    mGluonNewIncomingChangesUpdater,
                    this,
                    new UnityPlasticGuiMessage(this));

                mViewSwitcher.SetPlasticGUIClient(mPlasticClient);
                mViewSwitcher.ShowInitialView();

                mLastUpdateTime = EditorApplication.timeSinceStartup;
            }
            catch (Exception ex)
            {
                mException = ex;

                ExceptionsHandler.HandleException("InitializePlastic", ex);
            }
        }

        void InitializeNewIncomingChanges(
            WorkspaceInfo wkInfo,
            bool bIsGluonMode)
        {
            if (bIsGluonMode)
            {
                Gluon.IncomingChangesNotificationPanel gluonPanel =
                    new Gluon.IncomingChangesNotificationPanel(this);
                mGluonNewIncomingChangesUpdater =
                    NewIncomingChanges.BuildUpdaterForGluon(
                        wkInfo,
                        this,
                        gluonPanel,
                        new GluonCheckIncomingChanges.CalculateIncomingChanges());
                mIncomingChangesNotificationPanel = gluonPanel;
                return;
            }

            Developer.IncomingChangesNotificationPanel developerPanel =
                new Developer.IncomingChangesNotificationPanel(this);
            mDeveloperNewIncomingChangesUpdater =
                NewIncomingChanges.BuildUpdaterForDeveloper(
                    wkInfo, this, developerPanel);
            mIncomingChangesNotificationPanel = developerPanel;
        }

        void OnApplicationActivated()
        {
            if (mException != null)
                return;

            Reload.IfWorkspaceConfigChanged(
                mPlasticAPI, mWkInfo, mIsGluonMode,
                ExecuteFullReload);

            if (mWkInfo == null)
                return;

            NewIncomingChanges.LaunchUpdater(
                mDeveloperNewIncomingChangesUpdater,
                mGluonNewIncomingChangesUpdater);

            if (mViewSwitcher != null)
            {
                mViewSwitcher.AutoRefreshPendingChangesView();
                mViewSwitcher.AutoRefreshIncomingChangesView();
            }
        }

        void OnApplicationDeactivated()
        {
            if (mException != null)
                return;

            if (mWkInfo == null)
                return;

            NewIncomingChanges.StopUpdater(
                mDeveloperNewIncomingChangesUpdater,
                mGluonNewIncomingChangesUpdater);
        }

        void ExecuteFullReload()
        {
            mException = null;

            DisposeNewIncomingChanges(this);

            InitializePlastic();
        }

        void DoExceptionErrorArea()
        {
            string labelText = PlasticLocalization.GetString(
                PlasticLocalization.Name.UnexpectedError);

            string buttonText = PlasticLocalization.GetString(
                PlasticLocalization.Name.ReloadButton);

            DrawActionHelpBox.For(
                Images.GetErrorDialogIcon(), labelText, buttonText,
                ExecuteFullReload);
        }

        CreateWorkspaceView GetCreateWorkspaceView()
        {
            if (mCreateWorkspaceView != null)
                return mCreateWorkspaceView;

            mCreateWorkspaceView = new CreateWorkspaceView(
                this, this, mPlasticAPI);

            return mCreateWorkspaceView;
        }

        static void DoNoConfigAvailableArea(bool isGluonMode)
        {
            string labelText =
                "No configuration found. Plastic SCM plugin is disabled. " +
                "Please configure it by clicking here.";

            string buttonText = isGluonMode ?
                "Launch Gluon" : "Launch Plastic";

            DrawActionHelpBox.For(
                Images.GetWarnDialogIcon(),
                labelText, buttonText, delegate
                {
                    LaunchTool.OpenConfigurationForMode(isGluonMode);
                });
        }

        static void DoHeader(
            WorkspaceInfo workspaceInfo,
            PlasticGUIClient plasticClient,
            IMergeViewLauncher mergeViewLauncher,
            IGluonViewSwitcher gluonSwitcher,
            bool isGluonMode,
            IIncomingChangesNotificationPanel incomingChangesNotificationPanel)
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

            GUILayout.Label(
                plasticClient.HeaderTitle,
                UnityStyles.PlasticWindow.HeaderLabel);

            GUILayout.FlexibleSpace();

            DrawIncomingChangesNotificationPanel.ForMode(
                workspaceInfo, plasticClient,
                mergeViewLauncher, gluonSwitcher, isGluonMode,
                incomingChangesNotificationPanel.IsVisible,
                incomingChangesNotificationPanel.Data);

            //TO DO: Codice - beta: hide the switcher until the update dialog is implemented
            //DrawGuiModeSwitcher.ForMode(
            //    isGluonMode, plasticClient, changesTreeView, editorWindow);

            EditorGUILayout.EndHorizontal();
        }

        static void DoTabToolbar(
            WorkspaceInfo workspaceInfo,
            PlasticGUIClient plasticClient,
            ViewSwitcher viewSwitcher,
            bool isGluonMode)
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

            viewSwitcher.TabButtonsGUI();

            GUILayout.FlexibleSpace();

            DoLaunchButtons(
                workspaceInfo.ClientPath,
                isGluonMode);

            EditorGUILayout.EndHorizontal();
        }

        static void DoLaunchButtons(
            string wkPath, bool isGluonMode)
        {
            //TO DO: Codice - beta: hide the diff button until the behavior is implemented
            /*GUILayout.Button(PlasticLocalization.GetString(
                PlasticLocalization.Name.DiffWindowMenuItemDiff),
                EditorStyles.toolbarButton,
                GUILayout.Width(UnityConstants.REGULAR_BUTTON_WIDTH));*/

            if (isGluonMode)
            {
                if (DoLaunchButton("Configure Gluon"))
                    LaunchTool.OpenWorkspaceConfiguration(wkPath);
            }
            else
            {
                if (DoLaunchButton("Launch branch explorer"))
                    LaunchTool.OpenBranchExplorer(wkPath);
            }

            string openToolText = isGluonMode ?
                "Launch Gluon" : "Launch Plastic";

            if (DoLaunchButton(openToolText))
                LaunchTool.OpenGUIForMode(wkPath, isGluonMode);
        }

        static bool DoLaunchButton(string buttonText)
        {
            GUIContent buttonContent = new GUIContent(buttonText);
            GUIStyle buttonStyle = EditorStyles.miniButton;

            Rect rt = GUILayoutUtility.GetRect(buttonContent, buttonStyle);

            return GUI.Button(rt, buttonText, buttonStyle);
        }

        static void DisposeNewIncomingChanges(PlasticWindow window)
        {
            NewIncomingChanges.DisposeUpdater(
                window.mDeveloperNewIncomingChangesUpdater,
                window.mGluonNewIncomingChangesUpdater);

            window.mDeveloperNewIncomingChangesUpdater = null;
            window.mGluonNewIncomingChangesUpdater = null;
        }

        static void InitLocalization()
        {
            string language = null;
            try
            {
                language = ClientConfig.Get().GetLanguage();
            }
            catch
            {
                language = string.Empty;
            }

            Localization.Init(language);
            PlasticLocalization.SetLanguage(language);
        }

        static void ConfigureLogging()
        {
            try
            {
                string log4netpath = ToolConfig.GetUnityPlasticLogConfigFile();

                if (!File.Exists(log4netpath))
                    WriteLogConfiguration.For(log4netpath);

                XmlConfigurator.Configure(new FileInfo(log4netpath));
            }
            catch
            {
                //it failed configuring the logging info; nothing to do.
            }
        }

        static void RegisterApplicationFocusHandlers(PlasticWindow window)
        {
            EditorWindowFocus.OnApplicationActivated += window.OnApplicationActivated;
            EditorWindowFocus.OnApplicationDeactivated += window.OnApplicationDeactivated;
        }

        static void UnRegisterApplicationFocusHandlers(PlasticWindow window)
        {
            EditorWindowFocus.OnApplicationActivated -= window.OnApplicationActivated;
            EditorWindowFocus.OnApplicationDeactivated -= window.OnApplicationDeactivated;
        }

        static void RegisterExceptionHandlers()
        {
            AppDomain.CurrentDomain.UnhandledException += HandleUnhandledException;

            Application.logMessageReceivedThreaded += HandleLog;
        }

        static void UnRegisterExceptionHandlers()
        {
            AppDomain.CurrentDomain.UnhandledException -= HandleUnhandledException;

            Application.logMessageReceivedThreaded -= HandleLog;
        }

        static void HandleUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Exception ex = (Exception)args.ExceptionObject;

            if (IsExitGUIException(ex) ||
                !IsPlasticStackTrace(ex.StackTrace))
                return;

            GUIActionRunner.RunGUIAction(delegate {
                ExceptionsHandler.HandleException("HandleUnhandledException", ex);
            });
        }

        static bool IsExitGUIException(Exception ex)
        {
            return ex is ExitGUIException;
        }

        static bool IsPlasticStackTrace(string stackTrace)
        {
            if (stackTrace == null)
                return false;

            string[] namespaces = new[] {
                "Codice.",
                "GluonGui.",
                "PlasticGui."
            };

            return namespaces.Any(stackTrace.Contains);
        }

        static void HandleLog(string logString, string stackTrace, LogType type)
        {
            if (type != LogType.Exception)
                return;

            if (!IsPlasticStackTrace(stackTrace))
                return;

            GUIActionRunner.RunGUIAction(delegate {
                mLog.ErrorFormat("[HandleLog] Unexpected error: {0}", logString);
                mLog.DebugFormat("Stack trace: {0}", stackTrace);

                string message = logString;
                if (ExceptionsHandler.DumpStackTrace())
                    message += Environment.NewLine + stackTrace;

                GuiMessage.ShowError(message);
            });
        }

        static void ClosePlasticWindow(PlasticWindow window)
        {
            UnRegisterApplicationFocusHandlers(window);
            UnRegisterExceptionHandlers();

            if (window.mEventSenderScheduler != null)
            {
                window.mPingEventLoop.Stop();
                window.mEventSenderScheduler.End();
            }

            DisposeNewIncomingChanges(window);
        }

        static void ShowPlasticWindow(PlasticWindow window)
        {
            EditorWindow dockWindow = FindEditorWindow.ToDock<PlasticWindow>();

            PlasticWindow newPlasticWindow = InstantiateFrom(window);

            if (DockEditorWindow.IsAvailable())
                DockEditorWindow.To(dockWindow, newPlasticWindow);

            newPlasticWindow.Show();

            newPlasticWindow.Focus();
        }

        static void DisableFsWatcherIfNeeded()
        {
            if (!DisableFsWatcher.MustDisableFsWatcher())
                return;

            WorkspaceWatcherFsNodeReadersCache.Get().DisableWatcher();
            Debug.LogWarning(
                "Plastic SCM: Detected an unsupported Runtime Version. " + 
                "The Plastic SCM Filesystem Watcher is not compatible with this runtime, " + 
                "and has been disabled.This can affect to the Pending Changes view performance, " + 
                "and also causes that the auto - refresh feature for views is disabled.We strongly " + 
                "recommend upgrading to the.NET 4.x equivalent Runtime Version for a better experience. " + 
                "In order to change the Runtime Version go Edit > Project Settings > Player > Other Settings > Configuration > Scripting Runtime Version.");
        }

        static PlasticWindow InstantiateFrom(PlasticWindow window)
        {
            PlasticWindow result = Instantiate(window);
            result.mWkInfo = window.mWkInfo;
            result.mPlasticClient = window.mPlasticClient;
            result.mViewSwitcher = window.mViewSwitcher;
            result.mDeveloperNewIncomingChangesUpdater = window.mDeveloperNewIncomingChangesUpdater;
            result.mGluonNewIncomingChangesUpdater = window.mGluonNewIncomingChangesUpdater;
            result.mException = window.mException;
            result.mLastUpdateTime = window.mLastUpdateTime;
            result.mIsGluonMode = window.mIsGluonMode;
            result.mIncomingChangesNotificationPanel = window.mIncomingChangesNotificationPanel;
            result.mCreateWorkspaceView = window.mCreateWorkspaceView;
            result.mPlasticAPI = window.mPlasticAPI;
            result.mEventSenderRestApi = window.mEventSenderRestApi;
            result.mEventSenderScheduler = window.mEventSenderScheduler;
            result.mPingEventLoop = window.mPingEventLoop;
            return result;
        }

        static class Reload
        {
            internal static void IfWorkspaceConfigChanged(
                IPlasticAPI plasticApi,
                WorkspaceInfo lastWkInfo,
                bool lastIsGluonMode,
                Action reloadAction)
            {
                string applicationPath = Application.dataPath;

                bool isGluonMode = false;
                WorkspaceInfo wkInfo = null;

                IThreadWaiter waiter = ThreadWaiter.GetWaiter(10);
                waiter.Execute(
                    /*threadOperationDelegate*/ delegate
                    {
                        wkInfo = FindWorkspace.
                            InfoForApplicationPath(applicationPath, plasticApi);

                        if (wkInfo != null)
                            isGluonMode = plasticApi.IsGluonWorkspace(wkInfo);
                    },
                    /*afterOperationDelegate*/ delegate
                    {
                        if (waiter.Exception != null)
                            return;

                        if (!IsWorkspaceConfigChanged(
                                lastWkInfo, wkInfo,
                                lastIsGluonMode, isGluonMode))
                            return;

                        reloadAction();
                });
            }

            static bool IsWorkspaceConfigChanged(
                WorkspaceInfo lastWkInfo,
                WorkspaceInfo currentWkInfo,
                bool lastIsGluonMode,
                bool currentIsGluonMode)
            {
                if (lastIsGluonMode != currentIsGluonMode)
                    return true;

                if (lastWkInfo == null || currentWkInfo == null)
                    return true;

                return !lastWkInfo.Equals(currentWkInfo);
            }
        }

        [SerializeField]
        bool mForceToOpen;

        Exception mException;

        IIncomingChangesNotificationPanel mIncomingChangesNotificationPanel;

        double mLastUpdateTime = 0f;

        ViewSwitcher mViewSwitcher;
        CreateWorkspaceView mCreateWorkspaceView;

        PlasticGui.WorkspaceWindow.NewIncomingChangesUpdater mDeveloperNewIncomingChangesUpdater;
        GluonNewIncomingChangesUpdater mGluonNewIncomingChangesUpdater;

        PlasticGUIClient mPlasticClient;
        WorkspaceInfo mWkInfo;
        bool mIsGluonMode;

        PlasticAPI mPlasticAPI;
        EventSender.IRestApi mEventSenderRestApi;
        EventSenderScheduler mEventSenderScheduler;
        PingEventLoop mPingEventLoop;

        static string PLASTIC_GUI_CONFIG_FILE = "unitygui.conf";

        static readonly ILog mLog = LogManager.GetLogger("PlasticWindow");
    }
}
 