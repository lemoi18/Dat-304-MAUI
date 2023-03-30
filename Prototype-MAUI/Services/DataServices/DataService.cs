using MauiApp8.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp8.Services.DataServices
{
    internal class DataService : IDataService
    {

        HttpClient httpClient;
        HttpClient Client => httpClient ?? (httpClient = new HttpClient());

        public Task<Food> GetFoodById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Food>> GetFoods()
        {
            //var json = await Client.GetStringAsync("https://montemagno.com/monkeys.json");
            //var json = await Client.GetStringAsync("https://xam-workshop-twitch-func.azurewebsites.net/api/GetAllMonkeys");
            //var all = Foods.FromJson(json);
            //return all;
            throw new NotImplementedException();
        }

        public async Task UpdateDetails(string newDetails, string FoodId)
        {

           // var detailUpdate = new DetailUpdate { NewDetails = newDetails };
            //var detailUpdateJson = JsonConvert.SerializeObject(detailUpdate);

            //string updateUri = $"https://xam-workshop-twitch-func.azurewebsites.net/api/update/{FoodId}";

            //var req = new HttpRequestMessage(HttpMethod.Post, updateUri);

            //req.Content = new StringContent(detailUpdateJson, Encoding.UTF8, "application/json");

            //await Client.SendAsync(req);

            throw new NotImplementedException();

        }
    }
}
