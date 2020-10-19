using System;
using System.Linq;

using UnityEditor.IMGUI.Controls;
using UnityEngine;

using Codice.UI.Tree;
using PlasticGui;

namespace Codice.Views.PendingChanges
{
    internal enum PendingChangesTreeColumn
    {
        Item,
        Status,
        Size,
        Extension,
        Type,
        DateModififed,
        Repository
    }

    [Serializable]
    internal class PendingChangesTreeHeaderState : MultiColumnHeaderState, ISerializationCallbackReceiver
    {
        internal static PendingChangesTreeHeaderState Default
        {
            get
            {
                var headerState = new PendingChangesTreeHeaderState(new Column[]
                {
                    new Column()
                    {
                        width = 440,
                        headerContent = new GUIContent(
                            GetHeaderContent(PendingChangesTreeColumn.Item)),
                        minWidth = 200,
                        allowToggleVisibility = false,
                    },
                    new Column()
                    {
                        width = 150,
                        headerContent = new GUIContent(
                            GetHeaderContent(PendingChangesTreeColumn.Status)),
                        minWidth = 80
                    },
                    new Column()
                    {
                        width = 80,
                        headerContent = new GUIContent(
                            GetHeaderContent(PendingChangesTreeColumn.Size)),
                        minWidth = 45
                    },
                    new Column()
                    {
                        width = 80,
                        headerContent = new GUIContent(
                            GetHeaderContent(PendingChangesTreeColumn.Extension)),
                        minWidth = 70
                    },
                    new Column()
                    {
                        width = 60,
                        headerContent = new GUIContent(
                            GetHeaderContent(PendingChangesTreeColumn.Type)),
                        minWidth = 45
                    },
                    new Column()
                    {
                        width = 260,
                        headerContent = new GUIContent(
                            GetHeaderContent(PendingChangesTreeColumn.DateModififed)),
                        minWidth = 100
                    },
                    new Column()
                    {
                        width = 210,
                        headerContent = new GUIContent(
                            GetHeaderContent(PendingChangesTreeColumn.Repository)),
                        minWidth = 90
                    }
                });

                // NOTE(rafa): we cannot ensure that the order in the list is the same as in the enum
                // take extra care modifying columns list or the enum
                if (headerState.columns.Length != Enum.GetNames(typeof(PendingChangesTreeColumn)).Length)
                    throw new InvalidOperationException("header columns and Column enum must have the same size and order");

                return headerState;
            }
        }

        internal static string GetHeaderContent(PendingChangesTreeColumn column)
        {
            switch (column)
            {
                case PendingChangesTreeColumn.Item:
                    return PlasticLocalization.GetString(PlasticLocalization.Name.ItemColumn);
                case PendingChangesTreeColumn.Status:
                    return PlasticLocalization.GetString(PlasticLocalization.Name.StatusColumn);
                case PendingChangesTreeColumn.Size:
                    return PlasticLocalization.GetString(PlasticLocalization.Name.SizeColumn);
                case PendingChangesTreeColumn.Extension:
                    return PlasticLocalization.GetString(PlasticLocalization.Name.ExtensionColumn);
                case PendingChangesTreeColumn.Type:
                    return PlasticLocalization.GetString(PlasticLocalization.Name.TypeColumn);
                case PendingChangesTreeColumn.DateModififed:
                    return PlasticLocalization.GetString(PlasticLocalization.Name.DateModifiedColumn);
                case PendingChangesTreeColumn.Repository:
                    return PlasticLocalization.GetString(PlasticLocalization.Name.RepositoryColumn);
                default:
                    return null;
            }
        }

        internal static void SetMode(MultiColumnHeaderState state, bool isGluonMode)
        {
            var result = state.visibleColumns.ToList();
            if (!result.Contains((int)PendingChangesTreeColumn.Item))
                result.Add((int)PendingChangesTreeColumn.Item);

            if (isGluonMode)
            {
                result.Remove((int)PendingChangesTreeColumn.Type);
                result.Remove((int)PendingChangesTreeColumn.Repository);
            }

            state.columns[(int)PendingChangesTreeColumn.Item].allowToggleVisibility = false;
            state.columns[(int)PendingChangesTreeColumn.Type].allowToggleVisibility = !isGluonMode;
            state.columns[(int)PendingChangesTreeColumn.Repository].allowToggleVisibility = !isGluonMode;
            state.visibleColumns = result.ToArray();
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

        PendingChangesTreeHeaderState(Column[] columns)
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
