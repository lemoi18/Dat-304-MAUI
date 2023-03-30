using MauiApp8.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp8.Services.DataServices
{
    public interface IDataService
    {
        Task<List<Food>> GetFoods();
        Task UpdateDetails(string newDetails, string FoodId);
        Task<Food> GetFoodById(int id);
    }
}
