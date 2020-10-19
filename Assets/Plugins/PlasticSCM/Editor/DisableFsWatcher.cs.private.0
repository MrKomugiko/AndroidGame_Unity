using Codice.Utils;
using UnityEditor;

namespace Codice
{
    internal static class DisableFsWatcher
    {
        internal static bool MustDisableFsWatcher()
        {
            return PlatformIdentifier.IsWindows() &&
                   IsRunningUnder35RuntimeOrOlder();
        }

        static bool IsRunningUnder35RuntimeOrOlder()
        {
#if !UNITY_2019_2_OR_NEWER
            return PlayerSettings.scriptingRuntimeVersion ==
                ScriptingRuntimeVersion.Legacy;
#else
            return false;
#endif
        }
    }
}
