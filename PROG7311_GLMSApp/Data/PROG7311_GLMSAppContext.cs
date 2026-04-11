using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PROG7311_GLMSApp.Models;

namespace PROG7311_GLMSApp.Data
{
    public class PROG7311_GLMSAppContext : DbContext
    {
        public PROG7311_GLMSAppContext (DbContextOptions<PROG7311_GLMSAppContext> options)
            : base(options)
        {
        }

        public DbSet<PROG7311_GLMSApp.Models.Client> Client { get; set; } = default!;
    }
}
