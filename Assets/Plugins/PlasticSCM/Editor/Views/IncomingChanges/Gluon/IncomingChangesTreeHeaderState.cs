using System;

using UnityEditor.IMGUI.Controls;
using UnityEngine;

using Codice.UI.Tree;
using PlasticGui;

namespace Codice.Views.IncomingChanges.Gluon
{
    internal enum IncomingChangesTreeColumn
    {
        Path,
        LastEditedBy,
        Size,
        DateModififed
    }

    [Serializable]
    internal class IncomingChangesTreeHeaderState : MultiColumnHeaderState, ISerializationCallbackReceiver
    {
        internal static IncomingChangesTreeHeaderState Default
        {
            get
            {
                var headerState = new IncomingChangesTreeHeaderState(new Column[]
                {
                    new Column()
                    {
                        width = 440,
                        headerContent = new GUIContent(
                            GetHeaderContent(IncomingChangesTreeColumn.Path)),
                        minWidth = 200,
                        allowToggleVisibility = false,
                    },
                    new Column()
                    {
                        width = 150,
                        headerContent = new GUIContent(
                            GetHeaderContent(IncomingChangesTreeColumn.LastEditedBy)),
                        minWidth = 80
                    },
                    new Column()
                    {
                        width = 80,
                        headerContent = new GUIContent(
                            GetHeaderContent(IncomingChangesTreeColumn.Size)),
                        minWidth = 45
                    },
                    new Column()
                    {
                        width = 260,
                        headerContent = new GUIContent(
                            GetHeaderContent(IncomingChangesTreeColumn.DateModififed)),
                        minWidth = 100
                    }
                });

                // NOTE(rafa): we cannot ensure that the order in the list is the same as in the enum
                // take extra care modifying columns list or the enum
                if (headerState.columns.Length != Enum.GetNames(typeof(IncomingChangesTreeColumn)).Length)
                    throw new InvalidOperationException("header columns and Column enum must have the same size and order");

                return headerState;
            }
        }

        internal static string GetHeaderContent(IncomingChangesTreeColumn column)
        {
            switch (column)
            {
                case IncomingChangesTreeColumn.Path:
                    return PlasticLocalization.GetString(PlasticLocalization.Name.PathColumn);
                case IncomingChangesTreeColumn.LastEditedBy:
                    return PlasticLocalization.GetString(PlasticLocalization.Name.LastEditedByColumn);
                case IncomingChangesTreeColumn.Size:
                    return PlasticLocalization.GetString(PlasticLocalization.Name.SizeColumn);
                case IncomingChangesTreeColumn.DateModififed:
                    return PlasticLocalization.GetString(PlasticLocalization.Name.DateModifiedColumn);
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

        IncomingChangesTreeHeaderState(Column[] columns)
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
