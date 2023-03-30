using MauiApp8.ViewModel;
using MauiApp8.Model;
namespace MauiApp8.Views;

public partial class FoodItemView : ContentView
{
    public static readonly BindableProperty FoodItemProperty =
        BindableProperty.Create(nameof(FoodItem), typeof(FoodViewModel), typeof(FoodItemView));

    public FoodViewModel FoodItem
    {
        get => (FoodViewModel)GetValue(FoodItemProperty);
        set => SetValue(FoodItemProperty, value);
    }

    public FoodItemView()
    {
        InitializeComponent();
    }

    //async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    //{
    //    if (e.SelectedItem is FoodViewModel _selectedFoods)
    //    {
    //        // Add the selected food to the collection
    //        var viewModel = BindingContext as LogFoodModel;

    //        viewModel.SelectedFoodsVM.Add(_selectedFoods);

    //        await Navigation.PushAsync(new FoodDetailsPage(_selectedFoods));

    //        // Clear the selection
    //        ((ListView)sender).SelectedItem = null;
    //    }
    //}

    private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
            return;

        // Handle the selected item here

        ((ListView)sender).SelectedItem = null; // Clear the selection
    }


}