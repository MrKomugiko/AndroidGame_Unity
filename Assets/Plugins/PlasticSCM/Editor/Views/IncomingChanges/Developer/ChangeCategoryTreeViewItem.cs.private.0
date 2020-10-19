using PlasticGui.WorkspaceWindow.IncomingChanges;
using UnityEditor.IMGUI.Controls;

namespace Codice.Views.IncomingChanges.Developer
{
    internal class ChangeCategoryTreeViewItem : TreeViewItem
    {
        internal IncomingChangesCategory Category { get; private set; }

        internal ChangeCategoryTreeViewItem(int id, IncomingChangesCategory category)
            : base(id, 0, category.CategoryType.ToString())
        {
            Category = category;
        }
    }
}
