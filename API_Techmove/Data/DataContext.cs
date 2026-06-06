using API_Techmove.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace API_Techmove.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Contract> Contract { get; set; }
        public DbSet<ServiceRequest> ServiceRequest { get; set; }
    }
}
