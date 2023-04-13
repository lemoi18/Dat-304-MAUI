using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace MauiApp8.Model
{
    public class CommonHealthData : ObservableObject
    {
        public ObservableCollection<CommonActivityData> ActivityDataList { get; set; }
        public ObservableCollection<CommonCalorieData> CalorieDataList { get; set; }
        public ObservableCollection<CommonStepData> StepDataList { get; set; }
    }

    public class CommonActivityData: ObservableObject
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int ActivityType { get; set; }
        public int Count { get; set; }
        public TimeSpan ActivityDuration { get; set; }
    }

    public class CommonCalorieData: ObservableObject
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double Calories { get; set; }
    }

    public class CommonStepData:    ObservableObject
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Steps { get; set; }
    }
}
