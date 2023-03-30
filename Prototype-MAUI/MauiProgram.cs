using CommunityToolkit.Maui;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Logging;
using MauiApp8.Services.Authentication;
using MauiApp8.Services.DataServices;
using CreateDBLib;


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

        builder.Services.AddSingleton<Services.Authentication.IAuthenticationService>((e)=> new Services.Authentication.Authenticated_stub());
        builder.Services.AddTransient<Services.DataServices.IDataService>((e) => new Services.DataServices.FoodService_stub());
        builder.Services.AddSingleton<CreateDBLib.CreateDB>();


        var app = builder.Build();
        return app;
    }
}
