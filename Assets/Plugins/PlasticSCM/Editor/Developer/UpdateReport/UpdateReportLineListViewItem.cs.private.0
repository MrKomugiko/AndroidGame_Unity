using UnityEditor.IMGUI.Controls;

using Codice.Client.BaseCommands;

namespace Codice.Developer.UpdateReport
{
    internal class UpdateReportLineListViewItem : TreeViewItem
    {
        internal ReportLine ReportLine { get; private set; }

        internal UpdateReportLineListViewItem(int id, ReportLine reportLine)
            : base(id, 0)
        {
            ReportLine = reportLine;

            displayName = reportLine.ItemPath;
        }
    }
}
