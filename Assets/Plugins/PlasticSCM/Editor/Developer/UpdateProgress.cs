using Codice.Client.BaseCommands;
using Codice.Client.Commands;
using Codice.Client.Common;
using PlasticGui;

namespace Codice.Developer
{
    internal class UpdateProgress
    {
        internal UpdateProgress(
            UpdateNotifier notifier, string wkPath, string title,
            PlasticGUIClient guiClient)
        {
            mNotifier = notifier;
            mWkPath = wkPath;
            mGuiClient = guiClient;
            mGuiClient.Progress.ProgressHeader = title;
            mGuiClient.Progress.CanCancelProgress = false;
        }

        internal void OnUpdateProgress()
        {
            var progress = mGuiClient.Progress;

            progress.ProgressHeader = FixNotificationPath(
                mWkPath, mNotifier.GetNotificationMessage());

            UpdateOperationStatus status = mNotifier.GetUpdateStatus();

            string totalSize = SizeConverter.ConvertToSizeString(status.TotalSize);
            string updatedSize = SizeConverter.ConvertToSizeString(status.UpdatedSize);

            progress.TotalProgressMessage = PlasticLocalization.GetString(
                status.IsCalculating ?
                    PlasticLocalization.Name.UpdateProgressCalculating :
                    PlasticLocalization.Name.UpdateProgress,
                updatedSize, totalSize, status.UpdatedFiles, status.TotalFiles);

            progress.TotalProgressPercent = CalculateProgress(status.UpdatedSize, status.TotalSize);
        }

        string FixNotificationPath(string wkPath, string notification)
        {
            if (notification == null)
                return string.Empty;

            int position = notification.IndexOf(wkPath);

            if (position < 0)
                return notification;

            return notification.Remove(position, wkPath.Length);
        }

        static double CalculateProgress(long upload, long totalSize)
        {
            if (totalSize == 0)
                return 0.01;

            double progress = (double)upload / totalSize;

            if (progress < 0.01)
                progress = 0.01;

            return progress;
        }

        UpdateNotifier mNotifier;
        string mWkPath;

        PlasticGUIClient mGuiClient;
    }
}
