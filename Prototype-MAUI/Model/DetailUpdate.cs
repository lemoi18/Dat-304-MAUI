using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp8.Model
{
    public class DetailUpdate
    {
        [JsonProperty("newDetails")]
        public string NewDetails { get; set; }
    }
}
