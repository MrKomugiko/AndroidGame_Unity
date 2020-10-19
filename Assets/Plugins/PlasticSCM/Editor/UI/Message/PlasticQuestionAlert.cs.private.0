using System;

using UnityEditor;
using UnityEngine;

using Codice.Client.Common;

namespace Codice.UI.Message
{
    internal class PlasticQuestionAlert : PlasticDialog
    {
        protected override Rect DefaultRect
        {
            get
            {
                var baseRect = base.DefaultRect;

                string buttonsText = mFirst + mSecond + (mThird ?? string.Empty);

                int textWidth = (int)((GUIStyle)UnityStyles.Dialog.AcceptButtonText)
                    .CalcSize(new GUIContent(buttonsText)).x;

                return new Rect(
                    baseRect.x, baseRect.y,
                    Math.Max(500, textWidth + 150), 180);
            }
        }

        internal static ResponseType Show(
            string title,
            string message, string first,
            string second, string third,
            bool isFirstButtonEnabled,
            GuiMessage.GuiMessageType alertType,
            EditorWindow parentWindow)
        {
            PlasticQuestionAlert alert = Create(
                title, message, first, second, third,
                isFirstButtonEnabled, alertType);
            return alert.RunModal(parentWindow);
        }

        protected override PlasticDialog CloneModal()
        {
            return Create(
                mTitle, mMessage, mFirst,
                mSecond, mThird, mIsFirstButtonEnabled,
                mAlertType);
        }

        protected override void OnModalGUI()
        {
            DoMessageArea();

            GUILayout.FlexibleSpace();

            GUILayout.Space(20);

            DoButtonsArea();
        }

        void DoMessageArea()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                DrawDialogIcon.ForMessage(mAlertType);

                using (new EditorGUILayout.VerticalScope())
                {
                    GUILayout.Label(mTitle, UnityStyles.Dialog.MessageTitle);
                    GUILayout.Label(mMessage, UnityStyles.Dialog.MessageText);
                }
            }
        }

        void DoButtonsArea()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                if (Application.platform == RuntimePlatform.WindowsEditor)
                {
                    DoFirstButton();
                    DoSecondButton();
                    DoThirdButton();
                    return;
                }

                DoThirdButton();
                DoSecondButton();
                DoFirstButton();
            }
        }

        void DoFirstButton()
        {
            GUI.enabled = mIsFirstButtonEnabled;

            bool pressed = mIsFirstButtonEnabled ?
                AcceptButton(mFirst) :
                NormalButton(mFirst);

            GUI.enabled = true;

            if (!pressed)
                return;

            OkButtonAction();
        }

        void DoSecondButton()
        {
            if (!NormalButton(mSecond))
                return;

            CancelButtonAction();
        }

        void DoThirdButton()
        {
            if (mThird == null)
                return;

            bool pressed = mIsFirstButtonEnabled ?
                NormalButton(mThird) :
                AcceptButton(mThird);

            if (!pressed)
                return;

            ApplyButtonAction();
        }

        static PlasticQuestionAlert Create(
            string title, string message, string first,
            string second, string third, bool isFirstButtonEnabled,
            GuiMessage.GuiMessageType alertType)
        {
            var instance = CreateInstance<PlasticQuestionAlert>();
            instance.titleContent = new GUIContent(title);
            instance.mTitle = title;
            instance.mMessage = message;
            instance.mFirst = first;
            instance.mSecond = second;
            instance.mThird = third;
            instance.mIsFirstButtonEnabled = isFirstButtonEnabled;
            instance.mAlertType = alertType;
            instance.mEnterKeyAction = GetEnterKeyAction(isFirstButtonEnabled, instance);
            instance.mEscapeKeyAction = instance.CancelButtonAction;
            return instance;
        }

        static Action GetEnterKeyAction(
            bool isFirstButtonEnabled,
            PlasticQuestionAlert instance)
        {
            if (isFirstButtonEnabled)
                return instance.OkButtonAction;

            return instance.ApplyButtonAction;
        }

        string mTitle;
        string mMessage, mFirst, mSecond, mThird;
        bool mIsFirstButtonEnabled;
        GuiMessage.GuiMessageType mAlertType;
    }
}
