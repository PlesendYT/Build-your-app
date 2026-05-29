using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DesktopTool;

/// <summary>Application entry point for the Desktop Tool.</summary>
static class Program
{
    /// <summary>Main entry point. Initialises the application and shows the main form.</summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}
