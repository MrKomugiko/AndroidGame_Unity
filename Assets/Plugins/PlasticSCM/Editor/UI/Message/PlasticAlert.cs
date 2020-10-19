using UnityEditor;
using UnityEngine;

using Codice.Client.Common;
using PlasticGui;

namespace Codice.UI.Message
{
    internal class PlasticAlert : PlasticDialog
    {
        internal static void Show(
            string title, string message,
            GuiMessage.GuiMessageType alertType,
            EditorWindow parentWindow)
        {
            PlasticAlert alert = Create(title, message, alertType);
            alert.RunModal(parentWindow);
        }

        protected override PlasticDialog CloneModal()
        {
            return Create(mTitle, mMessage, mAlertType);
        }

        protected override void OnModalGUI()
        {
            DoMessageArea();

            GUILayout.FlexibleSpace();
            GUILayout.Space(20);

            DoButtonArea();
        }

        void DoMessageArea()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                DrawDialogIcon.ForMessage(mAlertType);

                using (new EditorGUILayout.VerticalScope())
                {
                    GUILayout.Label(mTitle, UnityStyles.Dialog.MessageTitle);

                    GUIContent message = new GUIContent(mMessage);

                    Rect lastRect = GUILayoutUtility.GetLastRect();
                    GUIStyle scrollPlaceholder = new GUIStyle(UnityStyles.Dialog.MessageText);
                    scrollPlaceholder.normal.textColor = Color.clear;
                    scrollPlaceholder.clipping = TextClipping.Clip;

                    if (Event.current.type == EventType.Repaint)
                    {
                        mMessageDesiredHeight = ((GUIStyle)UnityStyles.Dialog.MessageText)
                            .CalcHeight(message, lastRect.width - 20) + 20;
                        mMessageViewHeight = Mathf.Min(mMessageDesiredHeight, 500);
                    }

                    GUILayout.Space(mMessageViewHeight);

                    Rect scrollPanelRect = new Rect(
                        lastRect.xMin, lastRect.yMax,
                        lastRect.width + 20, mMessageViewHeight);

                    Rect contentRect = new Rect(
                        scrollPanelRect.xMin,
                        scrollPanelRect.yMin,
                        scrollPanelRect.width - 20,
                        mMessageDesiredHeight);

                    mScroll = GUI.BeginScrollView(scrollPanelRect, mScroll, contentRect);

                    GUI.Label(contentRect, mMessage, UnityStyles.Dialog.MessageText);

                    GUI.EndScrollView();
                };
            }
        }

        void DoButtonArea()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                if (!AcceptButton(PlasticLocalization.GetString(
                        PlasticLocalization.Name.CloseButton)))
                    return;

                CloseButtonAction();
            }
        }

        static PlasticAlert Create(string title, string message, GuiMessage.GuiMessageType alertType)
        {
            var instance = CreateInstance<PlasticAlert>();
            instance.titleContent = new GUIContent(title);
            instance.mTitle = title;
            instance.mMessage = message;
            instance.mAlertType = alertType;
            instance.mEnterKeyAction = instance.CloseButtonAction;
            instance.mEscapeKeyAction = instance.CloseButtonAction;
            return instance;
        }

        Vector2 mScroll;
        float mMessageDesiredHeight;
        float mMessageViewHeight;

        string mTitle;
        string mMessage;
        GuiMessage.GuiMessageType mAlertType;
    }
}
