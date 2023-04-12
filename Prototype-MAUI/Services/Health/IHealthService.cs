using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MauiApp8.Model;

namespace MauiApp8.Services.Health
{
    internal interface IHealthService
    {
        List<GlucoseInfo> ReadGlucoses(DateTimeOffset fromDate, DateTimeOffset toDate);
        List<InsulinInfo> ReadInsulins(DateTimeOffset fromDate, DateTimeOffset toDate);
    }
}
