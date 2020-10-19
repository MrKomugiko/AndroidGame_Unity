using UnityEditor;

using Codice.Client.Common;
using Codice.UI;
using PlasticGui;

namespace Codice.Configuration
{
    internal class CredentialsUiImpl : ICredentialsUI
    {
        internal CredentialsUiImpl(EditorWindow parentWindow)
        {
            mParentWindow = parentWindow;
        }

        public CredentialsDialogData AskUserForCredentials(string servername)
        {
            CredentialsDialogData result = null;

            GUIActionRunner.RunGUIAction(delegate
            {
                result = CredentialsDialog.RequestCredentials(
                    servername, mParentWindow);
            });

            return result;
        }

        public void ShowSaveProfileErrorMessage(string message)
        {
            GUIActionRunner.RunGUIAction(delegate
            {
                GuiMessage.ShowError(string.Format(
                    PlasticLocalization.GetString(
                        PlasticLocalization.Name.CredentialsErrorSavingProfile),
                    message));
            });
        }

        EditorWindow mParentWindow;
    }
}
