using CommunityToolkit.Maui;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Logging;
using MauiApp8.Services.Authentication;
using MauiApp8.Services.DataServices;
using Microsoft.Extensions.DependencyInjection;

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
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // ViewModels
        builder.Services.AddTransient<ViewModel.HomePageModel>();
        builder.Services.AddSingleton<ViewModel.SettingsPageModel>();
        builder.Services.AddSingleton<ViewModel.LoginPageModel>();
        builder.Services.AddSingleton<ViewModel.LogFoodModel>();
        builder.Services.AddSingleton<ViewModel.FoodDetailsModel>();
        builder.Services.AddSingleton<ViewModel.FoodViewModel>();


        //Views
        builder.Services.AddTransient<Views.HomePage>();
        builder.Services.AddSingleton<Views.LoginPage>();
        builder.Services.AddSingleton<Views.SettingsPage>();
        builder.Services.AddSingleton<Views.FoodPage>();
        builder.Services.AddSingleton<Views.FoodDetailsPage>();
        builder.Services.AddTransient<Views.FoodItemView>();

        builder.Services.AddTransient<Model.Food>();


        //Services

        builder.Services.AddSingleton<Services.Authentication.IAuthenticationService>((e) => new Services.Authentication.RefactoredGoogleAuth());
        builder.Services.AddTransient<Services.DataServices.IDataService>((e) => new Services.DataServices.FoodService_stub());
        builder.Services.AddTransient<Realms.Realm>(e => Services.DBService.CreateDB.RealmCreate());
        builder.Services.AddTransient<Services.BackgroundServices.IBackgroundService>((e) => new Services.BackgroundServices.DataBase());
        builder.Services.AddSingleton<Services.ThirdPartyHealthService.IThirdPartyHealthService>(e => new Services.ThirdPartyHealthService.GoogleFit(e.GetRequiredService<IAuthenticationService>()));
        builder.Services.AddSingleton<Services.PublishSubscribeService.Publish>(e => new Services.PublishSubscribeService.Publish(e.GetRequiredService<Services.BackgroundServices.IBackgroundService>()));
#if __ANDROID__
        builder.Services.AddSingleton<Services.BackgroundFetchService.IBackgroundFetchService, MauiApp8.Platforms.Android.AndroidServices.BackgroundFetchServiceAndroid>(services => new MauiApp8.Platforms.Android.AndroidServices.BackgroundFetchServiceAndroid());
#endif


        var app = builder.Build();
        return app;
    }

    




}


