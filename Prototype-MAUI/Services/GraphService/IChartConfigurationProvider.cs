﻿using LiveChartsCore;
using MauiApp8.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp8.Services.GraphService
{
    public interface IChartConfigurationProvider
    {
        ChartConfiguration GetChartConfiguration();
        ChartConfiguration GetBolosChartConfiguration();

    }
}
