using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROG7311_GLMSApp.Data;
using PROG7311_GLMSApp.Models;


namespace PROG7311_GLMSApp.Services
{
    public class ContractService
    {
        private readonly IContractFactory _icontractFactory;
        private readonly PROG7311_GLMSAppContext _context;

        public ContractService(IContractFactory icontractFactory, PROG7311_GLMSAppContext context)
        {
            _icontractFactory = icontractFactory;
            _context = context;
        }

        public async Task CreateAsync(Contract contract)
        {
            var SL = (_icontractFactory.Create(contract.ServiceLevel)).ServiceLevel;
            var newContract = new Contract
            {
                StartDate = contract.StartDate,
                EndDate = contract.EndDate,
                Status = contract.Status,
                ServiceLevel = SL,
                ClientId = contract.ClientId

            };
            _context.Add(newContract);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Contract>> GetAllContractsAsync()
        {
            return await _context.Contract.Include(c => c.Client).ToListAsync();
        }
        public async Task<Contract> GetContractByIdAsync(int id)
        {
            return await _context.Contract.Include(c => c.Client).FirstOrDefaultAsync(c => c.ContractId == id);
        }

        public async Task UpdateAsync(Contract contract)
        {
            _context.Update(contract);
            await _context.SaveChangesAsync();
        }
        public bool ContractExists(int id)
        {
            return _context.Contract.Any(e => e.ContractId == id);
        }


        public async Task Delete(int id)
        {
            var contract = await GetContractByIdAsync(id);
            if (contract != null)
            {
                _context.Contract.Remove(contract);
            }

            await _context.SaveChangesAsync();
        }
    }
}
