using Microsoft.EntityFrameworkCore;
using PassengerInformation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassengerInformation.Infrastructure.Persistence
{
    public class PassengerDbContext : DbContext
    {
        public PassengerDbContext(DbContextOptions<PassengerDbContext> options)
            : base(options)
        {
        }

        public DbSet<Passenger> Passengers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PassengerDbContext).Assembly);
        }
    }
}
