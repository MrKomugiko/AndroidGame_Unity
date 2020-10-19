using UnityEditor;

using Codice.Client.Common;
using Codice.UI.Message;
using PlasticGui;

namespace Codice.UI
{
    public class UnityPlasticGuiMessage : GuiMessage.IGuiMessage
    {
        public UnityPlasticGuiMessage(EditorWindow parentWindow)
        {
            mParentWindow = parentWindow;
        }

        void GuiMessage.IGuiMessage.ShowMessage(
            string title,
            string message,
            GuiMessage.GuiMessageType alertType)
        {
            string alertTypeText = string.Empty;

            switch (alertType)
            {
                case GuiMessage.GuiMessageType.Informational:
                    alertTypeText = "Information";
                    break;
                case GuiMessage.GuiMessageType.Warning:
                    alertTypeText = "Warning";
                    break;
                case GuiMessage.GuiMessageType.Critical:
                    alertTypeText = "Error";
                    break;
                case GuiMessage.GuiMessageType.Question:
                    alertTypeText = "Question";
                    break;
            }

            string dialogTitle = string.Format("{0} - {1}", alertTypeText, title);

            EditorUtility.DisplayDialog(
                dialogTitle,
                message,
                "Close");
        }

        void GuiMessage.IGuiMessage.ShowError(string message)
        {
            EditorUtility.DisplayDialog(
                "Error - Plastic SCM",
                message,
                "Close");
        }

        GuiMessage.GuiMessageResponseButton GuiMessage.IGuiMessage.ShowQuestion(
            string title,
            string message,
            string firstActionButton,
            string secondActionButton,
            string thirdActionButton,
            bool isFirstButtonEnabled)
        {
            ResponseType responseType = PlasticQuestionAlert.Show(
                title, message, firstActionButton,
                secondActionButton, thirdActionButton,
                isFirstButtonEnabled,
                GuiMessage.GuiMessageType.Question,
                mParentWindow);

            return GetResponse(responseType);
        }

        bool GuiMessage.IGuiMessage.ShowQuestion(
            string title,
            string message,
            string yesButton)
        {
            ResponseType responseType = PlasticQuestionAlert.Show(
                title, message, yesButton,
                PlasticLocalization.GetString(PlasticLocalization.Name.NoButton),
                null, true,
                GuiMessage.GuiMessageType.Question,
                mParentWindow);

            return GetResponse(responseType) == GuiMessage.GuiMessageResponseButton.First;
        }

        bool GuiMessage.IGuiMessage.ShowYesNoQuestion(string title, string message)
        {
            ResponseType responseType = PlasticQuestionAlert.Show(
                title, message,
                PlasticLocalization.GetString(PlasticLocalization.Name.YesButton),
                PlasticLocalization.GetString(PlasticLocalization.Name.NoButton),
                null, true,
                GuiMessage.GuiMessageType.Question,
                mParentWindow);

            return GetResponse(responseType) == GuiMessage.GuiMessageResponseButton.First;
        }

        GuiMessage.GuiMessageResponseButton GuiMessage.IGuiMessage.ShowYesNoCancelQuestion(
            string title, string message)
        {
            ResponseType responseType = PlasticQuestionAlert.Show(
                title, message,
                PlasticLocalization.GetString(PlasticLocalization.Name.YesButton),
                PlasticLocalization.GetString(PlasticLocalization.Name.NoButton),
                PlasticLocalization.GetString(PlasticLocalization.Name.CancelButton),
                true,
                GuiMessage.GuiMessageType.Question,
                mParentWindow);

            return GetResponse(responseType);
        }

        bool GuiMessage.IGuiMessage.ShowYesNoQuestionWithType(
            string title, string message, GuiMessage.GuiMessageType messageType)
        {
            ResponseType responseType = PlasticQuestionAlert.Show(
                title, message,
                PlasticLocalization.GetString(PlasticLocalization.Name.YesButton),
                PlasticLocalization.GetString(PlasticLocalization.Name.NoButton),
                null, true, messageType,
                mParentWindow);

            return GetResponse(responseType) == GuiMessage.GuiMessageResponseButton.First;
        }

        static GuiMessage.GuiMessageResponseButton GetResponse(ResponseType dialogResult)
        {
            switch (dialogResult)
            {
                case ResponseType.Ok:
                    return GuiMessage.GuiMessageResponseButton.First;
                case ResponseType.Cancel:
                    return GuiMessage.GuiMessageResponseButton.Second;
                case ResponseType.Apply:
                    return GuiMessage.GuiMessageResponseButton.Third;
                default:
                    return GuiMessage.GuiMessageResponseButton.Second;
            }
        }

        EditorWindow mParentWindow;
    }
}
