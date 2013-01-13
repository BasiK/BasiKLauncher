using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Reflection;

namespace BasiK.BasiKLauncher
{
    /// <summary>
    /// Application Entry.
    /// Contains data needed for display
    /// and starting of the application.
    /// </summary>
    internal abstract class ApplicationEntry : IComparable
    {
        public string DisplayName { get; protected set; }
        public Image DisplayImage { get; protected set; }

        public ApplicationEntry(string name)
            : this (name, null)
        { }

        public ApplicationEntry(string name, Image displayImage)
        {
            this.DisplayName = name;
            this.DisplayImage = displayImage;
        }

        public abstract void Execute();

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

    internal class ExternalApplication : ApplicationEntry
    {
        public string StartApp { get; protected set; }
        public string StartOptions { get; protected set; }

        public ExternalApplication(string name, string startapp)
            : this(name, startapp, null, null)
        { }
        public ExternalApplication(string name, string startapp, string startoptions)
            : this (name, startapp, startoptions, null)
        { }
        public ExternalApplication(string name, string startapp, string startoptions, string image)
            : base(name)
        {
            this.StartApp = startapp;
            this.StartOptions = startoptions;
            try
            {
                if (!string.IsNullOrEmpty(image))
                    DisplayImage = Image.FromFile(image);
            }
            catch { }
        }

        public override void Execute()
        {
            Process.Start(this.StartApp, this.StartOptions ?? "");
        }
    }

    internal class SteamApplication : ExternalApplication
    {
        // currently the same as external application... but could come in handy some day...
        public SteamApplication(string name, string startapp, string startoptions, string image)
            : base(name, startapp, startoptions, image)
        { }

    }

    internal class ApplicationExitEntry : ApplicationEntry
    {
        public ApplicationExitEntry()
            : base("~ Exit BasiKLauncher")
        {
            this.DisplayImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("BasiK.BasiKLauncher.Resources.exit_icon.png"));
        }

        public override void Execute()
        {
            System.Windows.Forms.Application.Exit();
        }
    }

    internal class WindowsShutdownEntry : ExternalApplication
    {
        public WindowsShutdownEntry()
            : base("~ Windows Shutdown",  "shutdown", "/s /f /t 05")
        {
            this.DisplayImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("BasiK.BasiKLauncher.Resources.shutdown_icon.png"));
        }
    }
}
