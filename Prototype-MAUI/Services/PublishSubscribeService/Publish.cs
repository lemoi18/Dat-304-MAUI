using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Apis.Oauth2.v2;
using MauiApp8.Services.BackgroundServices;
using Microsoft.Toolkit.Mvvm.Messaging; 
using CommunityToolkit.Mvvm;
using MauiApp8.Model;
using System.Globalization;




namespace MauiApp8.Services.PublishSubscribeService
{
    public class Publish
    {
        internal readonly IBackgroundService _backgroundService;
        internal readonly Services.Authentication.IAuthenticationService _authService;
        internal Publish(IBackgroundService backgroundService, Services.Authentication.IAuthenticationService authService)
        {
            _backgroundService = backgroundService;
            _authService = authService;
        }
        
       

        public class Alarm
        {
            public int Response { get; set; }
            public bool On { get; set; } = false;
        }

        

        public async Task Turn_On() 
        {
            WeakReferenceMessenger.Default.Send(new Alarm { Response = 0, On = true}); }

        public async Task Turn_Off()
        {
            WeakReferenceMessenger.Default.Send(new Alarm { Response = 0, On = false });
        }
        public async Task CheckSubscribe() 
        {
            WeakReferenceMessenger.Default.Register<Fetch.UpdateResponse>(this, (sender, message) =>
            {
                Console.WriteLine(message.Response);
                Console.WriteLine(sender);
                Console.WriteLine("Updated Data...");
            });

        }
        public async Task CheckTimeDiffrence()
        {
            WeakReferenceMessenger.Default.Register<Alarm>(this, async (sender, message) =>
            {
                List < BasalData.NightscoutProfile > Items;
                Items = await Nightscout.GetInsulinBasal("https://oskarnightscoutweb1.azurewebsites.net/");
                Console.WriteLine(Items.Count);
                DateTimeOffset date = DateTimeOffset.ParseExact("2023-04-13T08:34:56.626Z", "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture);
                Task<double?> a = _backgroundService.GetBasalInsulin("https://oskarnightscoutweb1.azurewebsites.net/", date);
                double? result = await a;
                Console.WriteLine(result);
            });

        }
        public async Task GoogleFetchSub()
        {
            WeakReferenceMessenger.Default.Register<Fetch.Update_Google>(this, async (sender, message) =>
            {
             
                Console.WriteLine(" GoogleFetchSub");
                
            });

        }

    }
}
