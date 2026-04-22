using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROG7311_GLMSApp.Data;
using PROG7311_GLMSApp.Models;



namespace PROG7311_GLMSApp.Services
{
    public class ContractService
    {
        private readonly PROG7311_GLMSAppContext _context;
        private readonly IContractFactory _icontractFactory;
        private List<IServiceRequestObserver> _observers = new();
        private readonly Notifier _notifier;

        public ContractService(IContractFactory icontractFactory, PROG7311_GLMSAppContext context, Notifier notifier)
        {
            _icontractFactory = icontractFactory;
            _context = context;
            _notifier = notifier;
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
                _context.Add(newContract);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<List<Contract>> GetAllContractsAsync()
        {
            var contracts = await _context.Contract.Include(c => c.Client).ToListAsync();
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
        public async Task<Contract> GetContractByIdAsync(int id)
        {
            return await _context.Contract.Include(c => c.Client).FirstOrDefaultAsync(c => c.ContractId == id);

        }

        public async Task UpdateAsync(Contract contract)
        {
            if (contract.EndDate < contract.StartDate)
            {
                throw new ArgumentException("End date cannot be earlier than start date");
            }
            else
            {
                _context.Update(contract);
                await _context.SaveChangesAsync();
            }
            

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

        public async Task<SelectList> ClientNames()
        {
            var clients = await _context.Client.ToListAsync();
            return new SelectList(clients, "ClientId", "FullName");

        }

        public IEnumerable<Contract> FilterByDateRange(DateOnly? startDate, DateOnly? endDate)
        {
            var dateRangeQuery = from contract in _context.Contract select contract;
            var searchResults = dateRangeQuery.Where(c => c.StartDate >= startDate & c.EndDate <= endDate);
            return searchResults.ToList();

        }

        public IEnumerable<Contract> FilterByStatus(string status)
        {
            var statusQuery = from contract in _context.Contract select contract;
            var searchResults = statusQuery.Where(c => c.Status == status);
            return searchResults.ToList();

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
