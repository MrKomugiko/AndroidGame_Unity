using System;

using UnityEditor.IMGUI.Controls;
using UnityEngine;

using Codice.UI.Tree;
using PlasticGui;

namespace Codice.Views.IncomingChanges.Gluon.Errors
{
    internal enum ErrorsListColumn
    {
        Path,
        Reason
    }

    [Serializable]
    internal class ErrorsListHeaderState : MultiColumnHeaderState, ISerializationCallbackReceiver
    {
        internal static ErrorsListHeaderState Default
        {
            get
            {
                var headerState = new ErrorsListHeaderState(new Column[]
                {
                    new Column()
                    {
                        width = 300,
                        headerContent = new GUIContent(
                            GetHeaderContent(ErrorsListColumn.Path)),
                        minWidth = 200,
                        allowToggleVisibility = false,
                        canSort = false
                    },
                    new Column()
                    {
                        width = 600,
                        headerContent = new GUIContent(
                            GetHeaderContent(ErrorsListColumn.Reason)),
                        minWidth = 200,
                        canSort = false
                    }
                });

                // NOTE(rafa): we cannot ensure that the order in the list is the same as in the enum
                // take extra care modifying columns list or the enum
                if (headerState.columns.Length != Enum.GetNames(typeof(ErrorsListColumn)).Length)
                    throw new InvalidOperationException("header columns and Column enum must have the same size and order");

                return headerState;
            }
        }

        internal static string GetHeaderContent(ErrorsListColumn column)
        {
            switch (column)
            {
                case ErrorsListColumn.Path:
                    return PlasticLocalization.GetString(PlasticLocalization.Name.PathColumn);
                case ErrorsListColumn.Reason:
                    return PlasticLocalization.GetString(PlasticLocalization.Name.Reason);
                default:
                    return null;
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (mHeaderTitles != null)
                TreeHeaderColumns.SetTitles(columns, mHeaderTitles);

            if (mColumsAllowedToggleVisibility != null)
                TreeHeaderColumns.SetVisibilities(columns, mColumsAllowedToggleVisibility);
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }

        ErrorsListHeaderState(Column[] columns)
            : base(columns)
        {
            if (mHeaderTitles == null)
                mHeaderTitles = TreeHeaderColumns.GetTitles(columns);

            if (mColumsAllowedToggleVisibility == null)
                mColumsAllowedToggleVisibility = TreeHeaderColumns.GetVisibilities(columns);
        }

        [SerializeField]
        string[] mHeaderTitles;

        [SerializeField]
        bool[] mColumsAllowedToggleVisibility;
    }
}
