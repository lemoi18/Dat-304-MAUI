using CommunityToolkit.Maui;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Logging;
using MauiApp8.Services.Authentication;
using MauiApp8.Services.DataServices;
using Microsoft.Extensions.DependencyInjection;
using Plugin.LocalNotification;
using SkiaSharp.Views.Maui.Controls.Hosting;
using MauiApp8.ViewModel;
using MauiApp8.Services;
using MauiApp8.Pages;
#if __ANDROID__
using MauiApp8.Platforms.Android.AndroidServices;
#endif

namespace MauiApp8;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseSkiaSharp(true)
            .UseMauiCommunityToolkit()
            .UseLocalNotification()
            .ConfigureViewModels()
            .ConfigureServices()
            .ConfigurePages()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();
        return app;
    }
}
