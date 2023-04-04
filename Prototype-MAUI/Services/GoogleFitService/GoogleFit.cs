
using MauiApp8.Services.Authentication;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Fitness.v1;
using Google.Apis.Fitness.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using System.Text.Json;

using System.Security.Authentication;

namespace MauiApp8.Services.GoogleFitService
{
    /// <summary>
    /// await _googlefit.FetchGoogleFitDataAsync(long.Parse(startTime), long.Parse(currentTime));
    /// This Code is not working, ill look at it later...
    /// </summary>
    
    public class GoogleFit
    {
        internal readonly Services.Authentication.IAuthenticationService _authService;
        private static readonly HttpClient httpClient = new HttpClient();
        public string auth_uri { get; set; }
        public string scheme { get; set; }

        public string token_uri { get; set; }
        public string client_id { get; set; }
        public string auth_url { get; set; }
        public string callback_url { get; set; }
        internal GoogleFit(IAuthenticationService authService)
        {
            _authService = authService;

        }

       
        public long ConvertToUnixTimeMilliseconds(string iso8601DateTime)
        {
            DateTime dateTime = DateTime.Parse(iso8601DateTime);
            DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime);
            return dateTimeOffset.ToUnixTimeMilliseconds();
        }
        public int GetCurrentUnixTimestamp()
        {
            return (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

       
       



        
        public async Task FetchCalorieHistoryAsync()
        {
           
            Console.WriteLine(_authService.User.AccessToken);
            string AccessToken = _authService.User.AccessToken.ToString();
            var client = new HttpClient();
            var startDate = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd'T'HH:mm:ss.FFFFFFFK");
            var endDate = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss.FFFFFFFK");
            var requestUrl = $"https://www.googleapis.com/fitness/v1/users/me/dataSources/derived:com.google.calories.expended:com.google.android.gms:merge_calories_expended/datasets/{startDate}-{endDate}?access_token={AccessToken}";

            var response = await client.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Success...");
                // Process the JSON data and extract the calorie history
            }
            else
            {
                Console.WriteLine("Error fetching calorie history.");
                Console.WriteLine(response.RequestMessage.ToString());
                Console.WriteLine(response.StatusCode.ToString());
                Console.WriteLine(response.ReasonPhrase.ToString());
            }
        }







        public async Task FetchGoogleFitDataAsync(long startTimeMillis, long endTimeMillis)
        {
            try
            {
                var httpClient = new HttpClient();
                string Token = _authService.User.AccessToken.ToString();

                var requestUrl = "https://www.googleapis.com/fitness/v1/users/me/dataset:aggregate";

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                var requestBody = new
                {
                    aggregateBy = new[]
                    {
                new
                {
                    dataTypeName = "com.google.calories.expended",
                    dataSourceId = "derived:com.google.calories.expended:com.google.android.gms:merge_calories_expended"
                }
            },
                    bucketByTime = new { durationMillis = 86400000 },
                    startTimeMillis,
                    endTimeMillis
                };

                var jsonPayload = JsonConvert.SerializeObject(requestBody);

                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(requestUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseData);
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
       
    }
}

