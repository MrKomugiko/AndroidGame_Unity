using System;

using UnityEngine;

namespace Codice.UI
{
    public class EditorVersion
    {
        public int Year;
        public int Release;
        public int Update;

        EditorVersion(int year, int release, int update)
        {
            Year = year;
            Release = release;
            Update = update;
        }


        public override string ToString()
        {
            return String.Format("{0}.{1}.{2}", Year, Release, Update);
        }

        public static bool IsCurrentEditorOlderThan(string version)
        {
            return IsEditorOlderThan(Application.unityVersion, version);
        }

        public static bool IsEditorOlderThan(string versionA, string versionB)
        {
#if UNITY_2017_1_OR_NEWER
            var editorA = Parse(versionA);
            var editorB = Parse(versionB);
            if (editorA.Year == editorB.Year)
            {
                if (editorA.Release == editorB.Release)
                {
                    return editorA.Update < editorB.Update;
                }
                return editorA.Release < editorB.Release;
            }
            return editorA.Year < editorB.Year;
#else
            return false;
#endif
        }

        static int ParseUpdateString(string version)
        {
            int pos = 0;
            char[] characters = version.ToCharArray();
            while (Char.IsDigit(characters[pos]))
            {
                ++pos;
            }
            return int.Parse(version.Substring(0, pos));
        }

        static EditorVersion Parse(string version)
        {
            var versions = version.Split('.');

            var year = 0;
            year = int.Parse(versions[0]);
            var release = 0;
            release = int.Parse(versions[1]);
            var update = 0;
            update = ParseUpdateString(versions[2]);

            return new EditorVersion(year, release, update);
        }
    }
}
