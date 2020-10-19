using UnityEngine;

using UnityEditor;

using Codice.UI;
using Codice.UI.Progress;
using PlasticGui;

namespace Codice.Configuration
{
    public class CredentialsDialog : PlasticDialog
    {
        protected override Rect DefaultRect
        {
            get
            {
                var baseRect = base.DefaultRect;
                return new Rect(baseRect.x, baseRect.y, 525, 240);
            }
        }

        public static CredentialsDialogData RequestCredentials(
            string server,
            EditorWindow parentWindow)
        {
            CredentialsDialog dialog = Create(
                server, new ProgressControlsForDialogs());

            ResponseType dialogResult = dialog.RunModal(parentWindow);

            CredentialsDialogData result = dialog.BuildCredentialsDialogData();

            result.Result = dialogResult == ResponseType.Ok;
            return result;
        }

        protected override PlasticDialog CloneModal()
        {
            return Create(mServer, mProgressControls);
        }

        protected override void OnModalGUI()
        {
            Title(PlasticLocalization.GetString(
                PlasticLocalization.Name.CredentialsDialogTitle));

            Paragraph(PlasticLocalization.GetString(
                PlasticLocalization.Name.CredentialsDialogExplanation, mServer));

            GUILayout.Space(5);

            DoEntriesArea();

            GUILayout.Space(10);

            DrawProgressForDialogs.For(
                mProgressControls.ProgressData);

            GUILayout.Space(10);

            DoButtonsArea();
        }

        CredentialsDialogData BuildCredentialsDialogData()
        {
            return new CredentialsDialogData(
                mUser, mPassword, mSaveProfile);
        }

        void DoEntriesArea()
        {
            mUser = TextEntry(PlasticLocalization.GetString(
                PlasticLocalization.Name.UserName), mUser,
                ENTRY_WIDTH, ENTRY_X);

            GUILayout.Space(5);

            mPassword = PasswordEntry(PlasticLocalization.GetString(
                PlasticLocalization.Name.Password), mPassword,
                ENTRY_WIDTH, ENTRY_X);

            GUILayout.Space(5);

            mSaveProfile = ToggleEntry(PlasticLocalization.GetString(
                PlasticLocalization.Name.RememberCredentialsAsProfile),
                mSaveProfile, ENTRY_WIDTH, ENTRY_X);
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

            OkButtonWithValidationAction();
        }

        void DoCancelButton()
        {
            if (!NormalButton(PlasticLocalization.GetString(
                    PlasticLocalization.Name.CancelButton)))
                return;

            CancelButtonAction();
        }

        void OkButtonWithValidationAction()
        {
            CredentialsDialogValidation.AsyncValidation(
                BuildCredentialsDialogData(), this, mProgressControls);
        }

        static CredentialsDialog Create(
            string server,
            ProgressControlsForDialogs progressControls)
        {
            var instance = CreateInstance<CredentialsDialog>();
            instance.mServer = server;
            instance.mProgressControls = progressControls;
            instance.mEnterKeyAction = instance.OkButtonWithValidationAction;
            instance.mEscapeKeyAction = instance.CancelButtonAction;
            return instance;
        }

        string mUser;
        string mPassword = string.Empty;

        ProgressControlsForDialogs mProgressControls;
        bool mSaveProfile;

        string mServer;

        const float ENTRY_WIDTH = 345f;
        const float ENTRY_X = 150f;
    }
}
