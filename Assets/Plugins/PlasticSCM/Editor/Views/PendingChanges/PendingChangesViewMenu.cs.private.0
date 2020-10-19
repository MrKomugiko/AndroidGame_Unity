using UnityEditor;
using UnityEngine;

using Codice.UI;
using PlasticGui;
using PlasticGui.WorkspaceWindow.Items;
using PlasticGui.WorkspaceWindow.PendingChanges;

namespace Codice.Views.PendingChanges
{
    internal class PendingChangesViewMenu
    {
        internal interface IMetaMenuOperations
        {
            void DiffMeta();
            void OpenMeta();
            void OpenMetaWith();
            void OpenMetaInExplorer();
            bool SelectionHasMeta();
        }

        internal PendingChangesViewMenu(
            IPendingChangesMenuOperations pendingChangesMenuOperations,
            IMetaMenuOperations metaMenuOperations,
            IFilesFilterPatternsMenuOperations filterMenuOperations)
        {
            mPendingChangesMenuOperations = pendingChangesMenuOperations;
            mMetaMenuOperations = metaMenuOperations;

            mFilterMenuBuilder = new FilesFilterPatternsMenuBuilder(filterMenuOperations);

            BuildComponents();
        }

        internal void Popup()
        {
            GenericMenu menu = new GenericMenu();

            UpdateMenuItems(menu);

            menu.ShowAsContext();
        }

        internal bool ProcessKeyActionIfNeeded(Event e)
        {
            PendingChangesMenuOperations operationToExecute = GetMenuOperation(e);

            if (operationToExecute == PendingChangesMenuOperations.None)
                return false;

            SelectedChangesGroupInfo info =
                mPendingChangesMenuOperations.GetSelectedChangesGroupInfo();
            PendingChangesMenuOperations operations =
                PendingChangesMenuUpdater.GetAvailableMenuOperations(info);

            if (!operations.HasFlag(operationToExecute))
                return false;

            ProcessMenuOperation(operationToExecute, mPendingChangesMenuOperations);
            return true;
        }

        void OpenMenuItem_Click()
        {
            mPendingChangesMenuOperations.Open();
        }

        void OpenWithMenuItem_Click()
        {
            mPendingChangesMenuOperations.OpenWith();
        }

        void OpenInExplorerMenuItem_Click()
        {
            mPendingChangesMenuOperations.OpenInExplorer();
        }

        void OpenMetaMenuItem_Click()
        {
            mMetaMenuOperations.OpenMeta();
        }

        void OpenMetaWithMenuItem_Click()
        {
            mMetaMenuOperations.OpenMetaWith();
        }

        void OpenMetaInExplorerMenuItem_Click()
        {
            mMetaMenuOperations.OpenMetaInExplorer();
        }

        void DiffMenuItem_Click()
        {
            mPendingChangesMenuOperations.Diff();
        }

        void DiffMetaMenuItem_Click()
        {
            mMetaMenuOperations.DiffMeta();
        }

        void UndoChangesMenuItem_Click()
        {
            mPendingChangesMenuOperations.UndoChanges();
        }

        void CheckoutMenuItem_Click()
        {
            mPendingChangesMenuOperations.ApplyLocalChanges();
        }

        void DeleteMenuItem_Click()
        {
            mPendingChangesMenuOperations.Delete();
        }

        void UpdateMenuItems(GenericMenu menu)
        {
            SelectedChangesGroupInfo info =
                mPendingChangesMenuOperations.GetSelectedChangesGroupInfo();
            PendingChangesMenuOperations operations =
                PendingChangesMenuUpdater.GetAvailableMenuOperations(info);

            if (operations == PendingChangesMenuOperations.None)
            {
                menu.AddDisabledItem(GetNoActionMenuItemContent());
                return;
            }

            UpdateOpenMenuItems(menu, operations);

            menu.AddSeparator(string.Empty);

            if (operations.HasFlag(PendingChangesMenuOperations.DiffWorkspaceContent))
                menu.AddItem(mDiffMenuItemContent, false, DiffMenuItem_Click);
            else
                menu.AddDisabledItem(mDiffMenuItemContent);

            if (mMetaMenuOperations.SelectionHasMeta())
            {
                if (operations.HasFlag(PendingChangesMenuOperations.DiffWorkspaceContent))
                    menu.AddItem(mDiffMetaMenuItemContent, false, DiffMetaMenuItem_Click);
                else
                    menu.AddDisabledItem(mDiffMetaMenuItemContent);
            }

            menu.AddSeparator(string.Empty);

            if (operations.HasFlag(PendingChangesMenuOperations.UndoChanges))
                menu.AddItem(mUndoChangesMenuItemContent, false, UndoChangesMenuItem_Click);
            else
                menu.AddDisabledItem(mUndoChangesMenuItemContent);

            menu.AddSeparator(string.Empty);

            if (operations.HasFlag(PendingChangesMenuOperations.ApplyLocalChanges))
                menu.AddItem(mCheckoutMenuItemContent, false, CheckoutMenuItem_Click);
            else
                menu.AddDisabledItem(mCheckoutMenuItemContent);

            if (operations.HasFlag(PendingChangesMenuOperations.Delete))
                menu.AddItem(mDeleteMenuItemContent, false, DeleteMenuItem_Click);
            else
                menu.AddDisabledItem(mDeleteMenuItemContent);

            menu.AddSeparator(string.Empty);

            mFilterMenuBuilder.UpdateMenuItems(
                menu, FilterMenuUpdater.GetMenuActions(info));
        }

        void UpdateOpenMenuItems(GenericMenu menu, PendingChangesMenuOperations operations)
        {
            if (!operations.HasFlag(PendingChangesMenuOperations.Open) &&
                !operations.HasFlag(PendingChangesMenuOperations.OpenWith) &&
                !operations.HasFlag(PendingChangesMenuOperations.OpenInExplorer))
            {
                menu.AddDisabledItem(mOpenSubmenuItemContent);
                return;
            }

            if (operations.HasFlag(PendingChangesMenuOperations.Open))
                menu.AddItem(mOpenMenuItemContent, false, OpenMenuItem_Click);
            else
                menu.AddDisabledItem(mOpenMenuItemContent);

            if (operations.HasFlag(PendingChangesMenuOperations.OpenWith))
                menu.AddItem(mOpenWithMenuItemContent, false, OpenWithMenuItem_Click);
            else
                menu.AddDisabledItem(mOpenWithMenuItemContent);

            if (operations.HasFlag(PendingChangesMenuOperations.OpenInExplorer))
                menu.AddItem(mOpenInExplorerMenuItemContent, false, OpenInExplorerMenuItem_Click);
            else
                menu.AddDisabledItem(mOpenInExplorerMenuItemContent);

            if (!mMetaMenuOperations.SelectionHasMeta())
                return;

            menu.AddSeparator(PlasticLocalization.GetString(PlasticLocalization.Name.ItemsMenuItemOpen) + "/");

            if (operations.HasFlag(PendingChangesMenuOperations.Open))
                menu.AddItem(mOpenMetaMenuItemContent, false, OpenMetaMenuItem_Click);
            else
                menu.AddDisabledItem(mOpenMetaMenuItemContent);

            if (operations.HasFlag(PendingChangesMenuOperations.OpenWith))
                menu.AddItem(mOpenMetaWithMenuItemContent, false, OpenMetaWithMenuItem_Click);
            else
                menu.AddDisabledItem(mOpenMetaWithMenuItemContent);

            if (operations.HasFlag(PendingChangesMenuOperations.OpenInExplorer))
                menu.AddItem(mOpenMetaInExplorerMenuItemContent, false, OpenMetaInExplorerMenuItem_Click);
            else
                menu.AddDisabledItem(mOpenMetaInExplorerMenuItemContent);
        }

        GUIContent GetNoActionMenuItemContent()
        {
            if (mNoActionMenuItemContent == null)
            {
                mNoActionMenuItemContent = new GUIContent(PlasticLocalization.GetString(
                    PlasticLocalization.Name.NoActionMenuItem));
            }

            return mNoActionMenuItemContent;
        }

        void BuildComponents()
        {
            mOpenSubmenuItemContent = new GUIContent(
                PlasticLocalization.GetString(PlasticLocalization.Name.ItemsMenuItemOpen));
            mOpenMenuItemContent = new GUIContent(
                UnityMenuItem.GetText(
                    PlasticLocalization.GetString(PlasticLocalization.Name.ItemsMenuItemOpen),
                    PlasticLocalization.GetString(PlasticLocalization.Name.ItemsMenuItemOpen))
                + " #o");
            mOpenWithMenuItemContent = new GUIContent(
                UnityMenuItem.GetText(
                    PlasticLocalization.GetString(PlasticLocalization.Name.ItemsMenuItemOpen),
                    PlasticLocalization.GetString(PlasticLocalization.Name.ItemsMenuItemOpenWith)));
            mOpenInExplorerMenuItemContent = new GUIContent(
                UnityMenuItem.GetText(
                    PlasticLocalization.GetString(PlasticLocalization.Name.ItemsMenuItemOpen),
                    PlasticLocalization.GetString(PlasticLocalization.Name.OpenInExplorerMenuItem)));
            mOpenMetaMenuItemContent = new GUIContent(
                UnityMenuItem.GetText(
                    PlasticLocalization.GetString(PlasticLocalization.Name.ItemsMenuItemOpen),
                    "Open .meta"));
            mOpenMetaWithMenuItemContent = new GUIContent(
                UnityMenuItem.GetText(
                    PlasticLocalization.GetString(PlasticLocalization.Name.ItemsMenuItemOpen),
                    "Open .meta with..."));
            mOpenMetaInExplorerMenuItemContent = new GUIContent(
                UnityMenuItem.GetText(
                    PlasticLocalization.GetString(PlasticLocalization.Name.ItemsMenuItemOpen),
                    "Open .meta in explorer"));
            mDiffMenuItemContent = new GUIContent("Diff #d");
            mDiffMetaMenuItemContent = new GUIContent("Diff .meta");
            mUndoChangesMenuItemContent = new GUIContent(
                PlasticLocalization.GetString(PlasticLocalization.Name.UndoChanges));
            mCheckoutMenuItemContent = new GUIContent(
                PlasticLocalization.GetString(PlasticLocalization.Name.PendingChangesMenuItemCheckout));
            mDeleteMenuItemContent = new GUIContent(
                PlasticLocalization.GetString(PlasticLocalization.Name.PendingChangesMenuItemDelete)
                + " _Del");

            mFilterMenuBuilder.BuildIgnoredSubmenuItem();
            mFilterMenuBuilder.BuildHiddenChangesSubmenuItem();
        }

        static void ProcessMenuOperation(
            PendingChangesMenuOperations operationToExecute,
            IPendingChangesMenuOperations pendingChangesMenuOperations)
        {
            if (operationToExecute == PendingChangesMenuOperations.Open)
            {
                pendingChangesMenuOperations.Open();
                return;
            }

            if (operationToExecute == PendingChangesMenuOperations.DiffWorkspaceContent)
            {
                pendingChangesMenuOperations.Diff();
                return;
            }

            if (operationToExecute == PendingChangesMenuOperations.Delete)
            {
                pendingChangesMenuOperations.Delete();
                return;
            }
        }

        static PendingChangesMenuOperations GetMenuOperation(Event e)
        {
            if (Keyboard.IsShiftPressed(e) && Keyboard.IsKeyPressed(e, KeyCode.O))
                return PendingChangesMenuOperations.Open;

            if (Keyboard.IsShiftPressed(e) && Keyboard.IsKeyPressed(e, KeyCode.D))
                return PendingChangesMenuOperations.DiffWorkspaceContent;

            if (Keyboard.IsKeyPressed(e, KeyCode.Delete))
                return PendingChangesMenuOperations.Delete;

            return PendingChangesMenuOperations.None;
        }

        GUIContent mNoActionMenuItemContent;

        GUIContent mOpenSubmenuItemContent;
        GUIContent mOpenMenuItemContent;
        GUIContent mOpenWithMenuItemContent;
        GUIContent mOpenInExplorerMenuItemContent;
        GUIContent mOpenMetaMenuItemContent;
        GUIContent mOpenMetaWithMenuItemContent;
        GUIContent mOpenMetaInExplorerMenuItemContent;
        GUIContent mDiffMenuItemContent;
        GUIContent mDiffMetaMenuItemContent;
        GUIContent mUndoChangesMenuItemContent;
        GUIContent mCheckoutMenuItemContent;
        GUIContent mDeleteMenuItemContent;

        FilesFilterPatternsMenuBuilder mFilterMenuBuilder;
        IPendingChangesMenuOperations mPendingChangesMenuOperations;
        IMetaMenuOperations mMetaMenuOperations;
    }
}
