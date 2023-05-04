using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Apis.Oauth2.v2;
using MauiApp8.Services.BackgroundServices;
using CommunityToolkit.Mvvm;
using MauiApp8.Model;
using System.Globalization;
using MauiApp8.Services.Health;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using CommunityToolkit.Mvvm.Messaging;

namespace MauiApp8.Services.PublishSubscribeService
{
    public class Publish
    {
        internal readonly IBackgroundService _backgroundService;
        internal readonly ThirdPartyHealthService.IThirdPartyHealthService _thirdPartyHealthService;
        IHealthService _healthService;

        private Dictionary<DateTimeOffset, List<GlucoseInfo>> _glucoseCache;
        private Dictionary<DateTimeOffset, List<InsulinInfo>> _insulinCache;
        public Publish(IBackgroundService backgroundService, ThirdPartyHealthService.IThirdPartyHealthService thirdPartyHealthService, IHealthService healthService
)
        {
            _backgroundService = backgroundService;
            _thirdPartyHealthService = thirdPartyHealthService;
            _healthService = healthService;
            _glucoseCache = new Dictionary<DateTimeOffset, List<GlucoseInfo>>();
            _insulinCache = new Dictionary<DateTimeOffset, List<InsulinInfo>>();
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

                
                await _thirdPartyHealthService.UpdateGoogleFitSteps();
                //DateTime now = DateTime.UtcNow;
                //DateTime startTime = now.AddDays(-1);

                //await _thirdPartyHealthService.FetchActivityDataAsync(now, startTime);
                //await _thirdPartyHealthService.FetchCalorieDataAsync(now, startTime);

                //foreach (var stepData in _thirdPartyHealthService.CommonHealthData.StepDataList)
                //{
                //    Console.WriteLine($"Steps: {stepData.Steps}, Start Time: {stepData.StartTime}, End Time: {stepData.EndTime}");
                //}

                //foreach (var calorieData in _thirdPartyHealthService.CommonHealthData.CalorieDataList)
                //{
                //    Console.WriteLine($"Calories: {calorieData.Calories}, Start Time: {calorieData.StartTime}, End Time: {calorieData.EndTime}");
                //}

                //foreach (var activityData in _thirdPartyHealthService.CommonHealthData.ActivityDataList)
                //{
                //    Console.WriteLine($"count: {activityData.Count}, Start Time: {activityData.StartTime}, End Time: {activityData.EndTime}, Activity Duration: {activityData.ActivityDuration}, Activity Type: {activityData.ActivityType} ");
                //}


            });

        }

        private DateTimeOffset GetLatestDate<T>(Dictionary<DateTimeOffset, List<T>> cache)
        {
            return cache.Keys.Any() ? cache.Keys.Max() : DateTimeOffset.MinValue;
        }



        public async Task HealthSub()
        {

            WeakReferenceMessenger.Default.Register<Fetch.Update_Health>(this, async (sender, message) =>
            {
                DateTimeOffset now = DateTime.UtcNow;
                DateTimeOffset glucoseStartTime = now.AddDays(-1);
                DateTimeOffset insulinStartTime = now.AddDays(-1);

                DateTimeOffset latestGlucoseDate = GetLatestDate(_glucoseCache);
                DateTimeOffset latestInsulinDate = GetLatestDate(_insulinCache);

                if (latestGlucoseDate > glucoseStartTime)
                {
                    glucoseStartTime = latestGlucoseDate;
                }

                List<GlucoseInfo> glucose;

                // Check if glucose data is in cache and fetch it if not
                if (!_glucoseCache.TryGetValue(glucoseStartTime, out glucose))
                {
                    glucose = await _healthService.ReadGlucoses(glucoseStartTime, now);
                    _glucoseCache[now] = glucose;
                }

                if (latestInsulinDate > insulinStartTime)
                {
                    insulinStartTime = latestInsulinDate;
                }

                List<InsulinInfo> insulin;

                // Check if insulin data is in cache and fetch it if not
                if (!_insulinCache.TryGetValue(insulinStartTime, out insulin))
                {
                    insulin = await _healthService.ReadInsulins(insulinStartTime, now);
                    _insulinCache[now] = insulin;
                }


                
                // Log the sending of GlucoseDataMessage
                WeakReferenceMessenger.Default.Send(new GlucoseDataMessage(glucose));

                // Log the sending of InsulinDataMessage
                WeakReferenceMessenger.Default.Send(new InsulinDataMessage(insulin));
            });

        }

    }

}

