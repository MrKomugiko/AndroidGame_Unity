namespace Codice.AssetsProcessor
{
    class AssetModificationProcessor : UnityEditor.AssetModificationProcessor
    {
        internal static bool IsEnabled { get; set; }

        static string[] OnWillSaveAssets(string[] paths)
        {
            if (!IsEnabled)
                return paths;

            PlasticAssetsProcessor.CheckoutOnSourceControl(paths);
            return paths;
        }
    }
}
