﻿using AndroidApp = Android.App;
using Android.Content.PM;
using Android.Content;
using Android.OS;
using Android.App;
using AW =Android.Widget;
using Android.Views;
using MauiApp8.Platforms.Android.AndroidServices;
using XE = Xamarin.Essentials;
using MauiApp8;
using static Android.Manifest;

using AndroidX.Core.App;


using AndroidX;
using Android.Net;
using Android.Runtime;
using Android.Support.V4.App;

using Android.Provider;

namespace MauiApp8;

[AndroidApp.Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]

public class MainActivity : MauiAppCompatActivity
{
    
    public static MainActivity ActivityCurrent { get; set; }

    protected override void OnResume()
    {
        base.OnResume();

        RequestNotificationPermission();
    }
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        Xamarin.Essentials.Platform.Init(this, savedInstanceState);

        RequestDndAccess();
        // Start the foreground service
        
        StartMyForegroundService();
        if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
        {
            var packageName = AndroidApp.Application.Context.PackageName;
            var powerManager = (PowerManager)GetSystemService(PowerService);
            if (!powerManager.IsIgnoringBatteryOptimizations(packageName))
            {
                var intent = new Intent(Android.Provider.Settings.ActionRequestIgnoreBatteryOptimizations, Android.Net.Uri.Parse("package:" + packageName));
                StartActivity(intent);
            }
        }
       


    }
    private void RequestNotificationPermission()
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            if (NotificationManagerCompat.From(this).AreNotificationsEnabled())
            {
                // notifications are enabled
            }
            else
            {
                // request notification permission
                var permissionIntent = new Intent(Settings.ActionApplicationDetailsSettings);
                var uri = Android.Net.Uri.FromParts("package", PackageName, null);
                permissionIntent.SetData(uri);
                StartActivity(permissionIntent);
            }
        }
        else
        {
            // notifications are enabled
        }
    }
    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
    {
        Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }
    

    

    private void RequestDndAccess()
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
        {
            NotificationManager notificationManager = (NotificationManager)GetSystemService(NotificationService);

            if (!notificationManager.IsNotificationPolicyAccessGranted)
            {
                Intent intent = new Intent(Android.Provider.Settings.ActionNotificationPolicyAccessSettings);
                StartActivity(intent);
            }
        }
    }
    private void StartMyForegroundService()
    {
        var myForegroundServiceIntent = new Intent(this, typeof(MauiApp8.Platforms.Android.AndroidServices.MyForegroundService));
        StartService(myForegroundServiceIntent);
    }

}




[AndroidApp.Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
[AndroidApp.IntentFilter(new[] { Intent.ActionView }, Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, DataScheme = "com.companyname.mauiapp8")]

public class WebAuthenticationCallbackActivity : WebAuthenticatorCallbackActivity
{
    const string CALLBACK_SCHEME = "mauiapp8";
}




