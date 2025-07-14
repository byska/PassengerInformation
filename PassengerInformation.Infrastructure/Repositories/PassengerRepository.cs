using Microsoft.EntityFrameworkCore;
using PassengerInformation.Application.Abstractions;
using PassengerInformation.Domain.Entities;
using PassengerInformation.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassengerInformation.Infrastructure.Repositories
{
    public class PassengerRepository : IPassengerRepository
    {
        private readonly PassengerDbContext _context;

        public PassengerRepository(PassengerDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Passenger passenger, CancellationToken cancellationToken)
        {
            passenger.CreateDate = DateTime.Now;
            await _context.Passengers.AddAsync(passenger, cancellationToken);
        }

        public async Task<List<Passenger>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Passengers.Where(x=>x.IsActive).ToListAsync(cancellationToken);
        }

        public async Task<Passenger?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Passengers
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive, cancellationToken);
        }

        public async Task<List<Passenger>> GetByFlightNumberAsync(string flightNumber, CancellationToken cancellationToken)
        {
            return await _context.Passengers
                .Where(p => p.FlightNumber.Value == flightNumber && p.IsActive)
                .ToListAsync(cancellationToken);
        }

        public void Update(Passenger passenger)
        {
            passenger.UpdateDate = DateTime.UtcNow;
            _context.Passengers.Update(passenger);
        }

        
    }
}
