using System.Text.Json.Serialization;
using System.Collections.Generic;


namespace MauiApp8.Services.GoogleFitService
{
   
         public class GoogleFitData
                    {
        [JsonPropertyName("bucket")]
        public List<Bucket> Buckets { get; set; }
                }

    public class Bucket
    {
        [JsonPropertyName("startTimeMillis")]
        public string StartTimeMillis { get; set; }

        [JsonPropertyName("endTimeMillis")]
        public string EndTimeMillis { get; set; }

        [JsonPropertyName("dataset")]
        public List<Dataset> Dataset { get; set; }
    }

    public class Dataset
    {
        [JsonPropertyName("dataSourceId")]
        public string DataSourceId { get; set; }

        [JsonPropertyName("point")]
        public List<Point> Point { get; set; }
    }

    public class Point
    {
        [JsonPropertyName("startTimeNanos")]
        public string StartTimeNanos { get; set; }

        [JsonPropertyName("endTimeNanos")]
        public string EndTimeNanos { get; set; }

        [JsonPropertyName("dataTypeName")]
        public string DataTypeName { get; set; }

        [JsonPropertyName("originDataSourceId")]
        public string OriginDataSourceId { get; set; }

        [JsonPropertyName("value")]
        public List<Value> Value { get; set; }
    }

    public class Value
    {
        [JsonPropertyName("fpVal")]
        public float? FpVal { get; set; }

        [JsonPropertyName("intVal")]

        public int? IntVal { get; set; }

        [JsonPropertyName("mapVal")]
        public List<object> MapVal { get; set; }
    }

    public class GoogleFitActivityData
    {
        public List<Point> Value { get; set; }
        public string StartTimeNanos { get; set; }
        public string EndTimeNanos { get; set; }
        
        public int ActivityId { get; set; }
        public string ActivityDuration { get; set; }
    }

    public class GoogleFitCalorieData
    {
        public List<Point> Value { get; set; }
        public string StartTimeNanos { get; set; }
        public string EndTimeNanos { get; set; }
    }

    public class GoogleFitStepData
    {
        public List<Point> Value { get; set; }
        public string StartTimeNanos { get; set; }
        public string EndTimeNanos { get; set; }
    }
}
