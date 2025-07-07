using MediatR;
using PassengerInformation.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassengerInformation.Application.UseCases.PassengersUseCases.Queries
{
    public class GetAllPassengersQuery : IRequest<List<GetAllPassengersResponse>>
    {
    }

    public class GetAllPassengersResponse
    {
        public Guid Id { get; set; }
        public string FlightNumber { get; set; }
        public PassengerInfoDto Info { get; set; }
        public string? SeatNumber { get; set; }
        public List<Guid> AffiliatedPassengerIds { get; set; } = new();
    }

    public class GetAllPassengersQueryHandler
    : IRequestHandler<GetAllPassengersQuery, List<GetAllPassengersResponse>>
    {
        private readonly IPassengerRepository _passengerRepository;

        public GetAllPassengersQueryHandler(IPassengerRepository passengerRepository)
        {
            _passengerRepository = passengerRepository;
        }

        public async Task<List<GetAllPassengersResponse>> Handle(GetAllPassengersQuery request, CancellationToken cancellationToken)
        {
            var passengers = await _passengerRepository.GetAllAsync(cancellationToken);

            var response = passengers.Select(p => new GetAllPassengersResponse
            {
                Id = p.Id,
                FlightNumber = p.FlightNumber.Value,
                Info = new PassengerInfoDto
                {
                    Name = p.Info.Name,
                    Age = p.Info.Age,
                    Gender = p.Info.Gender,
                    Nationality = p.Info.Nationality,
                    SeatType = p.Info.SeatType.ToString()
                },
                SeatNumber = p.Info.Age <= 2 ? null : p.SeatNumber?.Value, 
                AffiliatedPassengerIds = p.AffiliatedPassengerIds.ToList()
            }).ToList();

            return response;
        }
    }
}
