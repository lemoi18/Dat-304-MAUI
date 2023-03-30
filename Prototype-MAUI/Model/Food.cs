using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp8.Model
{
    public class Food : ObservableObject
    {
        
        private int id;

        public int Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        private string name;

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private string description;

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        private string category;

        public string Category
        {
            get => category;
            set => SetProperty(ref category, value);
        }

        private string carbohydrates;

        public string Carbohydrates
        {
            get => carbohydrates;
            set => SetProperty(ref carbohydrates, value);
        }

        private string protein;

        public string Protein
        {
            get => protein;
            set => SetProperty(ref protein, value);
        }

        private string fat;

        public string Fat
        {
            get => fat;
            set => SetProperty(ref fat, value);
        }
    }

}
