using UnityEditor;

namespace Codice
{
    internal static class Refresh
    {
        internal static void UnityAssetDatabase()
        {
            AssetDatabase.Refresh(ImportAssetOptions.Default);
            UnityEditor.VersionControl.Provider.ClearCache();
        }
    }
}