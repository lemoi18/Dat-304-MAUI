﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp8.Services.BackgroundServices.Realm;

namespace MauiApp8.Services.BackgroundServices
{
    public interface IBackgroundService
    {
        Task AddGlucoseEntry(float sgv, DateTimeOffset date);
        Task AddInsulinEntry(double? insulin,double? basal ,DateTimeOffset date);
        DateTimeOffset? ReadLatestGlucose();
        DateTimeOffset? ReadLatestInsulin();
        Task<int> UpdateGlucose(string domainName);
        Task<int> UpdateInsulin(string domainName);

        DateTimeOffset Get_NewestTimestamp(DateTimeOffset first_datetime, DateTimeOffset second_datetime);

        Task<double?> GetBasalInsulin(string DomainName, DateTimeOffset time);

        Task<float?> ReadLatestGlucoseValue();
    }
}
