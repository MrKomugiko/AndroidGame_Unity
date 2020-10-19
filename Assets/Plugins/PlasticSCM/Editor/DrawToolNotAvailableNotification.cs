using UnityEditor;
using UnityEngine;

using Codice.UI;
using PlasticGui;

namespace Codice
{
    internal class DrawToolNotAvailableNotification
    {
        internal static void ForMode(float width, bool isGluonMode)
        {
            EditorGUILayout.BeginVertical();

            Rect headerRect = DoPlasticHeaderImage(width);

            GUILayout.Space(headerRect.height + 10);

            DoActionHelpBox(isGluonMode);

            GUILayout.FlexibleSpace();

            EditorGUILayout.EndVertical();
        }

        static Rect DoPlasticHeaderImage(float width)
        {
            Texture2D image = Images.GetImage(Images.Name.PlasticHeader);

            Rect headerRect = GetImageRect(image, width);

            EditorGUI.DrawPreviewTexture(headerRect, image);

            return headerRect;
        }

        static void DoActionHelpBox(bool isGluonMode)
        {
            string labelText = GetLabelText(isGluonMode);

            string buttonText = PlasticLocalization.GetString(
                PlasticLocalization.Name.GoButton);

            DrawActionHelpBox.For(
                Images.GetWarnDialogIcon(), labelText, buttonText,
                DoGoButtonAction);
        }

        static void DoGoButtonAction()
        {
            Application.OpenURL(DOWNLOAD_URL);
        }

        static Rect GetImageRect(Texture2D image, float width)
        {
            float ratio = (image.height * width) / (image.width * 100f);
            float headerHeight = ratio * image.height;

            return new Rect(0, 0, width, headerHeight);
        }

        static string GetLabelText(bool isGluonMode)
        {
            string plasticFlavorName = isGluonMode ?
                "Gluon" : "Plastic SCM";

            return string.Format(
                "Please install {0} to enable the plugin. " +
                "You can download and install it by clicking here.",
                plasticFlavorName);
        }

        static string DOWNLOAD_URL = "https://www.plasticscm.com/download";
    }
}
