
using MauiApp8.Services.Authentication;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using MauiApp8.Services.BackgroundServices.Realm;
using MauiApp8.Services.ThirdPartyHealthService;
using MauiApp8.Model;
using System.Collections.ObjectModel;
using System.Text.Json;
using static MauiApp8.Services.BackgroundServices.Realm.Utils;

namespace MauiApp8.Services.ThirdPartyHealthService
{
    

    public class GoogleFit : IThirdPartyHealthService
    {
        internal readonly Services.Authentication.IAuthenticationService _authService;
        internal readonly Services.BackgroundServices.Realm.ICRUD _crud;
        private static readonly HttpClient httpClient = new HttpClient();

        public CommonHealthData CommonHealthData { get; set; }

        public GoogleFitData Data { get; set; }

        public GoogleFit(IAuthenticationService authService, ICRUD crud)
        {
            _crud = crud;
            _authService = authService;

            Data = new GoogleFitData();
            CommonHealthData = new CommonHealthData
            {
                ActivityDataList = new ObservableCollection<CommonActivityData>(),
                CalorieDataList = new ObservableCollection<CommonCalorieData>(),
                StepDataList = new ObservableCollection<CommonStepData>()
            };
        }


        public async Task UpdateGoogleFitSteps() 
        {
            await _authService.RefreshAuthenticateAsync();
            var utils = new Utils();
            using Realms.Realm realm = utils.RealmCreate();
            DateTimeOffset addStartDate;
            DateTimeOffset addEndDate;
          
            DateTime nowDateTime = DateTime.UtcNow;
            DateTime startDateTime = nowDateTime.AddDays(-1);
         
            await FetchActivityDataAsync(nowDateTime, startDateTime);
            await _crud.DeleteExerciceEntriesAfterDate(realm, startDateTime);
            //await _crud.DeleteAllExercice(realm);
            //_crud.ConsoleAllExercice(realm);
            Console.WriteLine("Update exercice data...");
            TimeZoneInfo norwegianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            
            foreach (var stepData in CommonHealthData.StepDataList)
            {
                
                if (DateTimeOffset.TryParse(stepData.StartTime.ToString(), out addStartDate) && DateTimeOffset.TryParse(stepData.EndTime.ToString(), out addEndDate))
                {
                    DateTime norwayTimeStart = TimeZoneInfo.ConvertTimeFromUtc(addStartDate.DateTime, norwegianTimeZone);
                    DateTimeOffset startDateTimeOffset = new DateTimeOffset(norwayTimeStart);
                    DateTime norwayTimeEnd = TimeZoneInfo.ConvertTimeFromUtc(addEndDate.DateTime, norwegianTimeZone);
                    DateTimeOffset endDateTimeOffset = new DateTimeOffset(norwayTimeEnd);
                    
                    await _crud.AddExerciceEntry(realm, stepData.Steps, startDateTimeOffset, endDateTimeOffset);

                    
                   
                }
                else
                {
                    Console.WriteLine("Invalid DateTime string");
                }
                
            }
           

        }


        private CommonCalorieData MapGoogleFitCalorieDataToCommon(Point point)
        {
            var startTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
               .AddTicks((long.TryParse(point.StartTimeNanos, out long ticks) ? ticks : 0) / 100);
            var endTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddTicks((long.TryParse(point.EndTimeNanos, out ticks) ? ticks : 0) / 100);

            var commonCalorieData = new CommonCalorieData
            {
                StartTime = startTime,
                EndTime = endTime,
                Calories = point.Value.First().FpVal ?? 0
            };

            return commonCalorieData;
        }


        public async Task FetchCalorieDataAsync(DateTime now, DateTime startTime)
        {
            var calorieData = await FetchGoogleFitDataAsync(now, startTime, "com.google.calories.expended");
            CommonHealthData.CalorieDataList.Clear();
            if (calorieData != null && calorieData.Buckets != null)
            {
                foreach (var bucket in calorieData.Buckets)
                {
                    foreach (var dataset in bucket.Dataset)
                    {
                        foreach (var point in dataset.Point)
                        {
                            var commonCalorieData = MapGoogleFitCalorieDataToCommon(point);
                            
                            CommonHealthData.CalorieDataList.Add(commonCalorieData);
                        }
                    }
                }
            }
        }

        private CommonStepData MapGoogleFitStepDataToCommon(Point point)
        {
            var startTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddTicks((long.TryParse(point.StartTimeNanos, out long ticks) ? ticks : 0) / 100);
            var endTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddTicks((long.TryParse(point.EndTimeNanos, out ticks) ? ticks : 0) / 100);

            var commonStepData = new CommonStepData
            {
                StartTime = startTime,
                EndTime = endTime,
                Steps = point.Value.First().IntVal ?? 0
            };

            return commonStepData;
        }





        private CommonActivityData MapGoogleFitActivityDataToCommon(Point point)
        {
            var startTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddTicks((long.TryParse(point.StartTimeNanos, out long ticks) ? ticks : 0) / 100);
            var endTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddTicks((long.TryParse(point.EndTimeNanos, out ticks) ? ticks : 0) / 100);

            var activityType = int.TryParse(point.Value[0].IntVal.ToString(), out var parsedSteps) ? parsedSteps : 0;
            var count = int.TryParse(point.Value[2].IntVal.ToString(), out var parsedActivityId) ? parsedActivityId : 0;
            long durationInSeconds = (long.TryParse(point.Value[1].IntVal.ToString(), out long time) ? time : 0) / 1000;
            TimeSpan duration = new TimeSpan(durationInSeconds * TimeSpan.TicksPerSecond);


            var commonActivityData = new CommonActivityData
            {
                StartTime = startTime,
                EndTime = endTime,
                ActivityType = activityType,
                ActivityDuration = duration,
                Count = count
            };

            return commonActivityData;
        }






        public async Task FetchActivityDataAsync(DateTime now, DateTime startTime)
        {
            var activityData = await FetchGoogleFitDataAsync(now, startTime, "com.google.activity.segment");
            CommonHealthData.ActivityDataList.Clear();
            if (activityData != null && activityData.Buckets != null)
            {
                foreach (var bucket in activityData.Buckets)
                {
                    foreach (var dataset in bucket.Dataset)
                    {
                        foreach (var point in dataset.Point)
                        {
                            var commonActivityData = MapGoogleFitActivityDataToCommon(point);
                            
                            CommonHealthData.ActivityDataList.Add(commonActivityData);
                        }
                    }
                }
            }
        



        var stepData = await FetchGoogleFitDataAsync(now, startTime, "com.google.step_count.delta");
            CommonHealthData.StepDataList.Clear();
            if (stepData != null && stepData.Buckets != null)
            {
                foreach (var bucket in stepData.Buckets)
                {
                    foreach (var dataset in bucket.Dataset)
                    {
                        foreach (var point in dataset.Point)
                        {
                            var commonStepData = MapGoogleFitStepDataToCommon(point);
                            
                            CommonHealthData.StepDataList.Add(commonStepData);
                        }
                    }
                }
            }
        }



        public long DateTimeToUnixTimestampMillis(DateTime dateTime)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan elapsedTime = dateTime.ToUniversalTime() - epoch;
            return (long)elapsedTime.TotalMilliseconds;
        }


        public async Task<string> PostGoogleFitDataAsync(string requestUrl, string jsonPayload, string accessToken)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(requestUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                return responseData;
            }
            else
            {
                Console.WriteLine($"Error in PostGoogleFitDataAsync: {response.StatusCode} - {response.ReasonPhrase}");
            }

            return null;
        }


        private async Task<GoogleFitData> FetchGoogleFitDataAsync(DateTime now, DateTime startTime, string dataTypeName)
        {
            try
            {
                string accessToken = _authService.User.AccessToken.ToString();
                long startTimeMillis = DateTimeToUnixTimestampMillis(startTime);
                long endTimeMillis = DateTimeToUnixTimestampMillis(now);

                var requestUrl = "https://www.googleapis.com/fitness/v1/users/me/dataset:aggregate";


                var requestBody = new
                {
                    aggregateBy = new[]
                  {
                        new
                        {
                    dataTypeName,
                        }
                    },
                    bucketByTime = new { durationMillis = 60000 },
                    startTimeMillis,
                    endTimeMillis
                };

                var jsonPayload = JsonConvert.SerializeObject(requestBody);
                

                var data = await PostGoogleFitDataAsync(requestUrl, jsonPayload, accessToken);

                if (data != null)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    
                    var googleFitData = System.Text.Json.JsonSerializer.Deserialize<GoogleFitData>(data);
                    Data = googleFitData;

                    return Data;
                }
                else
                {
                    Console.WriteLine("Error fetching Google Fit data.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }


    }
}

