using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Win32;
using System.Drawing;

namespace BasiK.BasiKLauncher
{
    /// <summary>
    /// Application Library.
    /// Contains a list of the defined applications as ApplicationEntry objects.
    /// Also provides functions to find applications from steam and add custom applications.
    /// </summary>
    internal class ApplicationLibrary
    {
        /// <summary>
        /// Provide the list with defined application entries.
        /// </summary>
        public List<ApplicationEntry> ApplicationList
        {
            get
            {
                return new List<ApplicationEntry>(_applicationEntries);
            }
        }
        private List<ApplicationEntry> _applicationEntries = new List<ApplicationEntry>();

        /// <summary>
        /// Try to extract steam path and apps information from the registry.
        /// Add found steam apps to the library.
        /// WARNING : needs to be tested on multiple systems.
        /// </summary>
        /// <returns>Null if everything went ok. Returns an error message if not.</returns>
        public string FindSteamGames()
        {
            try
            {
                string archRegKey = @"SYSTEM\CurrentControlSet\Control\Session Manager\Environment";
                RegistryKey rkproc = Registry.LocalMachine.OpenSubKey(archRegKey);
                string procArchitecture = rkproc.GetValue("PROCESSOR_ARCHITECTURE") as string;

                string steamExeRegKey = @"SOFTWARE\Wow6432Node\Valve\Steam";
                string regUninstallKey = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

                string steamExe = @"C:\Program Files (x86)\Steam\Steam.exe";

                if (procArchitecture.ToLower() == "x86")
                {
                    steamExeRegKey = @"SOFTWARE\Valve\Steam";
                    regUninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

                    RegistryKey rsteam = Registry.LocalMachine.OpenSubKey(steamExeRegKey);
                    steamExe = Path.Combine(rsteam.GetValue("InstallPath") as string, "steam.exe");
                }
                else
                {
                    steamExeRegKey = @"SOFTWARE\Wow6432Node\Valve\Steam";
                    regUninstallKey = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

                    RegistryKey rsteam = Registry.LocalMachine.OpenSubKey(steamExeRegKey);
                    steamExe = Path.Combine(rsteam.GetValue("InstallPath") as string, "steam.exe");
                }

                List<string> steamnames = new List<string>();
                RegistryKey rksoft = Registry.LocalMachine.OpenSubKey(regUninstallKey);
                foreach (string name in rksoft.GetSubKeyNames())
                    if (name.StartsWith("Steam App "))
                        steamnames.Add(name);

                foreach (string steamgame in steamnames)
                {
                    RegistryKey gamekey = rksoft.OpenSubKey(steamgame);
                    _applicationEntries.Add(
                        new SteamApplication(
                            gamekey.GetValue("DisplayName") as string, 
                            steamExe, 
                            "-applaunch " + steamgame.Substring(10) + " ", 
                            gamekey.GetValue("DisplayIcon") as string));
                }

                _applicationEntries.Sort();

                return null;
            }
            catch (Exception exc)
            {
                return string.Format("Excepction occured: {0} \r\n{1}", exc.Message, exc.StackTrace);
            }
        }

        /// <summary>
        /// Add a custom application definition to the library.
        /// </summary>
        /// <param name="name">Display name of the application.</param>
        /// <param name="exe">Absolute executable path.</param>
        /// <param name="options">Optional execution options.</param>
        /// <param name="iconFile">Optional absolute icon path.</param>
        /// <returns>Null if everything went ok. Returns an error message if not.</returns>
        public string AddApplication(string name, string exe, string options, string iconFile)
        {
            try
            {
                if (string.IsNullOrEmpty(name.Trim()))
                    throw new Exception("Empty name... can not create application entry.");
                if (string.IsNullOrEmpty(exe.Trim()))
                    throw new Exception("Empty executable name... can not create application entry.");

                _applicationEntries.Add(new ExternalApplication(name, exe, options, iconFile));

                return null;
            }
            catch (Exception exc)
            {
                return string.Format("Excepction occured: {0} \r\n{1}", exc.Message, exc.StackTrace);
            }
        }

        public void AddApplication(ApplicationEntry entry)
        {
            if (entry != null)
                _applicationEntries.Add(entry);
        }

    }

}
