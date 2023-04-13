using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace MauiApp8.Model
{
    public class BasalData
    {
        public class NightscoutProfile
        {
            [JsonPropertyName("_id")]
            public string Id { get; set; }

            [JsonPropertyName("defaultProfile")]
            public string DefaultProfile { get; set; }

            [JsonPropertyName("store")]
            public Dictionary<string, Store> Store { get; set; }

            [JsonPropertyName("startDate")]
            public DateTimeOffset StartDate { get; set; }

            [JsonPropertyName("mills")]
            public string Mills { get; set; }

            [JsonPropertyName("units")]
            public string Units { get; set; }

            [JsonPropertyName("created_at")]
            public DateTimeOffset CreatedAt { get; set; }
        }

        public class Store
        {
            [JsonPropertyName("carbratio")]
            public List<object> Carbratio { get; set; }

            [JsonPropertyName("sens")]
            public List<object> Sens { get; set; }

            [JsonPropertyName("basal")]
            public List<Basal> Basal { get; set; }

            [JsonPropertyName("target_low")]
            public List<object> TargetLow { get; set; }

            [JsonPropertyName("target_high")]
            public List<object> TargetHigh { get; set; }
        }

        public class Basal
        {
            [JsonPropertyName("time")]
            public string Time { get; set; }

            [JsonPropertyName("value")]
            public string Value { get; set; }

            [JsonPropertyName("timeAsSeconds")]
            public string TimeAsSeconds { get; set; }
        }
    }
}
