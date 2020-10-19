using UnityEditor;

using Codice.Client.Common.Encryption;
using Codice.UI;
using PlasticGui;

namespace Codice.Configuration
{
    internal class MissingEncryptionPasswordPromptHandler :
        ClientEncryptionServiceProvider.IEncryptioPasswordProvider
    {
        internal MissingEncryptionPasswordPromptHandler(EditorWindow parentWindow)
        {
            mParentWindow = parentWindow;
        }

        public string GetEncryptionEncryptedPassword(string server)
        {
            string result = null;

            GUIActionRunner.RunGUIAction(delegate
            {
                result = AskForEncryptionPassword(server);
            });

            return result;
        }

        string AskForEncryptionPassword(string server)
        {
            EncryptionConfigurationDialogData dialogData =
                EncryptionConfigurationDialog.RequestEncryptionPassword(server, mParentWindow);

            if (!dialogData.Result)
                return null;

            return dialogData.EncryptedPassword;
        }

        EditorWindow mParentWindow;
    }
}
