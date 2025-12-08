namespace AiFinanceTracker.Models
{
    public class ChartData
    {
        public string[] Labels { get; set; } = Array.Empty<string>();
        public double[] Values { get; set; } = Array.Empty<double>();
        public string Label { get; set; } = "";
    }
}
