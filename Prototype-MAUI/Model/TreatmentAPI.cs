namespace MauiApp8.Model
{
    public class TreatmentAPI
    {
        public string _id { get; set; }
        public string eventType { get; set; }
        public DateTime created_at { get; set; }
        public string notes { get; set; }
        public string enteredBy { get; set; }
        public double duration { get; set; }
        public int? utcOffset { get; set; }
        public float? carbs { get; set; }
        public double? insulin { get; set; }
        public double? relative { get; set; }
    }
}
