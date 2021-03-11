using Microsoft.EntityFrameworkCore;
using SharedLibrary.Models.Admin;
using SharedLibrary.Models.Customer;
using SharedLibrary.Models.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Data
{
    public class SqlDbContext : DbContext
    {
        public SqlDbContext(DbContextOptions<SqlDbContext> options) 
            : base(options) { }

        public DbSet<AdminModel> Administrators { get; set; }

        public DbSet<CustomerModel> Customers { get; set; }

        public DbSet<TicketModel> Tickets { get; set; }

        //public DbSet<SessionToken> SessionTokens { get; set; }
    }
}
