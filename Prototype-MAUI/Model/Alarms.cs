using MauiApp8.Services.PublishSubscribeService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp8.Model
{
    public class Alarms
    {
        public class Alarm
        {
            public bool On { get; set; }
            public int Response { get; set; }

            public string Message { get; set; }

        }

        public class Authenticate
        {
            public bool isAuth { get; set; }
           

        }
    }
}
