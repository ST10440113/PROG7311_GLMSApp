using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROG7311_GLMSApp.Data;
using PROG7311_GLMSApp.Models;



namespace PROG7311_GLMSApp.Services
{
    public class ContractService
    {
        private readonly HttpClient _httpClient;
       
        private readonly PROG7311_GLMSAppContext _context;
        private readonly IContractFactory _icontractFactory;
        private List<IServiceRequestObserver> _observers = new();
        private readonly Notifier _notifier;

        public ContractService(IContractFactory icontractFactory, PROG7311_GLMSAppContext context, 
               Notifier notifier, HttpClient httpClient)
        {
            _icontractFactory = icontractFactory;
            _context = context;
            _notifier = notifier;
            _httpClient = httpClient;
            
        }

        public string CheckContractStatus(Contract contract)
        {
            if (contract.EndDate <= DateOnly.FromDateTime(DateTime.Now))
            {
                contract.Status = "Expired";
            }
            else
            {
                contract.Status = "Draft";
            }
            return contract.Status;
        }


        public async Task CreateAsync(Contract contract)
        {
            var SL = (_icontractFactory.Create(contract.ServiceLevel)).ServiceLevel;
            var newContract = new Contract
            {
                StartDate = contract.StartDate,
                EndDate = contract.EndDate,
                Status = CheckContractStatus(contract),
                ServiceLevel = SL,
                FilePath = contract.FilePath,
                ClientId = contract.ClientId

            };
            if (newContract.EndDate < newContract.StartDate)
            {
                throw new ArgumentException("End date cannot be earlier than start date");
            }
            else
            {
                var response = await _httpClient.PostAsJsonAsync("/api/Contract", newContract);
                 await response.Content.ReadFromJsonAsync<Contract>();
            }

        }

        public async Task<List<Contract>> GetAllContractsAsync()
        {
           var response = await _httpClient.GetAsync("api/Contract");
            if (response.IsSuccessStatusCode)
            {
                var contracts = await response.Content.ReadFromJsonAsync<List<Contract>>();
                foreach (var contract in contracts)
                {
                    if (contract.EndDate < DateOnly.FromDateTime(DateTime.Now))
                    {
                        contract.Status = "Expired";
                        await UpdateAsync(contract);
                    }
                }
                return contracts;
            }
            return null;
        }


        
        
        public async Task<Contract?> GetContractByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Contract/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Contract>();
            }
            return null;

        }

        public async Task<Contract?> UpdateAsync(Contract contract)
        {
            if (contract.EndDate < contract.StartDate)
            {
                throw new ArgumentException("End date cannot be earlier than start date");
            }
            else
            {
                var response = await _httpClient.PutAsJsonAsync($"/api/Contract/{contract.ContractId}", contract);
                return await response.Content.ReadFromJsonAsync<Contract>();

            }
        }

        public async Task<bool> ContractExists(int id)
        {
            var response = await _httpClient.GetAsync($"/api/ContractExists/{id}");
            if (response.IsSuccessStatusCode)
            {
                var contract = await response.Content.ReadFromJsonAsync<Contract>();
                return contract != null;
            }
            return false;
        }


        public async Task<bool> Delete(Contract request)
        {
            var response = await _httpClient.DeleteAsync($"/api/Contract/{request.ContractId}");
            return response.IsSuccessStatusCode;
        }
        
        public async Task<SelectList> ClientNames()
        {
            var client = await _httpClient.GetAsync("api/Client");
             var clients = await client.Content.ReadFromJsonAsync<List<Client>>();
            
            return new SelectList(clients, "ClientId", "FullName");
        }

        public async Task<List<Contract>> FilterByDateRange(DateOnly? startDate, DateOnly? endDate)
        {
            var response = await _httpClient.GetAsync($"/api/Contract/DateRange?startDate={startDate}&endDate={endDate}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Contract>>();
            }
            return null;

        }

        public async Task<List<Contract>> FilterByStatus(string status)
        {
            var response = await _httpClient.GetAsync($"/api/Contract/Status?status={status}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Contract>>();
            }
            return null;

        }
        

        public void CheckFileExtension(IFormFile file)
        {
            var allowedExtension = ".pdf";
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (allowedExtension != extension)
            {
                throw new ArgumentException("Invalid file type. Only PDF files are allowed.");
            }           
        }
    }
}
