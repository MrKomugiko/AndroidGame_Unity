using System;
using System.IO;

using UnityEditor;
using UnityEngine;

using Codice.Client.BaseCommands;
using Codice.Client.Common;
using Codice.CM.Client.Gui;
using Codice.UI;
using Codice.UI.Progress;
using Codice.Views.CreateWorkspace.Dialogs;
using PlasticGui;
using PlasticGui.SwitcherWindow.Repositories;
using Codice.CM.Common;

namespace Codice.Views.CreateWorkspace
{
    internal static class DrawCreateWorkspace
    {
        internal static void ForState(
            Action<RepositoryCreationData> createRepositoryAction,
            Action<CreateWorkspaceViewState> createWorkspaceAction,
            EditorWindow parentWindow,
            string defaultServer,
            string projectPath,
            ref CreateWorkspaceViewState state)
        {
            GUILayout.Space(10);

            DoTitle();

            GUILayout.Space(15);

            DoFieldsArea(
                createRepositoryAction,
                parentWindow,
                defaultServer,
                projectPath,
                ref state);

            GUILayout.Space(10);

            DoRadioButtonsArea(ref state);

            GUILayout.Space(3);

            DoHelpLabel();

            GUILayout.Space(10);

            DoCreateWorkspaceButton(
                createWorkspaceAction,
                projectPath,
                ref state);

            GUILayout.Space(5);

            DoNotificationArea(state.ProgressData);
        }

        static void DoTitle()
        {
            GUILayout.Label(
                PlasticLocalization.GetString(
                    PlasticLocalization.Name.NewWorkspace),
                UnityStyles.Dialog.MessageTitle);

            GUILayout.Label(
                PlasticLocalization.GetString(
                    PlasticLocalization.Name.NewWorkspaceExplanation),
                EditorStyles.wordWrappedLabel);
        }

        static void DoFieldsArea(
            Action<RepositoryCreationData> createRepositoryAction,
            EditorWindow parentWindow,
            string defaultServer,
            string projectPath,
            ref CreateWorkspaceViewState state)
        {
            DoRepositoryField(
                createRepositoryAction,
                parentWindow,
                defaultServer,
                ref state);

            DoWorkspaceField(ref state);

            DoPathOnDiskField(projectPath, ref state);
        }

        static void DoRepositoryField(
            Action<RepositoryCreationData> createRepositoryAction,
            EditorWindow parentWindow,
            string defaultServer,
            ref CreateWorkspaceViewState state)
        {
            EditorGUILayout.BeginHorizontal();

            DoLabel("Repository name");

            state.RepositoryName = DoTextField(
                state.RepositoryName,
                !state.ProgressData.IsOperationRunning,
                LABEL_WIDTH,
                TEXTBOX_WIDTH - BROWSE_BUTTON_WIDTH);

            float browseButtonX =
                LABEL_WIDTH + TEXTBOX_WIDTH + BUTTON_MARGIN -
                BROWSE_BUTTON_WIDTH;
            float browseButtonWidth =
                BROWSE_BUTTON_WIDTH - BUTTON_MARGIN;

            if (DoButton(
                    "...",
                    !state.ProgressData.IsOperationRunning,
                    browseButtonX,
                    browseButtonWidth))
            {
                DoBrowseRepositoryButton(
                    parentWindow,
                    defaultServer,
                    ref state);
                EditorGUIUtility.ExitGUI();
            }

            float newButtonX =
                LABEL_WIDTH + TEXTBOX_WIDTH + BUTTON_MARGIN;
            float newButtonWidth =
                NEW_BUTTON_WIDTH - BUTTON_MARGIN;

            if (DoButton(
                    "New ...",
                    !state.ProgressData.IsOperationRunning,
                    newButtonX, newButtonWidth))
            {
                DoNewRepositoryButton(
                    createRepositoryAction,
                    parentWindow,
                    state.RepositoryName,
                    defaultServer);
                EditorGUIUtility.ExitGUI();
            }

            ValidationResult validationResult = ValidateRepositoryName(
                state.RepositoryName);

            if (!validationResult.IsValid)
                DoWarningLabel(validationResult.ErrorMessage,
                    LABEL_WIDTH + TEXTBOX_WIDTH + NEW_BUTTON_WIDTH + LABEL_MARGIN);

            EditorGUILayout.EndHorizontal();
        }

        static void DoWorkspaceField(
            ref CreateWorkspaceViewState state)
        {
            EditorGUILayout.BeginHorizontal();

            DoLabel("Workspace name");

            state.WorkspaceName = DoTextField(
                state.WorkspaceName,
                !state.ProgressData.IsOperationRunning,
                LABEL_WIDTH,
                TEXTBOX_WIDTH - BROWSE_BUTTON_WIDTH);

            ValidationResult validationResult = ValidateWorkspaceName(
                state.WorkspaceName);

            if (!validationResult.IsValid)
                DoWarningLabel(validationResult.ErrorMessage,
                    LABEL_WIDTH + TEXTBOX_WIDTH - BROWSE_BUTTON_WIDTH + LABEL_MARGIN);

            EditorGUILayout.EndHorizontal();
        }

        static void DoPathOnDiskField(
            string projectPath,
            ref CreateWorkspaceViewState state)
        {
            EditorGUILayout.BeginHorizontal();

            DoLabel("Path on disk");

            state.WorkspacePath = DoTextField(
                state.WorkspacePath,
                !state.ProgressData.IsOperationRunning,
                LABEL_WIDTH,
                TEXTBOX_WIDTH - BROWSE_BUTTON_WIDTH);

            float buttonX =
                LABEL_WIDTH + TEXTBOX_WIDTH + BUTTON_MARGIN -
                BROWSE_BUTTON_WIDTH;
            float buttonWidth =
                BROWSE_BUTTON_WIDTH - BUTTON_MARGIN;

            if (DoButton(
                "...",
                !state.ProgressData.IsOperationRunning,
                buttonX,
                buttonWidth))
                DoOpenFolderPanel(ref state);

            ValidationResult validationResult = ValidateWorkspacePath(
                state.WorkspacePath, projectPath);

            if (!validationResult.IsValid)
            {
                DoWarningLabel(
                    validationResult.ErrorMessage,
                    LABEL_WIDTH + TEXTBOX_WIDTH + LABEL_MARGIN);
            }

            EditorGUILayout.EndHorizontal();
        }

        static void DoOpenFolderPanel(
            ref CreateWorkspaceViewState state)
        {
            string result = EditorUtility.OpenFolderPanel(
                "Create workspace",
                state.WorkspacePath,
                string.Empty);

            if (string.IsNullOrEmpty(result))
                return;

            state.WorkspacePath = Path.GetFullPath(result);
        }

        static void DoRadioButtonsArea(
            ref CreateWorkspaceViewState state)
        {
            EditorGUILayout.BeginHorizontal();
            DoLabel("I'm a");

            EditorGUILayout.BeginVertical();
            if (DoRadioButton(
                "Developer, I use branches, merges and maybe push/pull",
                state.WorkspaceMode == CreateWorkspaceViewState.WorkspaceModes.Developer,
                !state.ProgressData.IsOperationRunning,
                LABEL_WIDTH))
                state.WorkspaceMode = CreateWorkspaceViewState.WorkspaceModes.Developer;

            if (DoRadioButton(
                "Artist, I simply want to checkin and forget",
                state.WorkspaceMode == CreateWorkspaceViewState.WorkspaceModes.Gluon,
                !state.ProgressData.IsOperationRunning,
                LABEL_WIDTH))
                state.WorkspaceMode = CreateWorkspaceViewState.WorkspaceModes.Gluon;

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }

        static void DoCreateWorkspaceButton(
            Action<CreateWorkspaceViewState> createWorkspaceAction,
            string projectPath,
            ref CreateWorkspaceViewState state)
        {
            EditorGUILayout.BeginHorizontal();

            bool isButtonEnabled =
                IsValidState(state, projectPath) &&
                !state.ProgressData.IsOperationRunning;

            if (DoButton("Create workspace", isButtonEnabled, LABEL_WIDTH, CREATE_WORKSPACE_BUTTON_WIDTH))
            {
                createWorkspaceAction(state);
                return;
            }

            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();
        }

        static void DoBrowseRepositoryButton(
            EditorWindow parentWindow,
            string defaultServer,
            ref CreateWorkspaceViewState state)
        {
            string result = RepositoryExplorerDialog.BrowseRepository(
                parentWindow, defaultServer);

            if (string.IsNullOrEmpty(result))
                return;

            state.RepositoryName = result;
        }

        static void DoNewRepositoryButton(
            Action<RepositoryCreationData> createRepositoryAction,
            EditorWindow parentWindow,
            string repository,
            string defaultServer)
        {
            RepositorySpec repSpec = new SpecGenerator().
                GenRepositorySpec(false, repository);

            RepositoryCreationData creationData = CreateRepositoryDialog.CreateRepository(
                parentWindow,
                repSpec.Server,
                defaultServer,
                ClientConfig.Get().GetWorkspaceServer());

            createRepositoryAction(creationData);
        }

        static void DoHelpLabel()
        {
            string linkText = "here.";
            string labelText = string.Format(
                "Learn more about the differences between Developer/Artist workspaces <color=\"{0}\">{1}</color>",
                UnityStyles.HexColors.LINK_COLOR,
                linkText);

            EditorGUILayout.BeginHorizontal();

            if (DoLinkLabel(labelText, linkText, LABEL_WIDTH))
                Application.OpenURL(HELP_URL);

            EditorGUILayout.EndHorizontal();
        }

        static void DoNotificationArea(ProgressControlsForViews.Data progressData)
        {
            if (string.IsNullOrEmpty(progressData.NotificationMessage))
                return;

            DrawProgressForViews.ForNotificationArea(progressData);
        }

        static void DoLabel(string labelText)
        {
            GUIStyle labelStyle = EditorStyles.label;

            Rect rect = GUILayoutUtility.GetRect(
                new GUIContent(labelText),
                labelStyle);

            GUI.Label(rect, labelText, labelStyle);
        }

        static string DoTextField(
            string entryValue,
            bool enabled,
            float textBoxLeft,
            float textBoxWidth)
        {
            GUI.enabled = enabled;

            var rect = GUILayoutUtility.GetRect(
                new GUIContent(entryValue),
                UnityStyles.Dialog.EntryLabel);
            rect.width = textBoxWidth;
            rect.x = textBoxLeft;

            string result = GUI.TextField(rect, entryValue);

            GUI.enabled = true;

            return result;
        }

        static bool DoButton(
            string text,
            bool isEnabled,
            float buttonLeft,
            float buttonWidth)
        {
            GUI.enabled = isEnabled;

            var rect = GUILayoutUtility.GetRect(
                new GUIContent(text),
                UnityStyles.Dialog.EntryLabel);

            rect.width = buttonWidth;
            rect.x = buttonLeft;

            bool result = GUI.Button(rect, text);
            GUI.enabled = true;
            return result;
        }

        static bool DoRadioButton(
            string text,
            bool isChecked,
            bool isEnabled,
            float buttonLeft)
        {
            GUI.enabled = isEnabled;

            GUIStyle radioButtonStyle =
                EditorStyles.radioButton;

            var rect = GUILayoutUtility.GetRect(
                new GUIContent(text),
                radioButtonStyle);

            rect.x = buttonLeft;

            bool result = GUI.Toggle(
                rect,
                isChecked,
                text,
                radioButtonStyle);

            GUI.enabled = true;

            return result;
        }

        static void DoWarningLabel(
            string labelText,
            float labelLeft)
        {
            Rect rect = GUILayoutUtility.GetRect(
                new GUIContent(labelText),
                EditorStyles.label);

            rect.x = labelLeft;

            GUI.Label(rect,
                new GUIContent(labelText, Images.GetWarnIcon()),
                UnityStyles.IncomingChangesTab.HeaderWarningLabel);
        }

        static bool DoLinkLabel(
            string labelText,
            string linkText,
            float labelLeft)
        {
            GUIContent labelContent = new GUIContent(labelText);
            GUIStyle labelStyle = new GUIStyle(EditorStyles.miniLabel);
            labelStyle.richText = true;

            Rect rect = GUILayoutUtility.GetRect(
                labelContent,
                labelStyle);

            rect.x = labelLeft;

            Rect linkRect = GetLinkRect(
                labelText,
                linkText,
                labelContent,
                labelStyle,
                rect);

            EditorGUIUtility.AddCursorRect(linkRect, MouseCursor.Link);

            GUI.Label(rect, labelText, labelStyle);

            return Mouse.IsLeftMouseButtonPressed(Event.current)
                && linkRect.Contains(Event.current.mousePosition);
        }

        static Rect GetLinkRect(
            string labelText,
            string linkText,
            GUIContent labelContent,
            GUIStyle labelStyle,
            Rect rect)
        {
            int beginLinkChar = labelText.IndexOf(linkText);
            int endLinkChar = beginLinkChar + linkText.Length;

            Vector2 beginPos = labelStyle.GetCursorPixelPosition(
               rect, labelContent, beginLinkChar);
            Vector2 endPos = labelStyle.GetCursorPixelPosition(
               rect, labelContent, endLinkChar);

            Rect linkRect = new Rect(
                beginPos.x,
                beginPos.y,
                endPos.x - beginPos.x,
                labelStyle.lineHeight * 1.2f);

            return linkRect;
        }

        static bool IsValidState(
            CreateWorkspaceViewState state,
            string projectPath)
        {
            if (!ValidateRepositoryName(state.RepositoryName).IsValid)
                return false;

            if (!ValidateWorkspaceName(state.WorkspaceName).IsValid)
                return false;

            if (!ValidateWorkspacePath(state.WorkspacePath, projectPath).IsValid)
                return false;

            return true;
        }

        static ValidationResult ValidateRepositoryName(string repositoryName)
        {
            ValidationResult result = new ValidationResult();

            if (string.IsNullOrEmpty(repositoryName))
            {
                result.ErrorMessage = "Repository name cannot be empty";
                result.IsValid = false;
                return result;
            }

            result.IsValid = true;
            return result;
        }

        static ValidationResult ValidateWorkspaceName(string workspaceName)
        {
            ValidationResult result = new ValidationResult();

            if (string.IsNullOrEmpty(workspaceName))
            {
                result.ErrorMessage = "Workspace name cannot be empty";
                result.IsValid = false;
                return result;
            }

            result.IsValid = true;
            return result;
        }

        static ValidationResult ValidateWorkspacePath(
            string workspacePath,
            string projectPath)
        {
            ValidationResult result = new ValidationResult();

            if (string.IsNullOrEmpty(workspacePath))
            {
                result.ErrorMessage = "Path on disk cannot be empty";
                result.IsValid = false;
                return result;
            }

            if (!PathHelper.IsContainedOn(projectPath, workspacePath))
            {
                result.ErrorMessage = "The selected path must contain the Unity project path";
                result.IsValid = false;
                return result;
            }

            result.IsValid = true;
            return result;
        }

        class ValidationResult
        {
            internal string ErrorMessage;
            internal bool IsValid;
        }

        const float LABEL_WIDTH = 120;
        const float TEXTBOX_WIDTH = 450;
        const float BROWSE_BUTTON_WIDTH = 25;
        const float NEW_BUTTON_WIDTH = 60;
        const float BUTTON_MARGIN = 2;
        const float LABEL_MARGIN = 2;
        const float CREATE_WORKSPACE_BUTTON_WIDTH = 225;

        const string HELP_URL = @"https://www.plasticscm.com/book/#_full_workspaces_vs_partial_workspaces";
    }
}
