using MauiApp8.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp8.Services.ThirdPartyHealthService
{
    public interface IThirdPartyHealthService
    {
        public CommonHealthData CommonHealthData { get; set; }
        long DateTimeToUnixTimestampMillis(DateTime dateTime);
        Task FetchCalorieDataAsync(DateTime now, DateTime startTime);
        Task FetchActivityDataAsync(DateTime now, DateTime startTime);

        Task UpdateGoogleFitSteps();
    }
}
