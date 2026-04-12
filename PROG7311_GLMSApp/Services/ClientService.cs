using Microsoft.EntityFrameworkCore;
using PROG7311_GLMSApp.Data;
using PROG7311_GLMSApp.Models;

namespace PROG7311_GLMSApp.Services
{
    public class ClientService
    {
        private readonly PROG7311_GLMSAppContext _context;
    
    public ClientService(PROG7311_GLMSAppContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Client client)
        {
            _context.Add(client);
           await _context.SaveChangesAsync();
          
        }
        public async Task UpdateAsync(Client client)
        {
            _context.Update(client);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Client client)
        {
            if (client != null)
            {
                _context.Client.Remove(client);
            }

            await _context.SaveChangesAsync();

        }
        public async Task<Client> GetClientByIdAsync(int id)
        {
            return await _context.Client.FirstOrDefaultAsync(m => m.ClientId == id);

        }
        
        public async Task<List<Client>> GetAllClientsAsync()
        {
            return await _context.Client.ToListAsync();
        }
       

        public bool ClientExists(int id)
        {
            return _context.Client.Any(e => e.ClientId == id);
        }

    }
}
