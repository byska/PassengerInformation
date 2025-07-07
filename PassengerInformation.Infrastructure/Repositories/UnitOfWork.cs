using PassengerInformation.Application.Abstractions;
using PassengerInformation.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassengerInformation.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PassengerDbContext _context;

        public UnitOfWork(PassengerDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
