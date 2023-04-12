using Google.Apis.PeopleService.v1.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp8.Model;
using MauiApp8.ViewModel;

namespace MauiApp8.Controls
{




    public class FoodSearchHandler : SearchHandler
    {

        public FoodSearchHandler()
        {
        }

        public ObservableCollection<object> SearchResults { get; set; } = new ObservableCollection<object>();

        protected override async void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);

            if (string.IsNullOrWhiteSpace(newValue))
            {
                ClearSearchResults();
            }
            else
            {
                await UpdateSearchResultsAsync(newValue);
            }
        }

        private async Task UpdateSearchResultsAsync(string query)
        {
            var results = await GetSearchResultsAsync(query);

            ClearSearchResults();
            foreach (var result in results)
            {
                SearchResults.Add(result);
            }

            ItemsSource = SearchResults;
        }

        private void ClearSearchResults()
        {
            SearchResults.Clear();
            ItemsSource = null;
        }

        private Task<ObservableCollection<object>> GetSearchResultsAsync(string query)
        {
            throw new NotImplementedException();
        }


    }
}
