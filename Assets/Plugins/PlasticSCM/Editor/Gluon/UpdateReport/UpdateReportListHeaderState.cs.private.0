using System;

using UnityEditor.IMGUI.Controls;
using UnityEngine;

using Codice.UI.Tree;
using PlasticGui;

namespace Codice.Gluon.UpdateReport
{
    internal enum UpdateReportListColumn
    {
        Path
    }

    [Serializable]
    internal class UpdateReportListHeaderState : MultiColumnHeaderState, ISerializationCallbackReceiver
    {
        internal static UpdateReportListHeaderState Default
        {
            get
            {
                var headerState = new UpdateReportListHeaderState(new Column[]
                {
                    new Column()
                    {
                        width = 600,
                        headerContent = new GUIContent(
                            GetHeaderContent(UpdateReportListColumn.Path)),
                        minWidth = 200,
                        allowToggleVisibility = false,
                        canSort = false
                    }
                });

                // NOTE(rafa): we cannot ensure that the order in the list is the same as in the enum
                // take extra care modifying columns list or the enum
                if (headerState.columns.Length != Enum.GetNames(typeof(UpdateReportListColumn)).Length)
                    throw new InvalidOperationException("header columns and Column enum must have the same size and order");

                return headerState;
            }
        }

        internal static string GetHeaderContent(UpdateReportListColumn column)
        {
            switch (column)
            {
                case UpdateReportListColumn.Path:
                    return PlasticLocalization.GetString(PlasticLocalization.Name.PathColumn);
                default:
                    return null;
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (mHeaderTitles != null)
                TreeHeaderColumns.SetTitles(columns, mHeaderTitles);
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }

        UpdateReportListHeaderState(Column[] columns)
            : base(columns)
        {
            if (mHeaderTitles == null)
                mHeaderTitles = TreeHeaderColumns.GetTitles(columns);
        }

        [SerializeField]
        string[] mHeaderTitles;
    }
}
