using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BasiK.BasiKLauncher
{
    /// <summary>
    /// Static class with the actual functionality
    /// to start applications.
    /// </summary>
    internal static class ApplicationStarter
    {
        public static void StartApplication(ApplicationEntry application)
        {
            Process.Start(application.StartApp, application.StartOptions ?? "");
        }
    }
}
