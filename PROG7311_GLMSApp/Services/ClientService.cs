using Azure.Core;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;

using PROG7311_GLMSApp.Models;


namespace PROG7311_GLMSApp.Services
{
    public class ClientService
    {
        
        private readonly HttpClient _httpClient;
       
        public ClientService( HttpClient httpClient )
        {   
            _httpClient = httpClient;
            
        }
       
        public async Task<Client?> CreateAsync(Client request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Client", request);
            return await response.Content.ReadFromJsonAsync<Client>();
        }
        
        public async Task<Client?> UpdateAsync(Client request)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/Client/{request.ClientId}", request);
            return await response.Content.ReadFromJsonAsync<Client>();

        }

        public async Task<bool> Delete(Client request)
        {
            var response = await _httpClient.DeleteAsync($"/api/Client/{request.ClientId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<Client?> GetClientByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Client/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Client>();
            }
            return null;

        }
        public async Task<List<Client>?> GetAllClientsAsync()
        {
            var response = await _httpClient.GetAsync("/api/Client");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Client>>();
            }
            return null;
        }

        public async Task<bool> ClientExists(int id)
        {
            var response = await _httpClient.GetAsync($"/api/ClientExists/{id}");
            if (response.IsSuccessStatusCode)
            {
                var client = await response.Content.ReadFromJsonAsync<Client>();                
            }
            return false;
        }

    }
}
