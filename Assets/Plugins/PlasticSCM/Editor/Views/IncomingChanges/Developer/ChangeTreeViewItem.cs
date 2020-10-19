using PlasticGui.WorkspaceWindow.IncomingChanges;
using UnityEditor.IMGUI.Controls;

namespace Codice.Views.IncomingChanges.Developer
{
    internal class ChangeTreeViewItem : TreeViewItem
    {
        internal IncomingChangeInfo ChangeInfo { get; private set; }

        internal ChangeTreeViewItem(int id, IncomingChangeInfo change)
            : base(id, 1)
        {
            ChangeInfo = change;

            displayName = id.ToString();
        }
    }
}
