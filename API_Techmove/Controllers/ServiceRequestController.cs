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
    public class ServiceRequestController : Controller
    {
        private readonly DataContext _context;

        public ServiceRequestController(DataContext context)
        {
            _context = context;
        }

        // GET: api/<ServiceRequestController>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ServiceRequest>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ServiceRequest>>> GetServiceRequests()
        {
            var serviceRequests = await _context.ServiceRequest.ToListAsync();
            return Ok(serviceRequests);
        }




        // GET api/<ServiceRequestController>/5
        [HttpGet("{id}")]
        public async Task<ServiceRequest> GetServiceRequestByIdAsync(int id)
        {
            return await _context.ServiceRequest.FirstOrDefaultAsync(m => m.ServiceRequestId == id);
        }




        // POST api/<ServiceRequestController>
        [HttpPost]
        public async Task<ActionResult<ServiceRequest>> AddServiceRequest(ServiceRequest serviceRequest)
        {
            _context.ServiceRequest.Add(serviceRequest);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetServiceRequestByIdAsync),
            new { id = serviceRequest.ServiceRequestId }, serviceRequest);
        }





        // PUT api/<ServiceRequestController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceRequest>> UpdateServiceRequest(int id, ServiceRequest serviceRequest)
        {
            if (id != serviceRequest.ServiceRequestId)
            {
                return BadRequest();
            }
            _context.Entry(serviceRequest).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(serviceRequest);
        }




        // DELETE api/<ServiceRequestController>/5
        [HttpDelete("{id}")]
        public async Task DeleteServiceRequest(int id)
        {
            var serviceRequest = await GetServiceRequestByIdAsync(id);
            if (serviceRequest != null)
            {
                _context.ServiceRequest.Remove(serviceRequest);
            }

            await _context.SaveChangesAsync();
        }

        [HttpGet("FindContractByServiceRequestFK_Id/{id}")]
        public async Task<Contract> FindContractByServiceRequestFK_Id(ServiceRequest sr)
        {
            var contract = await _context.Contract.FindAsync(sr.ContractId);
            return contract;
        }
    }
}


