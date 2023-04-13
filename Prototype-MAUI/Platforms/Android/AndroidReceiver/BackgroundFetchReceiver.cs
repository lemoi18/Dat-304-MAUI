using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using PS = MauiApp8.Services.PublishSubscribeService;
using MauiApp8.Services.BackgroundServices;
using IBF = MauiApp8.Services.BackgroundServices;


namespace MauiApp8.Platforms.Android.AndroidReceiver
{
    [BroadcastReceiver(Enabled = true)]
    public class BackgroundFetchReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            throw new NotImplementedException();
        }
    }
}
