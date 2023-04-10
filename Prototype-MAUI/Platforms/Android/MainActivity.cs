using AndroidApp = Android.App;
using Android.Content.PM;
using Android.Content;
using Android.OS;
namespace MauiApp8;

[AndroidApp.Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]

public class MainActivity : MauiAppCompatActivity
{
    public static MainActivity ActivityCurrent { get; set; }
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Start the foreground service
        var intentForGround = new Intent(this, typeof(MauiApp8.Platforms.Android.AndroidServices.MyForegroundService));
        StartService(intentForGround);

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

}




[AndroidApp.Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
[AndroidApp.IntentFilter(new[] { Intent.ActionView }, Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, DataScheme = "com.companyname.mauiapp8")]

public class WebAuthenticationCallbackActivity : WebAuthenticatorCallbackActivity
{
    const string CALLBACK_SCHEME = "mauiapp8";
}

