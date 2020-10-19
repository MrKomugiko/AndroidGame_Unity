using PlasticGui;

namespace Codice.AssetsProcessor
{
    internal static class AssetsProcessors
    {
        internal static void Enable(IPlasticAPI plasticApi)
        {
            PlasticAssetsProcessor.RegisterPlasticAPI(plasticApi);

            AssetPostprocessor.IsEnabled = true;
            AssetModificationProcessor.IsEnabled = true;
        }

        internal static void Disable()
        {
            AssetPostprocessor.IsEnabled = false;
            AssetModificationProcessor.IsEnabled = false;
        }
    }
}
