using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PassengerInformation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PassengerInformation.Domain.ValueObjects;

namespace PassengerInformation.Infrastructure.Persistence.Configurations
{
    public class PassengerConfiguration : IEntityTypeConfiguration<Passenger>
    {
        public void Configure(EntityTypeBuilder<Passenger> builder)
        {
            builder.ToTable("Passengers");

            builder.HasKey(x => x.Id);

           
            builder.OwnsOne(x => x.FlightNumber, fn =>
            {
                fn.Property(f => f.Value)
                  .HasColumnName("FlightNumber")
                  .IsRequired();
            });

            builder.OwnsOne(x => x.Info, pi =>
            {
                pi.Property(p => p.Name).HasColumnName("Name").IsRequired();
                pi.Property(p => p.Age).HasColumnName("Age").IsRequired();
                pi.Property(p => p.Gender).HasColumnName("Gender").IsRequired();
                pi.Property(p => p.Nationality).HasColumnName("Nationality").IsRequired();
                pi.Property(p => p.SeatType).HasColumnName("SeatType").IsRequired();
            });

            builder.OwnsOne(x => x.SeatNumber, sn =>
            {
                sn.Property(s => s.Value)
                  .HasColumnName("SeatNumber")
                  .IsRequired(false);
            });

            builder.Property(x => x.AffiliatedPassengerIds)
        .HasConversion(
             list => string.Join(',', list),
             str => string.IsNullOrWhiteSpace(str)
                 ? new List<Guid>()
                 : str.Split(',', StringSplitOptions.RemoveEmptyEntries)
                      .Select(Guid.Parse)
                      .ToList()
         )
        .HasColumnName("AffiliatedPassengerIds");

        }
    }
}
