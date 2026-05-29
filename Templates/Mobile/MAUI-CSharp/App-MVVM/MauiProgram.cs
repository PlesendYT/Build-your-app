// File: MauiProgram.cs
// MAUI application builder for the MVVM template. Registers services (MainViewModel, MainPage) for DI.

using AppMVVM.ViewModels;
using Microsoft.Extensions.Logging;

namespace AppMVVM;

/// <summary>
/// Configures the MAUI app with fonts, DI service registration, and debug logging.
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

        // Register ViewModel and Page for dependency injection
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<MainPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
