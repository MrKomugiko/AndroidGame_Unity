using System;

using UnityEditor.IMGUI.Controls;
using UnityEngine;

using Codice.UI.Tree;
using PlasticGui;

namespace Codice.Views.IncomingChanges.Developer
{
    internal enum IncomingChangesTreeColumn
    {
        Path,
        Size,
        Author,
        Details,
        Resolution
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
                        width = 450,
                        headerContent = new GUIContent(
                            GetHeaderContent(IncomingChangesTreeColumn.Path)),
                        minWidth = 200,
                        allowToggleVisibility = false,
                    },
                    new Column()
                    {
                        width = 150,
                        headerContent = new GUIContent(
                            GetHeaderContent(IncomingChangesTreeColumn.Size)),
                        minWidth = 45
                    },
                    new Column()
                    {
                        width = 150,
                        headerContent = new GUIContent(
                            GetHeaderContent(IncomingChangesTreeColumn.Author)),
                        minWidth = 80
                    },
                    new Column()
                    {
                        width = 200,
                        headerContent = new GUIContent(
                            GetHeaderContent(IncomingChangesTreeColumn.Details)),
                        minWidth = 100
                    },
                    new Column()
                    {
                        width = 250,
                        headerContent = new GUIContent(
                            GetHeaderContent(IncomingChangesTreeColumn.Resolution)),
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
                case IncomingChangesTreeColumn.Size:
                    return PlasticLocalization.GetString(PlasticLocalization.Name.SizeColumn);
                case IncomingChangesTreeColumn.Author:
                    return PlasticLocalization.GetString(PlasticLocalization.Name.AuthorColumn);
                case IncomingChangesTreeColumn.Details:
                    return PlasticLocalization.GetString(PlasticLocalization.Name.DetailsColumn);
                case IncomingChangesTreeColumn.Resolution:
                    return PlasticLocalization.GetString(PlasticLocalization.Name.ResolutionMethodColumn);
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
