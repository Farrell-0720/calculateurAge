using CalculateurAge.ViewModels;
using CalculateurAge.Views;
using Microsoft.Extensions.Logging;

namespace CalculateurAge;

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

        // Un seul ViewModel partagé par les deux pages.
        builder.Services.AddSingleton<CalculateurViewModel>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<ResultatPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}
