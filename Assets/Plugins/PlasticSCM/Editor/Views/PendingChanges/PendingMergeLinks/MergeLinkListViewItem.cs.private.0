using UnityEditor.IMGUI.Controls;

using Codice.UI;
using PlasticGui.WorkspaceWindow.PendingChanges;

namespace Codice.Views.PendingChanges.PendingMergeLinks
{
    internal class MergeLinkListViewItem : TreeViewItem
    {
        internal MountPendingMergeLink MergeLink { get; private set; }

        internal MergeLinkListViewItem(int id, MountPendingMergeLink mergeLink)
            : base(id, 0)
        {
            MergeLink = mergeLink;

            displayName = mergeLink.GetPendingMergeLinkText();
            icon = Images.GetImage(Images.Name.IconMergeLink);
        }
    }
}

