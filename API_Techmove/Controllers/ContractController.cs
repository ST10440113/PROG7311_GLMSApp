using API_Techmove.Data;
using API_Techmove.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Techmove.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ContractController : Controller
    {
        private readonly DataContext _context;

        public ContractController(DataContext context)
        {
            _context = context;
        }

        // GET: api/<ContractController>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Contract>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Contract>>> GetContracts(DateOnly? startDate, DateOnly? endDate, string? status)
        {
            var allContracts = await _context.Contract.Include(c => c.Client).ToListAsync();
            if (startDate != null || endDate != null)
            {
                if (startDate != null && endDate != null)
                {
                    var contracts = FilterByDateRange(startDate, endDate);
                    return Ok(contracts);
                }
            }
            if (!string.IsNullOrEmpty(status))
            {
                var contracts = FilterByStatus(status);
                return Ok(contracts);
            }
            return Ok(allContracts);
        }



        // GET api/<ContractController>/5
        [HttpGet("{id}")]
        public async Task<Contract> GetContractByIdAsync(int id)
        {
            return await _context.Contract.FirstOrDefaultAsync(m => m.ContractId == id);
        }

        // POST api/<ContractController>
        [HttpPost]
        public async Task<ActionResult<Contract>> AddContract(Contract contract)
        {
            _context.Contract.Add(contract);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetContracts), new { id = contract.ContractId }, contract);
        }

        // PUT api/<ContractController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Contract>> UpdateContract(int id, Contract contract)
        {
            if (id != contract.ContractId)
            {
                return BadRequest();
            }
            _context.Entry(contract).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(contract);
        }

        // DELETE api/<ContractController>/5
        [HttpDelete("{id}")]
        public async Task DeleteContract(int id)
        {
            var contract = await GetContractByIdAsync(id);
            if (contract != null)
            {
                _context.Contract.Remove(contract);
            }

            await _context.SaveChangesAsync();
        }

        [HttpGet("ContractExists/{id}")]
        public bool ContractExists(int id)
        {
            return _context.Contract.Any(e => e.ContractId == id);
        }

        [HttpGet("FilterByDateRange")]
        public IEnumerable<Contract> FilterByDateRange(DateOnly? startDate, DateOnly? endDate)
        {
            var dateRangeQuery = from contract in _context.Contract select contract;
            var searchResults = dateRangeQuery.Where(c => c.StartDate >= startDate & c.EndDate <= endDate);
            return searchResults.ToList();

        }


        [HttpGet("FilterByStatus")]
        public IEnumerable<Contract> FilterByStatus(string status)
        {
            var statusQuery = from contract in _context.Contract select contract;
            var searchResults = statusQuery.Where(c => c.Status == status);
            return searchResults.ToList();

        }


    }
}


