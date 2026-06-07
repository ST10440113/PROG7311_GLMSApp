using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Build.Evaluation;
using Microsoft.EntityFrameworkCore;
using PROG7311_GLMSApp.Models;

namespace PROG7311_GLMSApp.Services
{
    public class ServiceRequestService
    {
        private readonly HttpClient _httpClient;
        
        private readonly ContractContext _contractContext;
        private readonly Notifier _notifier;
        private readonly CurrencyService _currencyService;
        

        public ServiceRequestService( ContractContext contractContext, 
               Notifier notifier, CurrencyService currencyService, HttpClient httpClient)
        {
           
            _contractContext = contractContext;
            _notifier = notifier;
            _currencyService = currencyService;
            _httpClient = httpClient;
           
        }

        public async Task Conversion(ServiceRequest serviceRequest)
        {
            var conversion = await _currencyService.ConvertCurrencyAsync(serviceRequest.Cost);
            if (conversion != null)
            {
                serviceRequest.ZarAmount = conversion.ConversionResult;
            }
        }
        
        
        public async Task Create(ServiceRequest serviceRequest)
        {      
            var manager = new Notification(serviceRequest.ContractId, serviceRequest.Status);
            _notifier.Subscribe(manager);

            var contractResponse = await _httpClient.GetAsync($"api/ServiceRequest/FindContractByServiceRequestFK_Id/{serviceRequest.ContractId}");
            var contract = await contractResponse.Content.ReadFromJsonAsync<Contract>();

            var contractStatus = contract.Status;
            var stateChange = _contractContext.ChangeState(contractStatus);

            if (stateChange == true)
            {
                await Conversion(serviceRequest);

                var response = await _httpClient.PostAsJsonAsync("api/ServiceRequest", serviceRequest);              
                await response.Content.ReadFromJsonAsync<ServiceRequest>();

                _notifier.Notify(serviceRequest.Status, serviceRequest.ContractId);           
            }
            else
            {
              throw new InvalidOperationException($"Service Requests cannot be made for {contractStatus} contracts");
            }          
        }



        public async Task<SelectList> GetContractsWithClients()
        {
            var response = await _httpClient.GetAsync("api/Contract");
           
                var contracts = await response.Content.ReadFromJsonAsync<List<Contract>>();
                var contractSelectList = contracts.Select(c => new
                {
                    ContractId = c.ContractId,
                    listFormat = $"Contract {c.ContractId} - {c.Client.FullName}"
                }).ToList();

                return new SelectList(contractSelectList, "ContractId", "listFormat");            
        }

            
        public async Task<SelectList> GetContractsByServiceRequestId(int serviceRequestId)
        {            
            var response = await _httpClient.GetAsync($"api/ServiceRequest/{serviceRequestId}");
            var serviceRequest = await response.Content.ReadFromJsonAsync<ServiceRequest>();
            return new SelectList(new List<ServiceRequest> { serviceRequest }, "ContractId", "ContractId", serviceRequest.ContractId);

        }


        
        public async Task<List<ServiceRequest>?> GetAllServiceRequestsAsync()
        {
            var response = await _httpClient.GetAsync("api/ServiceRequest");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<ServiceRequest>>();
            }
            return null;
        }



        public async Task<ServiceRequest?> GetServiceRequestByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/ServiceRequest/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ServiceRequest>();
            }
            return null;

        }


        public async Task UpdateAsync(ServiceRequest serviceRequest)
        {
            var manager = new Notification(serviceRequest.ContractId, serviceRequest.Status);
            _notifier.Subscribe(manager);
            _notifier.Notify(serviceRequest.Status, serviceRequest.ContractId);
            await Conversion(serviceRequest);
            var response = await _httpClient.PutAsJsonAsync($"api/ServiceRequest/{serviceRequest.ServiceRequestId}", serviceRequest);
            await response.Content.ReadFromJsonAsync<ServiceRequest>();

        }


        public async Task<bool> Delete(ServiceRequest request)
        {
            var response = await _httpClient.DeleteAsync($"api/ServiceRequest/{request.ServiceRequestId}");
            return response.IsSuccessStatusCode;
        }


        public async Task<bool> ServiceRequestExists(int id)
        {
            var response = await _httpClient.GetAsync($"api/ServiceRequestExists/{id}");
            if (response.IsSuccessStatusCode)
            {
                var serviceRequest = await response.Content.ReadFromJsonAsync<ServiceRequest>();
                return serviceRequest != null;
            }
            return false;
        }
    }
}
