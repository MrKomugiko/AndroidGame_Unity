using System.Collections.Generic;

using UnityEditor;

using Codice.Client.GameUI.Checkin;
using Codice.UI;
using GluonGui.Dialog;
using GluonGui.WorkspaceWindow.Views.Checkin.Operations;
using PlasticGui;

namespace Codice.Views.PendingChanges.Dialogs
{
    public class LaunchCheckinConflictsDialog : CheckinUIOperation.ICheckinConflictsDialog
    {
        public LaunchCheckinConflictsDialog(EditorWindow window)
        {
            mWindow = window;
        }

        Result CheckinUIOperation.ICheckinConflictsDialog.Show(
            IList<CheckinConflict> conflicts,
            PlasticLocalization.Name dialogTitle,
            PlasticLocalization.Name dialogExplanation,
            PlasticLocalization.Name okButtonCaption)
        {
            ResponseType responseType = CheckinConflictsDialog.Show(
                conflicts, dialogTitle, dialogExplanation,
                okButtonCaption, mWindow);

            return responseType == ResponseType.Ok ?
                Result.Ok : Result.Cancel;
        }

        EditorWindow mWindow;
    }
}
