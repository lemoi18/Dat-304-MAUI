using AndroidApp = Android.App;
using Android.Content;
using Android.OS;
using MauiApp8.Platforms.Android.AndroidReceiver;
using MauiApp8.Services;
using System;
using MauiControls = Microsoft.Maui.Controls;


[assembly: Dependency(typeof(MauiApp8.Platforms.Android.AndroidServices.BackgroundFetchServiceAndroid))]
namespace MauiApp8.Platforms.Android.AndroidServices
{
    public class BackgroundFetchServiceAndroid : MauiApp8.Services.BackgroundFetchService.IBackgroundFetchService
    {
        public void ScheduleFetchTask(TimeSpan interval, Action fetchAction)
        {
            // Get the AlarmManager instance
            AndroidApp.AlarmManager alarmManager = (AndroidApp.AlarmManager)AndroidApp.Application.Context.GetSystemService(Context.AlarmService);
            Console.WriteLine("Got Alarm Manager Instance");

            // Create an Intent for the BackgroundFetchReceiver
            Intent alarmIntent = new Intent(AndroidApp.Application.Context, typeof(BackgroundFetchReceiver));
            alarmIntent.SetAction("com.companyname.mauiapp8.BackgroundFetchReceiver");

            // Create a PendingIntent to be fired by the AlarmManager
            AndroidApp.PendingIntent pendingIntent = AndroidApp.PendingIntent.GetBroadcast(AndroidApp.Application.Context, 0, alarmIntent, AndroidApp.PendingIntentFlags.UpdateCurrent | AndroidApp.PendingIntentFlags.Immutable);
            Console.WriteLine("Created PendingIntent");
            // Schedule the repeating task
            alarmManager.SetRepeating(AndroidApp.AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + 60000, 60000, pendingIntent);
            Console.WriteLine("Scheduled Repeating Task");
        }
    }
}