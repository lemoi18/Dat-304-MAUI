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
    public class InsulinDataMessage : ValueChangedMessage<List<InsulinInfo>>
    {
        public InsulinDataMessage(List<InsulinInfo> value) : base(value)
        {
        }
    }

    public class InsulinChartMessage : ValueChangedMessage<ISeries>
    {
        public InsulinChartMessage(ISeries value) : base(value)
        {
        }
    }
}
