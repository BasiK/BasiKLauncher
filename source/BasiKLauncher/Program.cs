using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

namespace BasiK.BasiKLauncher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ApplicationLibrary dict = new ApplicationLibrary();
            string steamgames = dict.FindSteamGames();
            if (!string.IsNullOrEmpty(steamgames))
                MessageBox.Show(steamgames);


            // Try to get custom application definitions from the configuration xml
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load("Configuration.xml");

                XmlElement xapps = xdoc["BasiK.lwbp"]["applications"];
                foreach (XmlElement xapp in xapps.GetElementsByTagName("application"))
                {
                    try
                    {
                        dict.AddApplication(
                            xapp.GetAttribute("name"),
                            xapp.GetAttribute("exe"),
                            xapp.HasAttribute("options") ? xapp.GetAttribute("options") : null,
                            xapp.HasAttribute("icon") ? xapp.GetAttribute("icon") : null);
                    }
                    catch { }
                }
            }
            catch
            {
            }

            dict.AddApplication("~ Windows Shutdown", "shutdown", "/s /f /t 05", "");


            Application.Run(new LauncherForm(dict));
        }
    }
}
