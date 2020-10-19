using Codice.Views.IncomingChanges;
using Codice.Views.PendingChanges;

namespace Codice
{
    internal static class AutoRefresh
    {
        internal static void PendingChangesView(PendingChangesTab pendingChangesTab)
        {
            if (DisableFsWatcher.MustDisableFsWatcher())
                return;

            if (pendingChangesTab == null)
                return;

            pendingChangesTab.AutoRefresh();
        }

        internal static void IncomingChangesView(IIncomingChangesTab incomingChangesTab)
        {
            if (DisableFsWatcher.MustDisableFsWatcher())
                return;

            if (incomingChangesTab == null)
                return;

            if (!incomingChangesTab.IsVisible)
                return;

            incomingChangesTab.AutoRefresh();
        }
    }
}
