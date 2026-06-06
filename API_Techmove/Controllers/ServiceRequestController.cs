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
            var serviceRequests = await _context.ServiceRequests.ToListAsync();
            return Ok(serviceRequests);
        }




        // GET api/<ServiceRequestController>/5
        [HttpGet("{id}")]
        public async Task<ServiceRequest> GetServiceRequestByIdAsync(int id)
        {
            return await _context.ServiceRequests.FirstOrDefaultAsync(m => m.ServiceRequestId == id);
        }


        [HttpGet("FindContractByServiceRequestFK_Id/{id}")]
        public async Task<IActionResult> FindContractByServiceRequestFK_Id(int id)
        {
            var contract = await _context.Contract.FindAsync(id);
            if (contract == null) return NotFound();
            return Ok(contract);
        }



        // POST api/<ServiceRequestController>
        [HttpPost]
        public async Task<ActionResult<ServiceRequest>> AddServiceRequest(ServiceRequest serviceRequest)
        {
            _context.ServiceRequests.Add(serviceRequest);
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
                _context.ServiceRequests.Remove(serviceRequest);
            }

            await _context.SaveChangesAsync();
        }

        
    }
}


