using MediatR;
using PassengerInformation.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassengerInformation.Application.UseCases.PassengersUseCases.Queries
{
    public class GetPassengersByFlightQueryRequest : IRequest<List<GetPassengersByFlightResponse>>
    {
        public string FlightNumber { get; }

        public GetPassengersByFlightQueryRequest(string flightNumber)
        {
            FlightNumber = flightNumber;
        }
    }

    public class GetPassengersByFlightResponse
    {
        public Guid Id { get; set; }
        public PassengerInfoDto Info { get; set; }
        public string? SeatNumber { get; set; }
        public List<Guid> AffiliatedPassengerIds { get; set; } = new();
    }

    public class GetPassengersByFlightQueryHandler
     : IRequestHandler<GetPassengersByFlightQueryRequest, List<GetPassengersByFlightResponse>>
    {
        private readonly IPassengerRepository _passengerRepository;

        public GetPassengersByFlightQueryHandler(IPassengerRepository passengerRepository)
        {
            _passengerRepository = passengerRepository;
        }

        public async Task<List<GetPassengersByFlightResponse>> Handle(GetPassengersByFlightQueryRequest request, CancellationToken cancellationToken)
        {
            var passengers = await _passengerRepository.GetByFlightNumberAsync(request.FlightNumber, cancellationToken);

            var response = passengers.Select(p => new GetPassengersByFlightResponse
            {
                Id = p.Id,
                Info = new PassengerInfoDto
                {
                    Name = p.Info.Name,
                    Age = p.Info.Age,
                    Gender = p.Info.Gender,
                    Nationality = p.Info.Nationality,
                    SeatType = p.Info.SeatType.ToString()
                },
                SeatNumber = p.SeatNumber?.Value,
                AffiliatedPassengerIds = p.AffiliatedPassengerIds.ToList()
            }).ToList();

            return response;
        }
    }
}
