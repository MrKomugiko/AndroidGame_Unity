namespace Codice.Tool
{
    internal static class ToolConstants
    {
        internal static class Plastic
        {

            internal const string GUI_CONFIGURE_ARG = "--configure";

            internal const string GUI_WINDOWS_WK_ARG = "--wk=\"{0}\"";
            internal const string GUI_WINDOWS_BREX_ARG = "--branchexplorer=\"{0}\"";
            internal const string GUI_WINDOWS_MERGE_ARG = "--resolve=\"{0}\"";
            internal const string GUI_WINDOWS_INCOMING_CHANGES_ARG = "--resolve=\"{0}\" --incomingmerge";

            internal const string GUI_MACOS_WK_EXPLORER_ARG = "--wk=\"{0}\" --view=ItemsView";
            internal const string GUI_MACOS_BREX_ARG = "--wk=\"{0}\" --view=BranchExplorerView";
            internal const string GUI_MACOS_MERGE_ARG = "--wk=\"{0}\" --view=MergeView";
            internal const string GUI_MACOS_INCOMING_CHANGES_ARG = "--wk=\"{0}\" --view=IncomingChangesView";
            internal const string GUI_MACOS_COMMAND_FILE_ARG = " --command-file=\"{0}\"";
            internal const string GUI_MACOS_COMMAND_FILE = "macplastic-command-file.txt";
        }

        internal static class Gluon
        {

            internal const string GUI_CONFIGURE_ARG = "--configure";
            internal const string GUI_WK_EXPLORER_ARG = "--wk=\"{0}\" --view=WorkspaceExplorerView";
            internal const string GUI_WK_CONFIGURATION_ARG = "--wk=\"{0}\" --view=WorkspaceConfigurationView";
            internal const string GUI_WK_INCOMING_CHANGES_ARG = "--wk=\"{0}\" --view=IncomingChangesView";
            internal const string GUI_COMMAND_FILE_ARG = " --command-file=\"{0}\"";
            internal const string GUI_COMMAND_FILE = "gluon-command-file.txt";
        }

        internal const string MACOS_BINDIR = "/Applications/PlasticSCM.app/Contents/MonoBundle";
    }
}
