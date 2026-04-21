using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Build.Evaluation;
using Microsoft.EntityFrameworkCore;
using PROG7311_GLMSApp.Data;
using PROG7311_GLMSApp.Models;

namespace PROG7311_GLMSApp.Services
{
    public class ServiceRequestService
    {
        private readonly PROG7311_GLMSAppContext _context;
        private readonly ContractContext _contractContext;
        private readonly Notifier _notifier;
        private readonly CurrencyService _currencyService;
        

        public ServiceRequestService(PROG7311_GLMSAppContext context, ContractContext contractContext, 
            Notifier notifier, CurrencyService currencyService)
        {
            _context = context;
            _contractContext = contractContext;
            _notifier = notifier;
            _currencyService = currencyService;
            
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

            var contract = await _context.Contract.FindAsync(serviceRequest.ContractId);
            var contractStatus = contract.Status;
            var stateChange = _contractContext.ChangeState(contractStatus);

            if (stateChange == true)
            {
                await Conversion(serviceRequest);

                _context.Add(serviceRequest);
                await _context.SaveChangesAsync();

              _notifier.Notify(serviceRequest.Status, serviceRequest.ContractId);
            
            }
            else
            {
              throw new InvalidOperationException($"Service Requests cannot be made for {contractStatus} contracts");
            }
          
        }

        public async Task<SelectList> GetContractsWithClients()
        {
            var contracts = await _context.Contract.Include(c => c.Client).ToListAsync();
            var contractSelectList = contracts.Select(c => new
            {
                ContractId = c.ContractId,
                listFormat = $"Contract {c.ContractId} - {c.Client.FullName}"}).ToList();
            return new SelectList(contractSelectList, "ContractId", "listFormat");
        }
       
        public async Task<SelectList> GetContractsByServiceRequestId(int serviceRequestId)
        {
            var serviceRequest = await _context.ServiceRequest.FindAsync(serviceRequestId);
            return new SelectList(_context.Contract, "ContractId", "ContractId", serviceRequest.ContractId);

        }
        public async Task<List<ServiceRequest>> GetAllServiceRequestsAsync()
        {
            return await _context.ServiceRequest.Include(s => s.Contract).ToListAsync();
        }
        public async Task<ServiceRequest> GetServiceRequestByIdAsync(int id)
        {
            return await _context.ServiceRequest.Include(s => s.Contract).FirstOrDefaultAsync(m => m.ServiceRequestId == id);
        }

        public async Task UpdateAsync(ServiceRequest serviceRequest)
        {
            var manager = new Notification(serviceRequest.ContractId, serviceRequest.Status);
            _notifier.Subscribe(manager);
            _context.Update(serviceRequest); 
            _notifier.Notify(serviceRequest.Status, serviceRequest.ContractId);
            await Conversion(serviceRequest);
            await _context.SaveChangesAsync();
            
        }
        public async Task DeleteAsync(int id) 
        {
            var serviceRequest = await GetServiceRequestByIdAsync(id);
            if (serviceRequest != null)
            {
                _context.ServiceRequest.Remove(serviceRequest);
            }

            await _context.SaveChangesAsync();
        }

        public bool ServiceRequestExists(int id)
        {
            return _context.ServiceRequest.Any(e => e.ServiceRequestId == id);
        }
    }
}
