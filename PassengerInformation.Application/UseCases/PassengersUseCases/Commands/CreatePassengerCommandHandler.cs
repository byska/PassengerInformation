using MediatR;
using PassengerInformation.Application.Abstractions;
using PassengerInformation.Domain.Entities;
using PassengerInformation.Domain.Enums;
using PassengerInformation.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassengerInformation.Application.UseCases.PassengersUseCases.Commands
{
    public class CreatePassengerCommand : IRequest<CreatePassengerResponse>
    {
        public string FlightNumber { get; set; }

        public PassengerInfoDto PassengerInfo { get; set; }

        public string? SeatNumber { get; set; }

        public List<Guid>? AffiliatedPassengerIds { get; set; } = new();
    }

    public class PassengerInfoDto
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string SeatType { get; set; }
    }

    public class CreatePassengerResponse
    {
        public Guid Id { get; set; }
    }
    public class CreatePassengerCommandHandler
    : IRequestHandler<CreatePassengerCommand, CreatePassengerResponse>
    {
        private readonly IPassengerRepository _passengerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePassengerCommandHandler(
            IPassengerRepository passengerRepository,
            IUnitOfWork unitOfWork)
        {
            _passengerRepository = passengerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreatePassengerResponse> Handle(CreatePassengerCommand request, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();

            var passengerInfo = new PassengerInfo(
                name: request.PassengerInfo.Name,
                age: request.PassengerInfo.Age,
                gender: request.PassengerInfo.Gender,
                nationality: request.PassengerInfo.Nationality,
                seatType: Enum.Parse<SeatType>(request.PassengerInfo.SeatType, true)
            );

            var flightNumber = new FlightNumber(request.FlightNumber);

            var passenger = new Passenger(id, flightNumber, passengerInfo);

            if (passengerInfo.Age <= 2)
            {
                if (!string.IsNullOrWhiteSpace(request.SeatNumber))
                    throw new InvalidOperationException("Infant (0–2) passengers cannot have a seat.");

                if (request.AffiliatedPassengerIds == null || !request.AffiliatedPassengerIds.Any())
                    throw new InvalidOperationException("Infant passengers must have a parent passenger id.");
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(request.SeatNumber))
                    passenger.AssignSeat(new SeatNumber(request.SeatNumber));
            }

            if (request.AffiliatedPassengerIds?.Any() == true)
            {
                passenger.UpdateAffiliations(request.AffiliatedPassengerIds);
            }

            await _passengerRepository.AddAsync(passenger, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreatePassengerResponse
            {
                Id = passenger.Id
            };
        }
    }
}
