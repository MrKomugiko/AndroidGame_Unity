using System;
using System.Diagnostics;
using System.IO;

using log4net;

using Codice.Utils;

namespace Codice.Tool
{
    internal static class LaunchTool
    {
        internal static void OpenConfigurationForMode(bool isGluonMode)
        {
            mLog.Debug("Opening Configuration'.");

            if (isGluonMode)
            {
                mGluonProcessId = ExecuteProcess(
                    PlasticInstallPath.GetGluonExePath(),
                    ToolConstants.Gluon.GUI_CONFIGURE_ARG);
                return;
            }

            mPlasticProcessId = ExecuteProcess(
                PlasticInstallPath.GetPlasticExePath(),
                ToolConstants.Plastic.GUI_CONFIGURE_ARG);
        }

        internal static void OpenGUIForMode(string wkPath, bool isGluonMode)
        {
            mLog.DebugFormat("Opening GUI on wkPath '{0}'.", wkPath);

            if (isGluonMode)
            {
                mGluonProcessId = ExecuteGUI(
                    PlasticInstallPath.GetGluonExePath(),
                    string.Format(ToolConstants.Gluon.GUI_WK_EXPLORER_ARG, wkPath),
                    ToolConstants.Gluon.GUI_COMMAND_FILE_ARG,
                    ToolConstants.Gluon.GUI_COMMAND_FILE,
                    mGluonProcessId);
                return;
            }

            if (PlatformIdentifier.IsMac())
            {
                mPlasticProcessId = ExecuteGUI(
                    PlasticInstallPath.GetPlasticExePath(),
                    string.Format(ToolConstants.Plastic.GUI_MACOS_WK_EXPLORER_ARG, wkPath),
                    ToolConstants.Plastic.GUI_MACOS_COMMAND_FILE_ARG,
                    ToolConstants.Plastic.GUI_MACOS_COMMAND_FILE,
                    mPlasticProcessId);
                return;
            }

            ExecuteProcess(
                PlasticInstallPath.GetPlasticExePath(),
                string.Format(ToolConstants.Plastic.GUI_WINDOWS_WK_ARG, wkPath));
        }

        internal static void OpenBranchExplorer(string wkPath)
        {
            mLog.DebugFormat("Opening Branch Explorer on wkPath '{0}'.", wkPath);

            if (PlatformIdentifier.IsMac())
            {
                mPlasticProcessId = ExecuteGUI(
                    PlasticInstallPath.GetPlasticExePath(),
                    string.Format(ToolConstants.Plastic.GUI_MACOS_BREX_ARG, wkPath),
                    ToolConstants.Plastic.GUI_MACOS_COMMAND_FILE_ARG,
                    ToolConstants.Plastic.GUI_MACOS_COMMAND_FILE,
                    mPlasticProcessId);
                return;
            }

            ExecuteProcess(
                PlasticInstallPath.GetPlasticExePath(),
                string.Format(ToolConstants.Plastic.GUI_WINDOWS_BREX_ARG, wkPath));
        }

        internal static void OpenWorkspaceConfiguration(string wkPath)
        {
            mLog.DebugFormat("Opening Workspace Configuration on wkPath '{0}'.", wkPath);

            mGluonProcessId = ExecuteGUI(
                PlasticInstallPath.GetGluonExePath(),
                string.Format(ToolConstants.Gluon.GUI_WK_CONFIGURATION_ARG, wkPath),
                ToolConstants.Gluon.GUI_COMMAND_FILE_ARG,
                ToolConstants.Gluon.GUI_COMMAND_FILE,
                mGluonProcessId);
        }

        internal static void OpenMerge(string wkPath)
        {
            mLog.DebugFormat("Opening Merge on wkPath '{0}'.", wkPath);

            if (PlatformIdentifier.IsMac())
            {
                mPlasticProcessId = ExecuteGUI(
                    PlasticInstallPath.GetPlasticExePath(),
                    string.Format(ToolConstants.Plastic.GUI_MACOS_MERGE_ARG, wkPath),
                    ToolConstants.Plastic.GUI_MACOS_COMMAND_FILE_ARG,
                    ToolConstants.Plastic.GUI_MACOS_COMMAND_FILE,
                    mPlasticProcessId);
                return;
            }

            ExecuteProcess(
                PlasticInstallPath.GetPlasticExePath(),
                string.Format(ToolConstants.Plastic.GUI_WINDOWS_MERGE_ARG, wkPath));
        }

        internal static void OpenIncomingChanges(string wkPath, bool isGluonMode)
        {
            mLog.DebugFormat("Opening IncomingChanges on wkPath '{0}'.", wkPath);

            if (isGluonMode)
            {
                mGluonProcessId = ExecuteGUI(
                    PlasticInstallPath.GetGluonExePath(),
                    string.Format(ToolConstants.Gluon.GUI_WK_INCOMING_CHANGES_ARG, wkPath),
                    ToolConstants.Gluon.GUI_COMMAND_FILE_ARG,
                    ToolConstants.Gluon.GUI_COMMAND_FILE,
                    mGluonProcessId);
                return;
            }

            if (PlatformIdentifier.IsMac())
            {
                mPlasticProcessId = ExecuteGUI(
                    PlasticInstallPath.GetPlasticExePath(),
                    string.Format(ToolConstants.Plastic.GUI_MACOS_INCOMING_CHANGES_ARG, wkPath),
                    ToolConstants.Plastic.GUI_MACOS_COMMAND_FILE_ARG,
                    ToolConstants.Plastic.GUI_MACOS_COMMAND_FILE,
                    mPlasticProcessId);
                return;
            }

            ExecuteProcess(
                PlasticInstallPath.GetPlasticExePath(),
                string.Format(ToolConstants.Plastic.GUI_WINDOWS_INCOMING_CHANGES_ARG, wkPath));
        }

        static int ExecuteGUI(
            string program,
            string args,
            string commandFileArg,
            string commandFileName,
            int processId)
        {
            string commandFile = Path.Combine(
                Path.GetTempPath(), commandFileName);

            Process process = GetGUIProcess(program, processId);

            if (process == null)
            {
                mLog.DebugFormat("Executing {0} (new process).", program);

                return ExecuteProcess(
                    program, args + string.Format(commandFileArg, commandFile));
            }

            mLog.DebugFormat("Executing {0} (reuse process pid:{1}).", program, processId);

            using (StreamWriter writer = new StreamWriter(new FileStream(
                commandFile, FileMode.Append, FileAccess.Write, FileShare.Read)))
            {
                writer.WriteLine(args);
            }

            return processId;
        }

        static int ExecuteProcess(string program, string args)
        {
            mLog.DebugFormat("Execute process: '{0} {1}'", program, args);

            Process process = BuildProcess(program, args);

            try
            {
                try
                {
                    process.Start();

                    return process.Id;
                }
                catch (Exception ex)
                {
                    mLog.ErrorFormat("Couldn't execute the program {0}: {1}",
                        program, ex.Message);
                    mLog.DebugFormat("Stack trace: {0}",
                        ex.StackTrace);

                    return -1;
                }
            }
            finally
            {
                if (process != null)
                    process.Close();
            }
        }

        static Process BuildProcess(string program, string args)
        {
            Process result = new Process();
            result.StartInfo.FileName = program;
            result.StartInfo.Arguments = args;
            result.StartInfo.CreateNoWindow = false;
            return result;
        }

        static Process GetGUIProcess(string program, int processId)
        {
            if (processId == -1)
                return null;

            mLog.DebugFormat("Checking {0} process [pid:{1}].", program, processId);

            try
            {
                Process process = Process.GetProcessById(processId);

                if (process == null)
                    return null;

                return process.HasExited ? null : process;
            }
            catch
            {
                // process is not running
                return null;
            }
        }

        static int mPlasticProcessId = -1;
        static int mGluonProcessId = -1;

        static readonly ILog mLog = LogManager.GetLogger("LaunchTool");
    }
}
