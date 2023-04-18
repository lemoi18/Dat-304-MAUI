using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp8.Model;
using MauiApp8.Services.Authentication;
using MauiApp8.Services.Food;
using MauiApp8.Views;
using MvvmHelpers;
using System.Windows.Input;

namespace MauiApp8.ViewModel
{

    [QueryProperty(nameof(Food), nameof(Food))]
    [QueryProperty(nameof(Grams), nameof(Grams))]


    public partial class LogFoodModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {
        //For testing 
        private string _createdMealText;
        public string CreatedMealText
        {
            get => _createdMealText;
            set => SetProperty(ref _createdMealText, value);
        }


        IAuthenticationService authService;
        IFoodService foodService;

        public IFoodService FoodService { get; }

        private ICommand _createMealCommand;
        public ICommand CreateMealCommand => _createMealCommand ?? (_createMealCommand = new Command(async () => await CreateMealAsync()));

        private async Task CreateMealAsync()
        {
            List<int> foodEntryIds = new List<int>();

            foreach (var foodVM in SelectedFoodsVM)
            {
                int foodEntryId = await foodService.CreateFoodEntry(foodVM.Name, (float)foodVM.Grams);
                foodEntryIds.Add(foodEntryId);
            }

            await foodService.CreateMeal(foodEntryIds);

            // Update the created meal text
            CreatedMealText = $"Meal created with {foodEntryIds.Count} food entries";
            
            SelectedFoodsVM.Clear();
        }

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

        private async Task CreateMeal()
        {
            List<int> foodEntryIds = new List<int>();

            foreach (var foodVM in SelectedFoodsVM)
            {
                int foodEntryId = await foodService.CreateFoodEntry(foodVM.Name, (float)foodVM.Grams);
                foodEntryIds.Add(foodEntryId);
            }

            await foodService.CreateMeal(foodEntryIds);

            SelectedFoodsVM.Clear();

        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    Find(value);
                }
            }
        }



        public LogFoodModel(IAuthenticationService authService, IFoodService foodService)
        {
            this.authService = authService;
            this.FoodService = foodService;

            this.Foods = new MvvmHelpers.ObservableRangeCollection<Food>();
            this.FoodVM = new MvvmHelpers.ObservableRangeCollection<FoodViewModel>();
            this.selectedFoodsVM = new MvvmHelpers.ObservableRangeCollection<FoodViewModel>();
            Task.Run(() => InitializeAsync());
        }

        private async Task InitializeAsync()
        {
            await LoadFoodsAsync();
        }


        private async Task LoadFoodsAsync(string query = null)
        {
            var foodServiceResult = await this.foodService.GetFoods();
            if (foodServiceResult == null)
            {
                await Shell.Current.DisplayAlert(
                              "Error",
                               "An error occurred.",
                               "Close");
                return;
            }

            Console.WriteLine($"Loaded {foodServiceResult.Count} food items from the foodService");

            this.Foods = new ObservableRangeCollection<Food>(foodServiceResult);
            this.FoodVM.ReplaceRange(this.Foods.Select(food => new FoodViewModel(food)));
        }


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
                this.FoodVM.ReplaceRange(this.Foods.Select(food => new FoodViewModel(food)));
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

                Console.WriteLine($"Query: '{query}', Filtered results count: {filteredResults.Count}"); // Add this line
            }
        }

        //public ICommand NavigateToFoodDetailsCommand { get; }


        [RelayCommand]

        async Task Find(string query) => await UpdateSearchResultsAsync(query);




        [RelayCommand]
        Task RemoveFoodFromList(FoodViewModel foodView)
        {
            SelectedFoodsVM.Remove(foodView);
            return Task.CompletedTask;

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
        async Task RemoveSelectedFoods(FoodViewModel foodView)
        {
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


