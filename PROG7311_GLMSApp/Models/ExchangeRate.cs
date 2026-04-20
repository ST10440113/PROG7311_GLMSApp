using System.Text.Json.Serialization;

namespace PROG7311_GLMSApp.Models
{
    public class ExchangeRate
    {
        [JsonPropertyName("result")] public string CalculatedAmount { get; set; }
        [JsonPropertyName("base_code")] public string BaseCode { get; set; }
        [JsonPropertyName("target_code")] public string TargetCode { get; set; }
        [JsonPropertyName("conversion_rate")] public double ConversionRate { get; set; }
        [JsonPropertyName("conversion_result")] public double ConversionResult { get; set; }
    }
}
