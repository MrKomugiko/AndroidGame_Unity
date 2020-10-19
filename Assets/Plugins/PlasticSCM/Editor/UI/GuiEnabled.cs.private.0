using System;
using UnityEngine;

namespace Codice.UI
{
    public class GuiEnabled : IDisposable
    {
        public GuiEnabled(bool enabled)
        {
            mEnabled = GUI.enabled;
            GUI.enabled = enabled && mEnabled;
        }

        public void Dispose()
        {
            GUI.enabled = mEnabled;
        }

        bool mEnabled;
    }
}
