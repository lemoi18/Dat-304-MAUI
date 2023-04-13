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

        private async Task<int> UpdateGlucose(string domainName)
        {
            try
            {
                await _backgroundService.UpdateGlucose(domainName);
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("_backgroundService.UpdateGlucose Issue: " + ex);
                return 0;
            }
        }

        private async Task<int> UpdateInsulin(string domainName)
        {
            try
            {
                await _backgroundService.UpdateInsulin(domainName);
                return 2;
            }
            catch (Exception ex)
            {
                Console.WriteLine("_backgroundService.UpdateInsulin Issue: " + ex);
                return 0;
            }
        }

        private async Task<int> FetchCalorieDataAsync(DateTime now, DateTime startTime)
        {
            try
            {
                await _thirdPartyHealthService.FetchCalorieDataAsync(now, startTime);
                // ...
                return 3;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching calorie data: {ex.Message}");
                return 0;
            }
        }

        private async Task<int> FetchActivityDataAsync(DateTime now, DateTime startTime)
        {
            try
            {
                await _thirdPartyHealthService.FetchActivityDataAsync(now, startTime);
                // ...
                return 4;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching activity data: {ex.Message}");
                return 0;
            }
        }




        public async Task CheckSubscribe() 
        {
            WeakReferenceMessenger.Default.Register<UpdateResponse>(this, (sender, message) =>
            {
                Console.WriteLine(message.Response);
                Console.WriteLine(sender);
                Console.WriteLine("Gone Inn...");
            });

        }

    }
}
