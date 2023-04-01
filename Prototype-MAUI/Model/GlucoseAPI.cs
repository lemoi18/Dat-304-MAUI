using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp8.Model2
{
    public class GlucoseAPI
    {
        public string _id { get; set; }
        public string type { get; set; }
        public int sgv { get; set; }
        public DateTime dateString { get; set; }
        public object date { get; set; }
        public string app { get; set; }
        public string device { get; set; }
        public int utcOffset { get; set; }
        public DateTime sysTime { get; set; }
    }
}
