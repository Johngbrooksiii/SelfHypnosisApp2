using HypnosisApp.Core.Services;
using HypnosisApp.UI.Converters;
using HypnosisApp.UI.Data;
using HypnosisApp.UI.ViewModels;
using HypnosisApp.UI.Views;
using Microsoft.Extensions.Logging;

#if ANDROID
using HypnosisApp.UI.Platforms.Android.Services;
#endif

namespace HypnosisApp.UI;

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

        // Register Core Services
        builder.Services.AddSingleton<IFrequencyEngine, FrequencyEngine>();
        builder.Services.AddSingleton<ISessionPlayer, SessionPlayer>();

        // Register Platform Services
#if ANDROID
        builder.Services.AddSingleton<INarrationEngine, AndroidNarrationEngine>();
        builder.Services.AddSingleton<IAudioPlaybackService, AndroidAudioService>();
#endif

        // Register Data Services
        builder.Services.AddSingleton<ISessionRepository, SessionRepository>();

        // Register ViewModels
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<SessionPlayerViewModel>();

        // Register Views
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<SessionPlayerPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
