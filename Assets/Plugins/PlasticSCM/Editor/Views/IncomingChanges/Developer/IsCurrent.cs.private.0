using Codice.Client.BaseCommands.Merge;
using PlasticGui.WorkspaceWindow.IncomingChanges;

namespace Codice.Views.IncomingChanges.Developer
{
    // public for testing purpuses
    public static class IsCurrent
    {
        public static bool Conflict(
            IncomingChangeInfo changeInfo,
            IncomingChangeInfo metaChangeInfo,
            MergeSolvedFileConflicts solvedFileConflicts)
        {
            if (solvedFileConflicts == null)
                return false;

            MergeSolvedFileConflicts.CurrentConflict currentConflict;

            if (!solvedFileConflicts.TryGetCurrentConflict(out currentConflict))
                return false;

            return IsSameConflict(currentConflict, changeInfo) ||
                   IsSameConflict(currentConflict, metaChangeInfo);
        }

        static bool IsSameConflict(
            MergeSolvedFileConflicts.CurrentConflict currentConflict,
            IncomingChangeInfo changeInfo)
        {
            if (changeInfo == null)
                return false;

            return currentConflict.MountId.Equals(changeInfo.GetMount().Id) &&
                   currentConflict.ItemId == changeInfo.GetRevision().ItemId;
        }
    }
}
