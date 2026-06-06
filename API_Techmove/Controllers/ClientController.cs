using API_Techmove.Data;
using API_Techmove.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Techmove.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ClientController : Controller
    {
        private readonly DataContext _context;

        public ClientController(DataContext context)
        {
            _context = context;
        }
        // GET: api/<ClientController>

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Client>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            var clients = await _context.Clients.ToListAsync();
            return Ok(clients);
        }


        // GET api/<ClientController>/5
        [HttpGet("{id}")]
        public async Task<Client> GetClientByIdAsync(int id)
        {
            return await _context.Clients.FirstOrDefaultAsync(m => m.ClientId == id);
        }

        // POST api/<ClientController>
        [HttpPost]
        public async Task<ActionResult<Client>> AddClient(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetClients), new { id = client.ClientId }, client);
        }


        // PUT api/<ClientController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Client>> UpdateClient(int id, Client client)
        {
            if (id != client.ClientId)
            {
                return BadRequest();
            }
            _context.Entry(client).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(client);
        }

        // DELETE api/<ClientController>/5
        [HttpDelete("{id}")]
        public async Task DeleteClient(int id)
        {
            var client = await GetClientByIdAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
            }

            await _context.SaveChangesAsync();
        }

        [HttpGet("ClientExists/{id}")]
        public bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.ClientId == id);
        }
    }
}
