// File: MauiProgram.cs
// MAUI application builder for the Basic app template. Configures fonts and debug logging.

using Microsoft.Extensions.Logging;

namespace AppBasic;

/// <summary>
/// Static entry point for the .NET MAUI app. Creates and configures the MauiApp builder.
/// </summary>
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
