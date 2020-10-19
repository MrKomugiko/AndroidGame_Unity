using UnityEditor.IMGUI.Controls;

using PlasticGui.WorkspaceWindow.PendingChanges;

namespace Codice.Views.PendingChanges
{
    internal class ChangeCategoryTreeViewItem : TreeViewItem
    {
        internal PendingChangeCategory Category { get; private set; }

        internal ChangeCategoryTreeViewItem(int id, PendingChangeCategory category)
            : base(id, 0, category.CategoryName)
        {
            Category = category;
        }
    }
}
