using System;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

using Codice.Client.Common;
using Codice.UI;
using Codice.UI.Progress;
using Codice.UI.Tree;
using PlasticGui;
using PlasticGui.SwitcherWindow.Repositories;
using PlasticGui.WorkspaceWindow.Servers;

namespace Codice.Views.CreateWorkspace.Dialogs
{
    internal class RepositoryExplorerDialog :
        PlasticDialog,
        KnownServersListOperations.IKnownServersList
    {
        protected override Rect DefaultRect
        {
            get
            {
                var baseRect = base.DefaultRect;
                return new Rect(baseRect.x, baseRect.y, 750, 450);
            }
        }

        public static string BrowseRepository(
            EditorWindow parentWindow, string defaultServer)
        {
            RepositoryExplorerDialog dialog = Create(
                new ProgressControlsForDialogs(), defaultServer);

            ResponseType dialogResult = dialog.RunModal(parentWindow);

            if (dialogResult != ResponseType.Ok)
                return null;

            return dialog.mRepositoriesListView.GetSelectedRepository();
        }

        protected override PlasticDialog CloneModal()
        {
            return Create(mProgressControls, mState.Server);
        }

        protected override void SaveSettings()
        {
            TreeHeaderSettings.Save(
                mRepositoriesListView.multiColumnHeader.state,
                UnityConstants.REPOSITORIES_TABLE_SETTINGS_NAME);
        }

        void Refresh()
        {
            mFillRepositoriesTable.FillTable(
                mRepositoriesListView,
                null,
                mProgressControls,
                null,
                new FillRepositoriesTable.SaveLastUsedServer(true),
                null,
                null,
                null,
                mState.SearchString,
                mState.Server,
                false,
                false,
                true);
        }

        void KnownServersListOperations.IKnownServersList.FillValues(
            List<string> values)
        {
            mState.AvailableServers = values;

            Refresh();
        }

        void OnServerSelected(object server)
        {
            mState.Server = server.ToString();
            Repaint();
            Refresh();
        }

        protected override void OnModalGUI()
        {
            Title("Choose repository");

            Paragraph(PlasticLocalization.GetString(
                PlasticLocalization.Name.SelectRepositoryBelow));

            if (Event.current.type == EventType.Layout)
            {
                mProgressControls.ProgressData.CopyInto(
                    mState.ProgressData);
            }

            bool isEnabled = !mProgressControls.ProgressData.IsWaitingAsyncResult;

            DoToolbarArea(
                mSearchField,
                isEnabled,
                Refresh,
                OnServerSelected,
                ref mState);

            GUILayout.Space(10);

            DoListArea(
                mRepositoriesListView,
                isEnabled,
                mState.SearchString);

            DrawProgressForDialogs.For(
                mProgressControls.ProgressData);

            DoButtonsArea();

            mProgressControls.ForcedUpdateProgress(this);
        }

        static void DoToolbarArea(
            SearchField searchField,
            bool isEnabled,
            Action refreshAction,
            GenericMenu.MenuFunction2 selectServerAction,
            ref State state)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label("Server:");

            GUI.enabled = isEnabled;

            state.Server = DoDropDownTextField(
                state.Server,
                state.AvailableServers,
                selectServerAction,
                refreshAction);

            if (GUILayout.Button("Refresh", EditorStyles.miniButton))
                refreshAction();

            GUILayout.FlexibleSpace();

            DoSearchField(
                searchField,
                ref state);

            GUI.enabled = true;

            GUILayout.EndHorizontal();
        }

        static void DoSearchField(
            SearchField searchField,
            ref State state)
        {
            var searchFieldRect = GUILayoutUtility.GetRect(
                SEARCH_FIELD_WIDTH / 2, EditorGUIUtility.singleLineHeight);
            searchFieldRect.y += 2;

            state.SearchString = searchField.OnToolbarGUI(
                searchFieldRect, state.SearchString);

            if (!string.IsNullOrEmpty(state.SearchString))
                return;

            GUI.Label(searchFieldRect, PlasticLocalization.GetString(
                PlasticLocalization.Name.SearchTooltip), UnityStyles.Search);
        }

        static void DoListArea(
            RepositoriesListView listView,
            bool isEnabled,
            string searchString)
        {
            GUI.enabled = isEnabled;

            Rect treeRect = GUILayoutUtility.GetRect(0, 100000, 0, 100000);

            listView.searchString = searchString;
            listView.OnGUI(treeRect);

            GUI.enabled = true;
        }

        static string DoDropDownTextField(
            string text,
            List<string> options,
            GenericMenu.MenuFunction2 selectServerAction,
            Action enterKeyAction)
        {
            bool isEnterKeyPressed = false;

            Event e = Event.current;

            if (Keyboard.IsReturnOrEnterKeyPressed(e))
            {
                isEnterKeyPressed = true;
            }

            string result = DropDownTextField.DoDropDownTextField(
                text,
                DROPDOWN_CONTROL_NAME,
                options,
                selectServerAction,

                GUILayout.Width(DROPDOWN_WIDTH));

            if (isEnterKeyPressed && 
                GUI.GetNameOfFocusedControl() == DROPDOWN_CONTROL_NAME)
            {
                e.Use();
                enterKeyAction();
            }

            return result;
        }

        void DoButtonsArea()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                if (Application.platform == RuntimePlatform.WindowsEditor)
                {
                    DoOkButton();
                    DoCancelButton();
                    return;
                }

                DoCancelButton();
                DoOkButton();
            }
        }

        void DoOkButton()
        {
            if (!AcceptButton(PlasticLocalization.GetString(
                    PlasticLocalization.Name.OkButton)))
                return;

            OkButtonAction();
        }

        void DoCancelButton()
        {
            if (!NormalButton(PlasticLocalization.GetString(
                    PlasticLocalization.Name.CancelButton)))
                return;

            CancelButtonAction();
        }

        static RepositoryExplorerDialog Create(
            ProgressControlsForDialogs progressControls, string defaultServer)
        {
            var instance = CreateInstance<RepositoryExplorerDialog>();
            instance.mEscapeKeyAction = instance.CancelButtonAction;
            instance.mProgressControls = progressControls;
            instance.BuildComponents(defaultServer);
            return instance;
        }

        void BuildComponents(string defaultServer)
        {
            mSearchField = new SearchField();

            RepositoriesListHeaderState headerState = RepositoriesListHeaderState.Default;
            TreeHeaderSettings.Load(headerState,
                UnityConstants.REPOSITORIES_TABLE_SETTINGS_NAME,
                (int)RepositoriesListColumn.Name);

            mRepositoriesListView = new RepositoriesListView(
                headerState,
                OkButtonAction);
            mRepositoriesListView.Reload();

            mFillRepositoriesTable = new FillRepositoriesTable(
                new LocalRepositoriesProvider());

            mState = new State()
            {
                Server = defaultServer,
                SearchString = string.Empty,
                ProgressData = new ProgressControlsForDialogs.Data()
            };

            KnownServersListOperations.GetCombinedServers(
                true,
                new List<string>(),
                mProgressControls,
                this,
                CmConnection.Get().GetProfileManager());
        }

        SearchField mSearchField;
        IList mRepositories;
        RepositoriesListView mRepositoriesListView;
        ProgressControlsForDialogs mProgressControls;
        FillRepositoriesTable mFillRepositoriesTable;
        State mState;

        const string DROPDOWN_CONTROL_NAME = "RepositoryExplorerDialog.ServerDropdown";
        const float DROPDOWN_WIDTH = 250;
        const float SEARCH_FIELD_WIDTH = 450;

        class State
        {
            internal List<string> AvailableServers { get; set; }
            internal string Server { get; set; }
            internal string SearchString { get; set; }
            internal ProgressControlsForDialogs.Data ProgressData { get; set; }
        }
    }
}
