using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasiK.BasiKLauncher
{
    /// <summary>
    /// Application Entry.
    /// Contains data needed for display
    /// and starting of the application.
    /// </summary>
    internal class ApplicationEntry : IComparable
    {
        public string SteamAppID = null;
        public string DisplayIcon;
        public string DisplayName;
        public string StartApp;
        public string StartOptions;

        public int CompareTo(object obj)
        {
            ApplicationEntry ge = obj as ApplicationEntry;
            if (ge == null)
                return 1;

            if (ge.DisplayName == null)
                return 1;

            if (DisplayName == null)
                return -1;

            return DisplayName.CompareTo(ge.DisplayName);
        }
    }
}
