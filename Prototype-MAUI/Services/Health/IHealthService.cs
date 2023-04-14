using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MauiApp8.Model;

namespace MauiApp8.Services.Health
{
    public interface IHealthService
    {
        Task<List<GlucoseInfo>> ReadGlucoses(DateTimeOffset fromDate, DateTimeOffset toDate);
        Task<List<InsulinInfo>> ReadInsulins(DateTimeOffset fromDate, DateTimeOffset toDate);
    }
}
