using Microsoft.AspNetCore.Mvc.Rendering;
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

        public ServiceRequestService(PROG7311_GLMSAppContext context, ContractContext contractContext)
        {
            _context = context;
            _contractContext = contractContext;
        }

        public async Task Create(ServiceRequest serviceRequest)
        {
            var contract = await _context.Contract.FindAsync(serviceRequest.ContractId);
            var contractStatus = contract.Status;
            var stateChange = _contractContext.ChangeState(contractStatus);

            if (stateChange == true)
            {
                _context.Add(serviceRequest);
                await _context.SaveChangesAsync();
            }
            else
            {
              throw new InvalidOperationException($"Service Request cannot be made for a/an {contractStatus} contract");
            }
          
        }

        public SelectList GetContracts()
        {
            var contracts = _context.Contract.ToList();
            return new SelectList(contracts, "ContractId", "ContractId");

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
            _context.Update(serviceRequest);
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
