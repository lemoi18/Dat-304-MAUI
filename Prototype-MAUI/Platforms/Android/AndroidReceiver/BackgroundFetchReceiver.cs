using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using MauiApp8.Services.BackgroundServices;

namespace MauiApp8.Platforms.Android.AndroidReceiver
{
    [BroadcastReceiver(Enabled = true)]
    public class BackgroundFetchReceiver : BroadcastReceiver
    {
        
        public override async void OnReceive(Context context, Intent intent)
        {
            // Implement your background fetch logic here
            DataBase db = new DataBase();
            await db.UpdateGlucose("https://oskarnightscoutweb1.azurewebsites.net/");
            await db.UpdateInsulin("https://oskarnightscoutweb1.azurewebsites.net/");
            Console.WriteLine("Testing Background fetch...");
        }
    }
}
