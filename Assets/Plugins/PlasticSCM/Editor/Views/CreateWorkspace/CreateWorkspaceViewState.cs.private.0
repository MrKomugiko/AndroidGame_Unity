using System.IO;

using Codice.CM.Common;
using Codice.UI.Progress;
using PlasticGui;
using UnityEngine;

namespace Codice.Views.CreateWorkspace
{
    internal class CreateWorkspaceViewState
    {
        internal enum WorkspaceModes
        {
            Developer,
            Gluon
        }

        internal string RepositoryName { get; set; }
        internal string WorkspaceName { get; set; }
        internal string WorkspacePath { get; set; }
        internal WorkspaceModes WorkspaceMode { get; set; }
        internal ProgressControlsForViews.Data ProgressData { get; set; }

        internal static CreateWorkspaceViewState BuildForProjectDefaults()
        {
            string projectName = Application.productName;
            string projectPath = Path.GetFullPath(
                Path.GetDirectoryName(Application.dataPath));

            return new CreateWorkspaceViewState()
            {
                RepositoryName = projectName,
                WorkspaceName = projectName,
                WorkspacePath = projectPath,
                WorkspaceMode = WorkspaceModes.Developer,
                ProgressData = new ProgressControlsForViews.Data()
            };
        }
    }
}
