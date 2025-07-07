using PassengerInformation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassengerInformation.Application.Abstractions
{
    public interface IPassengerRepository
    {
        Task<List<Passenger>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(Passenger passenger, CancellationToken cancellationToken);
        Task<Passenger?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Passenger>> GetByFlightNumberAsync(string flightNumber, CancellationToken cancellationToken);
        void Update(Passenger passenger);
        void Remove(Passenger passenger);
    }
}

