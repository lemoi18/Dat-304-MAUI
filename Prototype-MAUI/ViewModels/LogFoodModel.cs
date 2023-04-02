

using MauiApp8.Model;
using MauiApp8.Services.Authentication;
using MauiApp8.Services.DataServices;
using MvvmHelpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp8.Views;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
namespace MauiApp8.ViewModel
{

    [QueryProperty(nameof(Food), nameof(Food))]
    [QueryProperty(nameof(Grams), nameof(Grams))]


    public partial class LogFoodModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {
        IAuthenticationService authService;
        IDataService dataService;
        
        private string _searchText;


        [ObservableProperty]
        double grams;

       

        [ObservableProperty]
        Food food;



        [ObservableProperty]
        Account _User;

        [ObservableProperty]
        MvvmHelpers.ObservableRangeCollection<FoodViewModel> foodVM;
        [ObservableProperty]
        MvvmHelpers.ObservableRangeCollection<Food> foods;
        [ObservableProperty]
        FoodViewModel selectedFood;
        [ObservableProperty]
        MvvmHelpers.ObservableRangeCollection<FoodViewModel> selectedFoodsVM;



        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                Task.Run(()=> UpdateSearchResultsAsync(value));

            }
        }



        public LogFoodModel(IAuthenticationService authService, IDataService dataService)
        {

            this.authService = authService;
            
           this.dataService = dataService;
            this.Foods = new MvvmHelpers.ObservableRangeCollection<Food>();
            this.FoodVM = new MvvmHelpers.ObservableRangeCollection<FoodViewModel>();
            this.selectedFoodsVM= new MvvmHelpers.ObservableRangeCollection<FoodViewModel>();
            Task.Run(() => InitializeAsync());
            //NavigateToFoodDetailsCommand = new RelayCommand<FoodViewModel>(NavigateToFoodDetails);

        }

        //private async void NavigateToFoodDetails(FoodViewModel selectedFoodViewModel)
        //{
        //    var route = $"{nameof(FoodDetailsModel)}?foodId={selectedFoodViewModel.Food.Id}";
        //    await Shell.Current.GoToAsync(route);
        //}
        private async Task InitializeAsync()
        {
            await LoadFoodsAsync("");
            //NavigateToFoodDetailsCommand = new RelayCommand<FoodViewModel>(NavigateToFoodDetails);
        }

        private async Task LoadFoodsAsync(string query = null)
        {
            var foodService = await dataService.GetFoods();
            if (foodService == null)
            {
                await Shell.Current.DisplayAlert(
                          "Error",
                           "An error occurred.",
                           "Close");
                return;
            }
            
            Console.WriteLine($"Loaded {foodService.Count} food items from the dataService");

            this.Foods = new ObservableRangeCollection<Food>(foodService);
            if (!string.IsNullOrEmpty(query))
            {
                var filteredResults = await Task.Run(() => this.Foods
                    .Where(p => !string.IsNullOrEmpty(p.Name) &&
                            p.Name.ToLower().Contains(query.ToLower())).ToList());
                this.FoodVM.ReplaceRange(filteredResults.Select(food => new FoodViewModel(food)));
            }

        }





        //public async Task UpdateSearchResultsAsync(string query)
        //{
        //    if (this.Foods == null)
        //    {
        //        Console.WriteLine("FoodCollection is null");
        //        return;
        //    }

        //    if (this.FoodVM == null)
        //    {
        //        Console.WriteLine("FoodCollectionVM is null");
        //        return;
        //    }

        //    if (string.IsNullOrEmpty(query))
        //    {
        //        // populate the list with items when query is empty
        //        //this.FoodVM.ReplaceRange(this.Foods?.Select(food => new FoodViewModel(food)) ?? Enumerable.Empty<FoodViewModel>());
        //        this.FoodVM.Clear();
        //        return;
        //    }
        //    else
        //    {
        //        var filteredResults = await Task.Run(() => this.Foods
        //           .Where(p => !string.IsNullOrEmpty(p.Name) &&
        //                  p.Name.ToLower().Contains(query.ToLower())).ToList());
        //        this.FoodVM.ReplaceRange(filteredResults.Select(food => new FoodViewModel(food)));
        //    }
        //    await Task.CompletedTask;
        //}

        private CancellationTokenSource _debounceTokenSource;

        public async Task UpdateSearchResultsAsync(string query)
        {
            if (this.Foods == null)
            {
                Console.WriteLine("FoodCollection is null");
                return;
            }

            if (this.FoodVM == null)
            {
                Console.WriteLine("FoodCollectionVM is null");
                return;
            }

            if (string.IsNullOrEmpty(query))
            {
                Console.WriteLine(string.IsNullOrEmpty(query));
                this.FoodVM.Clear();
                return;
            }
            else
            {
                // Cancel the previous debounce if it exists
                _debounceTokenSource?.Cancel();

                // Create a new debounce with a 500ms delay
                _debounceTokenSource = new CancellationTokenSource();
                try
                {
                    await Task.Delay(500, _debounceTokenSource.Token);
                }
                catch (TaskCanceledException)
                {
                    return;
                }

                var filteredResults = await Task.Run(() => this.Foods
                    .Where(p => !string.IsNullOrEmpty(p.Name) &&
                            p.Name.ToLower().Contains(query.ToLower())).ToList());
                this.FoodVM.ReplaceRange(filteredResults.Select(food => new FoodViewModel(food)));
            }
        }

        //public ICommand NavigateToFoodDetailsCommand { get; }


        [RelayCommand]

        async Task Find(string query) => await UpdateSearchResultsAsync(query);




        [RelayCommand]
         Task RemoveFoodFromList(FoodViewModel foodView)
        {
            SelectedFoodsVM.Remove(foodView);
            return  Task.CompletedTask;

        }
        [RelayCommand]
        async Task EditFoodFromList(FoodViewModel foodView)
        {

            
            
                var parameters = new Dictionary<string, object>();

                if (!parameters.ContainsKey("Food"))
                {
                    parameters.Add("Food", foodView.Food);
                }

                foodView.IsEdit = true;
                await Shell.Current.GoToAsync($"{nameof(FoodDetailsPage)}?IsEdit={foodView.IsEdit}", parameters);
            

        }
        [RelayCommand]
         async Task RemoveSelectedFoods(FoodViewModel foodView) {
            var selectedFoods = this.SelectedFoodsVM.Where(vm => vm.IsSelected).ToList();

            if (!selectedFoods.Any())
            {
                return;
            }

            var confirmResult = await Shell.Current.DisplayAlert("Confirm", "Are you sure you want to remove the selected foods?", "Yes", "No");
            if (!confirmResult)
            {
                return;
            }

            foreach (var selectedFood in selectedFoods)
            {
                this.SelectedFoodsVM.Remove(selectedFood);
            }
        }

    }
}


