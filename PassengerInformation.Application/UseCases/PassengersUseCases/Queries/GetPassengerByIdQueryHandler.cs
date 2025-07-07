using MediatR;
using PassengerInformation.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassengerInformation.Application.UseCases.PassengersUseCases.Queries
{
    public class GetPassengerByIdQueryRequest : IRequest<GetPassengerByIdResponse?>
    {
        public Guid PassengerId { get; }

        public GetPassengerByIdQueryRequest(Guid passengerId)
        {
            PassengerId = passengerId;
        }
    }

    public class GetPassengerByIdResponse
    {
        public Guid Id { get; set; }
        public string FlightNumber { get; set; }
        public PassengerInfoDto Info { get; set; }
        public string? SeatNumber { get; set; }
        public List<Guid> AffiliatedPassengerIds { get; set; } = new();
    }

    public class PassengerInfoDto
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string SeatType { get; set; }
    }

    public class GetPassengerByIdQueryHandler
    : IRequestHandler<GetPassengerByIdQueryRequest, GetPassengerByIdResponse?>
    {
        private readonly IPassengerRepository _passengerRepository;

        public GetPassengerByIdQueryHandler(IPassengerRepository passengerRepository)
        {
            _passengerRepository = passengerRepository;
        }

        public async Task<GetPassengerByIdResponse?> Handle(GetPassengerByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var passenger = await _passengerRepository.GetByIdAsync(request.PassengerId, cancellationToken);

            if (passenger is null)
                return null;

            return new GetPassengerByIdResponse
            {
                Id = passenger.Id,
                FlightNumber = passenger.FlightNumber.Value,
                Info = new PassengerInfoDto
                {
                    Name = passenger.Info.Name,
                    Age = passenger.Info.Age,
                    Gender = passenger.Info.Gender,
                    Nationality = passenger.Info.Nationality,
                    SeatType = passenger.Info.SeatType.ToString()
                },
                SeatNumber = passenger.SeatNumber?.Value,
                AffiliatedPassengerIds = passenger.AffiliatedPassengerIds.ToList()
            };
        }
    }
}
