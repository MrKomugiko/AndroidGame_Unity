using UnityEditor;

using Codice.UI;

namespace Codice
{
    class PlasticMenuItem
    {
        [MenuItem(MENU_ITEM_NAME)]
        public static void ShowPanel()
        {
            EditorWindow dockWindow = FindEditorWindow.ToDock<PlasticWindow>();

            if (dockWindow == null)
            {
                // create
                EditorWindow.GetWindow<PlasticWindow>(
                    UnityConstants.WINDOW_TITLE).Focus();
                return;
            }

            // reuse
            EditorWindow.GetWindow<PlasticWindow>(
                UnityConstants.WINDOW_TITLE, dockWindow.GetType()).Focus();
        }

        const string MENU_ITEM_NAME = "Window/" + UnityConstants.WINDOW_TITLE;
    }
}
