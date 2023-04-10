﻿using Android.App;
using Android.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.Content;

using System.Threading.Tasks;
using AndroidX.Core.App; // Add this for NotificationCompat

using Android.Support.V4.App; // Add this for NotificationCompat
using System;
using System.Threading.Tasks;
using MauiApp8.Services.BackgroundServices;



namespace MauiApp8.Platforms.Android.AndroidServices
{
    [Service]
    public class MyForegroundService : Service
    {
        private const int ServiceId = 1001;
        private const string CHANNEL_ID = "my_foreground_service_channel";

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            // Create the notification channel
            CreateNotificationChannel();

            // Create a notification using NotificationCompat.Builder
            var notification = new NotificationCompat.Builder(this, CHANNEL_ID)
                .SetContentTitle("My Foreground Service")
                .SetContentText("Fetching data...")
                .SetSmallIcon(Resource.Drawable.notification_icon_background)
                .Build();

            // Start the foreground service with the notification
            StartForeground(ServiceId, notification);

            // Execute the background fetch logic every 15 minutes (900000 milliseconds)
            Task.Run(async () =>
            {
                //while (true)
                //{
                //    DataBase db = new DataBase();
                //    await db.UpdateGlucose("https://oskarnightscoutweb1.azurewebsites.net/");
                //    await db.UpdateInsulin("https://oskarnightscoutweb1.azurewebsites.net/");
                //    Console.WriteLine("Testing Background fetch...");
                //    // Your background fetch logic here
                //    await Task.Delay(60000);
                //}
            });

            return StartCommandResult.Sticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelName = "My Foreground Service Channel";
                var channelDescription = "Notification channel for my foreground service";
                var channel = new NotificationChannel(CHANNEL_ID, channelName, NotificationImportance.Default)
                {
                    Description = channelDescription
                };

                var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }
        }
    }
}
