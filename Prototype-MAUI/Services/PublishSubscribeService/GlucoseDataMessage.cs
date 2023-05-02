using CommunityToolkit.Mvvm.Messaging.Messages;
using LiveChartsCore;
using MauiApp8.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp8.Services.PublishSubscribeService
{
    public class GlucoseDataMessage : ValueChangedMessage<List<GlucoseInfo>>
    {
        public GlucoseDataMessage(List<GlucoseInfo> value) : base(value)
        {
        }
    }
    public class GlucoseChartMessage : ValueChangedMessage<ISeries>
    {
        public GlucoseChartMessage(ISeries value) : base(value)
        {
        }
    }
}
