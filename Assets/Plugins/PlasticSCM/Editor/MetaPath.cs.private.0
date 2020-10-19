using Codice.UI;

namespace Codice
{
    internal static class MetaPath
    {
        public const string META_EXTENSION = ".meta";

        internal static bool IsMetaPath(string path)
        {
            return path.EndsWith(META_EXTENSION);
        }

        internal static string GetMetaPath(string path)
        {
            return string.Concat(
                path,
                META_EXTENSION);
        }

        internal static string GetPathFromMetaPath(string path)
        {
            return path.Substring(
                0,
                path.Length - META_EXTENSION.Length);
        }
    }
}
