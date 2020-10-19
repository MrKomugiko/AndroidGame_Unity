using System.IO;

using Codice.Client.Common;
using Codice.Tool;
using Codice.Utils;

namespace Codice
{
    internal static class UnityConfigurationChecker
    {
        internal static bool NeedsConfiguration()
        {
            string clientBinFolder = PlasticInstallPath.GetClientBinDir();

            if (string.IsNullOrEmpty(clientBinFolder))
                return true;

            SetupEditionFile(clientBinFolder);

            return ConfigurationChecker.NeedConfiguration();
        }

        static void SetupEditionFile(string plasticInstallDir)
        {
            bool isCloudPlasticInstall = EditionToken.IsCloudEditionForPath(plasticInstallDir);
            bool isDvcsPlasticInstall = EditionToken.IsDvcsEditionForPath(plasticInstallDir);

            string unityCloudEditionTokenFile = Path.Combine(
                ApplicationLocation.GetAppPath(),
                EditionToken.CLOUD_EDITION_FILE_NAME);

            string unityDvcsEditionTokenFile = Path.Combine(
                ApplicationLocation.GetAppPath(),
                EditionToken.DVCS_EDITION_FILE_NAME);

            SetupTokenFile(isCloudPlasticInstall, unityCloudEditionTokenFile);
            SetupTokenFile(isDvcsPlasticInstall, unityDvcsEditionTokenFile);
        }

        static void SetupTokenFile(bool isEdition, string editionTokenFile)
        {
            if (isEdition && !File.Exists(editionTokenFile))
            {
                File.Create(editionTokenFile).Dispose();
                return;
            }

            if (!isEdition && File.Exists(editionTokenFile))
            {
                File.Delete(editionTokenFile);

                string metaPath = MetaPath.GetMetaPath(editionTokenFile);

                if (File.Exists(metaPath))
                {
                    File.Delete(metaPath);
                }

                return;
            }
        }
    }
}
