using System.Collections;
using System.IO;

using UnityEditor;
using UnityEngine;

using Codice.Client.Common;
using Codice.Client.Common.Threading;
using Codice.CM.Common;
using Codice.UI.Progress;
using PlasticGui;
using PlasticGui.Configuration.CloudEdition;
using PlasticGui.SwitcherWindow.Repositories;
using PlasticGui.SwitcherWindow.Workspaces;

namespace Codice.Views.CreateWorkspace
{
    internal class CreateWorkspaceView :
        IPlasticDialogCloser,
        IWorkspacesRefreshableView
    {
        internal interface ICreateWorkspaceListener
        {
            void OnWorkspaceCreated(WorkspaceInfo wkInfo, bool isGluonMode);
        }

        internal CreateWorkspaceView(
            EditorWindow parentWindow,
            ICreateWorkspaceListener listener,
            PlasticAPI plasticApi)
        {
            mParentWindow = parentWindow;
            mCreateWorkspaceListener = listener;
            mProgressControls = new ProgressControlsForViews();
            mWorkspaceOperations = new WorkspaceOperations(this, mProgressControls);
            mCreateWorkspaceState = CreateWorkspaceViewState.BuildForProjectDefaults();

            Initialize(plasticApi);
        }

        internal void Update()
        {
            mProgressControls.UpdateProgress(mParentWindow);
        }

        internal void OnGUI()
        {
            if (Event.current.type == EventType.Layout)
            {
                mProgressControls.ProgressData.CopyInto(
                    mCreateWorkspaceState.ProgressData);
            }

            string repositoryName = mCreateWorkspaceState.RepositoryName;

            DrawCreateWorkspace.ForState(
                CreateRepository,
                ValidateAndCreateWorkspace,
                mParentWindow,
                mDefaultServer,
                Path.GetFullPath(Application.dataPath),
                ref mCreateWorkspaceState);

            if (repositoryName == mCreateWorkspaceState.RepositoryName)
                return;

            OnRepositoryChanged(
                mDialogUserAssistant,
                mCreateWorkspaceState);
        }

        void Initialize(PlasticAPI plasticApi)
        {
            ((IProgressControls)mProgressControls).ShowProgress(string.Empty);

            WorkspaceInfo[] allWorkspaces = null;
            IList allRepositories = null;

            IThreadWaiter waiter = ThreadWaiter.GetWaiter(10);
            waiter.Execute(
                /*threadOperationDelegate*/ delegate
                {
                    mDefaultServer =
                        GetDefaultServer.ToCreateWorkspace();

                    allWorkspaces = plasticApi.GetAllWorkspacesArray();
                    allRepositories = plasticApi.GetAllRepositories(
                        mDefaultServer, true);
                },
                /*afterOperationDelegate*/ delegate
                {
                    ((IProgressControls)mProgressControls).HideProgress();

                    string serverSpecPart = string.Format("@{0}",
                        mDefaultServer);

                    mCreateWorkspaceState.RepositoryName = ValidRepositoryName.Get(
                        string.Format("{0}{1}",
                            mCreateWorkspaceState.RepositoryName,
                            serverSpecPart),
                        allRepositories);

                    mCreateWorkspaceState.WorkspaceName =
                        mCreateWorkspaceState.RepositoryName.Replace(
                            serverSpecPart, string.Empty);

                    mDialogUserAssistant = new CreateWorkspaceDialogUserAssistant(
                        mCreateWorkspaceState.WorkspacePath,
                        allWorkspaces);

                    OnRepositoryChanged(
                        mDialogUserAssistant,
                        mCreateWorkspaceState);
                });
        }

        static void OnRepositoryChanged(
            CreateWorkspaceDialogUserAssistant dialogUserAssistant,
            CreateWorkspaceViewState createWorkspaceState)
        {
            if (dialogUserAssistant == null)
                return;

            dialogUserAssistant.RepositoryChanged(
                createWorkspaceState.RepositoryName,
                createWorkspaceState.WorkspaceName,
                createWorkspaceState.WorkspacePath);

            createWorkspaceState.WorkspaceName =
                dialogUserAssistant.GetProposedWorkspaceName();
        }

        void CreateRepository(
            RepositoryCreationData data)
        {
            if (!data.Result)
                return;

            ((IProgressControls)mProgressControls).ShowProgress(
                PlasticLocalization.GetString(
                    PlasticLocalization.Name.CreatingRepository,
                    data.RepName));

            RepositoryInfo createdRepository = null;

            IThreadWaiter waiter = ThreadWaiter.GetWaiter();
            waiter.Execute(
                /*threadOperationDelegate*/ delegate
                {
                    createdRepository = Plastic.API.CreateRepository(
                        data.ServerName, data.RepName);
                },
                /*afterOperationDelegate*/ delegate
                {
                    ((IProgressControls)mProgressControls).HideProgress();
                
                    if (waiter.Exception != null)
                    {
                        ((IProgressControls)mProgressControls).ShowError(
                            waiter.Exception.Message);
                        return;
                    }
                
                    if (createdRepository == null)
                        return;
                
                    mCreateWorkspaceState.RepositoryName =
                        createdRepository.GetRepSpec().ToString();
                });
        }

        void ValidateAndCreateWorkspace(
            CreateWorkspaceViewState state)
        {
            mWkCreationData = BuildCreationDataFromState(state);

            WorkspaceCreationValidation.AsyncValidation(
                mWkCreationData, this, mProgressControls);
            // validation calls IPlasticDialogCloser.CloseDialog()
            // when the validation is ok
        }

        void IPlasticDialogCloser.CloseDialog()
        {
            ((IProgressControls)mProgressControls).ShowProgress(string.Empty);

            IThreadWaiter waiter = ThreadWaiter.GetWaiter(10);
            waiter.Execute(
                /*threadOperationDelegate*/ delegate
                {
                    RepositorySpec repSpec = new SpecGenerator().GenRepositorySpec(
                        false, mWkCreationData.Repository);

                    bool repositoryExist = Plastic.API.CheckRepositoryExists(
                        repSpec.Server, repSpec.Name);

                    if (!repositoryExist)
                        Plastic.API.CreateRepository(repSpec.Server, repSpec.Name);
                },
                /*afterOperationDelegate*/ delegate
                {
                    ((IProgressControls)mProgressControls).HideProgress();

                    if (waiter.Exception != null)
                    {
                        ((IProgressControls)mProgressControls).ShowError(
                            waiter.Exception.Message);
                            return;
                    }

                    mWkCreationData.Result = true;
                    mWorkspaceOperations.CreateWorkspace(mWkCreationData);
                    // the operation calls IWorkspacesRefreshableView.RefreshAndSelect
                    // when the workspace is created
                });
        }

        void IWorkspacesRefreshableView.RefreshAndSelect(WorkspaceInfo wkInfo)
        {
            mCreateWorkspaceListener.OnWorkspaceCreated(
                wkInfo, mWkCreationData.IsGluonWorkspace);
        }

        static WorkspaceCreationData BuildCreationDataFromState(
            CreateWorkspaceViewState state)
        {
            return new WorkspaceCreationData(
                state.WorkspaceName,
                state.WorkspacePath,
                state.RepositoryName,
                state.WorkspaceMode ==
                    CreateWorkspaceViewState.WorkspaceModes.Gluon);
        }

        WorkspaceCreationData mWkCreationData;
        CreateWorkspaceViewState mCreateWorkspaceState;

        CreateWorkspaceDialogUserAssistant mDialogUserAssistant;

        string mDefaultServer;

        readonly EditorWindow mParentWindow;
        readonly ICreateWorkspaceListener mCreateWorkspaceListener;
        readonly WorkspaceOperations mWorkspaceOperations;
        readonly ProgressControlsForViews mProgressControls;

        class GetDefaultServer
        {
            internal static string ToCreateWorkspace()
            {
                string clientConfServer = Plastic.ConfigAPI.GetClientConfServer();

                if (!EditionToken.IsCloudEdition())
                    return clientConfServer;

                CloudEditionCreds.Data config =
                    CloudEditionCreds.GetFromClientConf();

                string organizationName = CloudOrganizationRetriever.GetOrganization(
                    config.Email, config.Password);

                if (string.IsNullOrEmpty(organizationName))
                    return clientConfServer;

                return string.Format("{0}@{1}", organizationName, CloudServer.Alias);
            }
        }
    }
}
