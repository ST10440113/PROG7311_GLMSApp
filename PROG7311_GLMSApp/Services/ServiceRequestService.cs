using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROG7311_GLMSApp.Data;
using PROG7311_GLMSApp.Models;

namespace PROG7311_GLMSApp.Services
{
    public class ServiceRequestService
    {
        private readonly PROG7311_GLMSAppContext _context;

        public ServiceRequestService(PROG7311_GLMSAppContext context)
        {
            _context = context;
        }

        public async Task Create(ServiceRequest serviceRequest)
        {
            _context.Add(serviceRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<SelectList> GetContracts()
        {
            var contracts = await _context.Contract.ToListAsync();
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
