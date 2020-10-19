using System.IO;
using System.Reflection;

using log4net;

using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

using Codice.Utils;
using PlasticGui;

namespace Codice.UI
{
    public class Images
    {
        public enum Name
        {
            None,

            PlasticHeader,
            IconPlastic,
            IconCloseButton,
            IconPressedCloseButton,
            IconAdded,
            IconDeleted,
            IconChanged,
            IconMoved,
            IconMergeLink,
            Ignored,
            IconMergeConflict,
            XLink,
            Ok,
            NotOnDisk,
            IconRepository,

            //owls (provisional location)
            GenericBuho1,
            GenericBuhoShe1
        }

        public static Texture2D GetImage(Name image)
        {
            string imageFileName = image.ToString().ToLower() + ".png";
            string imageFilePath = GetFilePath(imageFileName);

            if (!File.Exists(imageFilePath))
            {
                mLog.WarnFormat("Image not found: {0}", imageFileName);
                return GetEmptyImage();
            }

            return AssetDatabase.LoadAssetAtPath<Texture2D>(imageFilePath);
        }

        internal static Texture2D GetFileIcon(string path)
        {
            string relativePath = GetRelativePath.ToApplication(path);

            Texture2D result = InternalEditorUtility.GetIconForFile(relativePath);

            if (result == null)
                return GetFileIcon();

            return result;
        }

        internal static Texture2D GetDropDownIcon()
        {
            if (mPopupIcon == null)
                mPopupIcon = EditorGUIUtility.FindTexture("icon dropdown");

            return mPopupIcon;
        }

        internal static Texture2D GetDirectoryIcon()
        {
            if (mDirectoryIcon == null)
                mDirectoryIcon = EditorGUIUtility.FindTexture("Folder Icon");

            return mDirectoryIcon;
        }

        internal static Texture2D GetPrivatedOverlayIcon()
        {
            if (mPrivatedOverlayIcon == null)
                mPrivatedOverlayIcon = EditorGUIUtility.FindTexture("d_P4_Local");

            return mPrivatedOverlayIcon;
        }

        internal static Texture2D GetAddedOverlayIcon()
        {
            if (mAddedOverlayIcon == null)
                mAddedOverlayIcon = EditorGUIUtility.FindTexture("d_P4_AddedLocal");

            return mAddedOverlayIcon;
        }

        internal static Texture2D GetDeletedOverlayIcon()
        {
            if (mDeletedOverlayIcon == null)
                mDeletedOverlayIcon = EditorGUIUtility.FindTexture("d_P4_DeletedLocal");

            return mDeletedOverlayIcon;
        }

        internal static Texture2D GetCheckedOutOverlayIcon()
        {
            if (mCheckedOutOverlayIcon == null)
                mCheckedOutOverlayIcon = EditorGUIUtility.FindTexture("d_P4_CheckOutLocal");

            return mCheckedOutOverlayIcon;
        }

        internal static Texture2D GetWarnIcon()
        {
            if (mWarnIcon == null)
                mWarnIcon = EditorGUIUtility.FindTexture("console.warnicon.sml");

            return mWarnIcon;
        }

        internal static Texture2D GetInfoIcon()
        {
            if (mInfoIcon == null)
                mInfoIcon = EditorGUIUtility.FindTexture("console.infoicon.sml");

            return mInfoIcon;
        }

        internal static Texture2D GetErrorDialogIcon()
        {
            if (mErrorDialogIcon == null)
                mErrorDialogIcon = EditorGUIUtility.FindTexture("console.erroricon");

            return mErrorDialogIcon;
        }

        internal static Texture2D GetWarnDialogIcon()
        {
            if (mWarnDialogIcon == null)
                mWarnDialogIcon = EditorGUIUtility.FindTexture("console.warnicon");

            return mWarnDialogIcon;
        }

        internal static Texture2D GetInfoDialogIcon()
        {
            if (mInfoDialogIcon == null)
                mInfoDialogIcon = EditorGUIUtility.FindTexture("console.infoicon");

            return mInfoDialogIcon;
        }

        internal static Texture2D GetRefreshIcon()
        {
            if (mRefreshIcon == null)
                mRefreshIcon = EditorGUIUtility.FindTexture("Refresh");

            return mRefreshIcon;
        }

        static string GetFilePath(string imageFileName)
        {
            string path = Path.Combine(
                AssemblyLocation.GetAssemblyParentDirectory(
                    Assembly.GetAssembly(typeof(PlasticLocalization))),
                "PlasticImage");

            string relativePath = GetRelativePath.ToApplication(path);

            return Path.Combine(relativePath, imageFileName);
        }

        static Texture2D GetEmptyImage()
        {
            if (mEmptyImage == null)
            {
                mEmptyImage = new Texture2D(1, 1);
                mEmptyImage.SetPixel(0, 0, Color.clear);
                mEmptyImage.Apply();
            }

            return mEmptyImage;
        }

        static Texture2D GetFileIcon()
        {
            if (mFileIcon == null)
                mFileIcon = EditorGUIUtility.FindTexture("DefaultAsset Icon");

            return mFileIcon;
        }

        static Texture2D mFileIcon;
        static Texture2D mDirectoryIcon;

        static Texture2D mPrivatedOverlayIcon;
        static Texture2D mAddedOverlayIcon;
        static Texture2D mDeletedOverlayIcon;
        static Texture2D mCheckedOutOverlayIcon;

        static Texture2D mWarnIcon;
        static Texture2D mInfoIcon;

        static Texture2D mErrorDialogIcon;
        static Texture2D mWarnDialogIcon;
        static Texture2D mInfoDialogIcon;

        static Texture2D mRefreshIcon;

        static Texture2D mEmptyImage;

        static Texture2D mPopupIcon;

        static readonly ILog mLog = LogManager.GetLogger("Images");
    }
}
