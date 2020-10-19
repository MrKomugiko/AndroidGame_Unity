using System;

using UnityEngine;

namespace Codice
{
    internal static class GetRelativePath
    {
        internal static string ToApplication(string path)
        {
            Uri relativeToUri = new Uri(Application.dataPath);
            Uri pathUri = new Uri(FixVolumeLetterPath(path));

            return relativeToUri.MakeRelativeUri(pathUri).ToString();
        }

        static string FixVolumeLetterPath(string path)
        {
            string volumeLetter = new string(new char[] { path[0] });
            volumeLetter = volumeLetter.ToUpperInvariant();

            return string.Concat(volumeLetter, path.Substring(1));
        }
    }
}
