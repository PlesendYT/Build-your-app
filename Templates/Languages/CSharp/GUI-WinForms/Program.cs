namespace GUIWinForms;

/// <summary>Application entry point for the WinForms app.</summary>
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
