using LiveChartsCore;
using MauiApp8.Model;

namespace MauiApp8.ViewModel
{
    public static class ViewModelExtensions
    {
        public static MauiAppBuilder ConfigureViewModels(this MauiAppBuilder builder)
        {

            // ViewModels
            builder.Services.AddTransient<ViewModel.HomePageModel>();
            builder.Services.AddSingleton<ViewModel.SettingsPageModel>();
            builder.Services.AddSingleton<ViewModel.LoginPageModel>();
            builder.Services.AddSingleton<ViewModel.LogFoodModel>();
            builder.Services.AddSingleton<ViewModel.FoodDetailsModel>();
            builder.Services.AddSingleton<ViewModel.FoodViewModel>();
            builder.Services.AddSingleton<ViewModel.GraphPageModel>();



            return builder;
        }
    }
}