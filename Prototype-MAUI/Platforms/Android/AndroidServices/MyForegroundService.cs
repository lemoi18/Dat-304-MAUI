using Android.App;
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
using BGF =  MauiApp8.Platforms.Android.AndroidServices;
using Android.Media;
using MauiApp8.Platforms.Android.AndroidReceiver;
using CommunityToolkit.Mvvm.Messaging;

namespace MauiApp8.Platforms.Android.AndroidServices
{
    [Service]
    public class MyForegroundService : Service
    {
        private const int ServiceId = 1001;
        private const string CHANNEL_ID = "my_foreground_service_channel";

        private NotificationHelper _notificationHelper;

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {

            _notificationHelper = new NotificationHelper(this);
            

            // Create the notification channel
            CreateNotificationChannel();

           
            
            // Execute the background fetch logic every 15 minutes (900000 milliseconds)
            Task.Run(async () =>
            {
                await CheckAlarm();
                await AuthAlarm();
                
            });

            return StartCommandResult.Sticky;
        }
        public async Task CheckAlarm()
        {
            WeakReferenceMessenger.Default.Register<MauiApp8.Model.Alarms.Alarm>(this, async (sender, message) =>
            {
                if(message.On == true) { ;

                    var notificationHelper = new NotificationHelper(this);

                    //notificationHelper.CreateNotification("My Notification", "This is my notification message!");
                    notificationHelper.ShowNotification(message.Response, message.Message + " : " + message.Response + " BG ");
                }
                
            });
        }
        public async Task AuthAlarm()
        {
            WeakReferenceMessenger.Default.Register<MauiApp8.Model.Alarms.Authenticate>(this, async (sender, message) =>
            {
                if (message.isAuth == true)
                {
                    Action fetchAction = () =>
                    {
                        // Your background fetch logic here
                        // Logic in Background reciver in android directory
                    };
                    CreateNotificationChannel();

                    // Create a notification using NotificationCompat.Builder
                    var notification = new NotificationCompat.Builder(this, CHANNEL_ID)
                        .SetContentTitle("Starting background service...")
                        .SetContentText("Fetching data...")
                        .SetSmallIcon(Resource.Drawable.notification_icon_background)
                        .Build();

                    // Start the foreground service with the notification
                    StartForeground(ServiceId, notification);
                    BGF.BackgroundFetchServiceAndroid bgf = new BGF.BackgroundFetchServiceAndroid();
                    bgf.ScheduleFetchTask(TimeSpan.FromMinutes(1), fetchAction); // calling
                    DateTime currentTime = DateTime.Now;
                    Console.WriteLine(currentTime);
                    Console.WriteLine("STARTED TIME!");
                    BackgroundFetchReceiver backgroundFetchReceiver = new BackgroundFetchReceiver();
                    await backgroundFetchReceiver.UpdateBackgroundData("https://oskarnightscoutweb1.azurewebsites.net/");
                }

            });
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
                var channel = new NotificationChannel(CHANNEL_ID, channelName, NotificationImportance.High)
                {
                    Description = channelDescription
                };


                var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }
        }
        public void SendNotificationForVariable()
        {
            //_notificationHelper.SendNotificationForVariable("Sample Notification", "Hello World! This is my first notification!");
        }
       

        public void SendAppOffNotification()
        {
            var notification = new NotificationCompat.Builder(this, CHANNEL_ID)
                .SetContentTitle("Your App")
                .SetContentText("The app is off.")
                .SetSmallIcon(Resource.Drawable.notification_icon_background)
                .Build();

            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(2, notification); // Use a different notification ID than the one used for the foreground service notification
        }
        private bool IsAppOff()
        {
            var activityManager = (ActivityManager)GetSystemService(ActivityService);
            var runningTasks = activityManager.GetRunningTasks(1);
            var topActivity = runningTasks[0].TopActivity;

            // Check if the app's package name is equal to the top activity's package name
            return !topActivity.PackageName.Equals(PackageName);
        }
    }
}

