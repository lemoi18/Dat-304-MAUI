using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Apis.Oauth2.v2;
using MauiApp8.Services.BackgroundServices;
using Microsoft.Toolkit.Mvvm.Messaging; 
using CommunityToolkit.Mvvm;
using MauiApp8.Services.ThirdPartyHealthService;

namespace MauiApp8.Services.PublishSubscribeService
{
    public class Publish
    {
        internal readonly IBackgroundService _backgroundService;
        private readonly IThirdPartyHealthService _thirdPartyHealthService;


        public Publish(IBackgroundService backgroundService)
        {
            _backgroundService = backgroundService;

        }
        public class UpdateResponse
        {
            public int Response { get; set; }
        }

        public class Alarm
        {
            public int Response { get; set; }
            public bool On { get; set; } = false;
        }

        public async Task UpdateBackgroundData(string domainName)
        {
            int response = 0;

            DateTime now = DateTime.UtcNow;
            DateTime startTime = now.AddDays(-1);
            try
            {
                response += await UpdateGlucose(domainName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Glucose Update Issue: " + ex);
            }

            try
            {
                response += await UpdateInsulin(domainName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Insulin Update Issue: " + ex);
            }

            try
            {
                response += await FetchActivityDataAsync(now,startTime);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Activity Update Issue: " + ex);
            }

            try
            {
                response += await FetchCalorieDataAsync(now, startTime);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Calorie Update Issue: " + ex);
            }


       

            

            WeakReferenceMessenger.Default.Send(new UpdateResponse { Response = response });
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
            WeakReferenceMessenger.Default.Register<UpdateResponse>(this, (sender, message) =>
            {
                Console.WriteLine(message.Response);
                Console.WriteLine(sender);
                Console.WriteLine("Updated Data...");
            });

        }

    }
}
