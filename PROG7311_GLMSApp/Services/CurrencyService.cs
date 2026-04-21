using PROG7311_GLMSApp.Models;

namespace PROG7311_GLMSApp.Services
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }

        public async Task<ExchangeRate> ConvertCurrencyAsync(double amount)
        {
            var response = await _httpClient.GetAsync($"39a42ce614d440f01d558d73/pair/USD/ZAR/{amount}");
            var exchangeData = await response.Content.ReadFromJsonAsync<ExchangeRate>();
            exchangeData.ConversionResult = ConvertToZar(amount, exchangeData.ConversionRate);
            return exchangeData;
        }

        public double ConvertToZar(double amount, double exchangeRate)
        {
            return amount * exchangeRate;
        }

    }
}

       
       