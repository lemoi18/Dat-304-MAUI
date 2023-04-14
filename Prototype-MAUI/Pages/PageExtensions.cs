using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp8.Pages
{
    

    public static class PagesExtensions
    {
        public static MauiAppBuilder ConfigurePages(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<Views.HomePage>();
            builder.Services.AddSingleton<Views.LoginPage>();
            builder.Services.AddSingleton<Views.SettingsPage>();
            builder.Services.AddSingleton<Views.FoodPage>();
            builder.Services.AddSingleton<Views.GraphPage>();


            builder.Services.AddTransient<Views.FoodDetailsPage>();
            builder.Services.AddTransient<Model.Food>();


            return builder;
        }
    }
}

