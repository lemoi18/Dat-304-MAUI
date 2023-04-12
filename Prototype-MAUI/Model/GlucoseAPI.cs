namespace MauiApp8.Model
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
