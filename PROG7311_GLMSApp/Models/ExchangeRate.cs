namespace PROG7311_GLMSApp.Models
{
    public class ExchangeRate
    {
        public string CalculatedAmount { get; set; }
        public string BaseCode { get; set; }
        public string TargetCode { get; set; }
        public double ConversionRate { get; set; }
        public double ConversionResult { get; set; }
    }
}
