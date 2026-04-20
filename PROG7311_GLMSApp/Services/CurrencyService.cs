namespace PROG7311_GLMSApp.Services
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;
       

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            
        }

    }
}
