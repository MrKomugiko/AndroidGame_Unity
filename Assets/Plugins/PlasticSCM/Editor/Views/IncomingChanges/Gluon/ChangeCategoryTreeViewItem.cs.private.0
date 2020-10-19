using UnityEditor.IMGUI.Controls;

using PlasticGui.Gluon.WorkspaceWindow.Views.IncomingChanges;

namespace Codice.Views.IncomingChanges.Gluon
{
    internal class ChangeCategoryTreeViewItem : TreeViewItem
    {
        internal IncomingChangeCategory Category { get; private set; }

        internal ChangeCategoryTreeViewItem(int id, IncomingChangeCategory category)
            : base(id, 0, category.CategoryName)
        {
            Category = category;
        }
    }
}