using UnityEngine;

using Codice.Client.BaseCommands;
using Codice.Client.Commands;
using Codice.ThemeImages;
using PlasticGui.WorkspaceWindow.PendingChanges;

namespace Codice.UI.Tree
{
    static class GetOverlayIcon
    {
        internal class Data
        {
            internal readonly Texture Texture;
            internal readonly float XOffset;
            internal readonly float YOffset;
            internal readonly float Size;

            internal Data(Texture texture, float xOffset, float yOffset, float size)
            {
                Texture = texture;
                XOffset = xOffset;
                YOffset = yOffset;
                Size = size;
            }
        }

        internal static Data ForConflict(bool isResolved)
        {
            if (isResolved)
                return BuildData.ForOk();

            return BuildData.ForNotOnDisk();
        }

        internal static Data ForChangeToApply(bool isXLink)
        {
            if (!isXLink)
                return null;

            return BuildData.ForXLink();
        }

        internal static Data ForChange(ChangeInfo changeInfo)
        {
            ItemIconImageType type = ChangeInfoView.
                GetIconImageType(changeInfo);

            if (ChangeTypesOperator.AreAllSet(
                    changeInfo.ChangeTypes, ChangeTypes.Added))
                return BuildData.ForAdded();

            switch (type)
            {
                case ItemIconImageType.Ignored:
                    return BuildData.ForIgnored();
                case ItemIconImageType.Private:
                    return BuildData.ForPrivated();
                case ItemIconImageType.Deleted:
                    return BuildData.ForDeleted();
                case ItemIconImageType.CheckedOut:
                    return BuildData.ForCheckedOut();
                default:
                    return null;
            }
        }

        static class BuildData
        {
            internal static Data ForOk()
            {
                return new Data(
                    Images.GetImage(Images.Name.Ok),
                    4f, 4f, SIZE);
            }

            internal static Data ForNotOnDisk()
            {
                return new Data(
                    Images.GetImage(Images.Name.NotOnDisk),
                    4f, 4f, SIZE);
            }

            internal static Data ForXLink()
            {
                return new Data(
                    Images.GetImage(Images.Name.XLink),
                    2f, 3f, SIZE);
            }

            internal static Data ForIgnored()
            {
                return new Data(
                    Images.GetImage(Images.Name.Ignored),
                    -8f, 3f, SIZE);
            }

            internal static Data ForPrivated()
            {
                return new Data(
                    Images.GetPrivatedOverlayIcon(),
                    GetBottomLeftXOffset(),
                    GetBottomLeftYOffset(),
                    SIZE);
            }

            internal static Data ForAdded()
            {
                return new Data(
                    Images.GetAddedOverlayIcon(),
                    GetTopLeftXOffset(),
                    GetTopLeftYOffset(),
                    SIZE);
            }

            internal static Data ForDeleted()
            {
                return new Data(
                    Images.GetDeletedOverlayIcon(),
                    GetTopLeftXOffset(),
                    GetTopLeftYOffset(),
                    SIZE);
            }

            internal static Data ForCheckedOut()
            {
                return new Data(
                    Images.GetCheckedOutOverlayIcon(),
                    GetTopLeftXOffset(),
                    GetTopLeftYOffset(),
                    SIZE);
            }

            static float GetTopLeftXOffset()
            {
                return -4f;
            }

            static float GetTopLeftYOffset()
            {
                return -1f;
            }

            static float GetBottomLeftXOffset()
            {
                return -4f;
            }

            static float GetBottomLeftYOffset()
            {
                return UnityConstants.TREEVIEW_ROW_HEIGHT - SIZE + 1f;
            }

            const float SIZE = 16;
        }
    }
}
