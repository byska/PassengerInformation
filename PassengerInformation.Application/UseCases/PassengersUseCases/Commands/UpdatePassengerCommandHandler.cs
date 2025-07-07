using MediatR;
using PassengerInformation.Application.Abstractions;
using PassengerInformation.Domain.Enums;
using PassengerInformation.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassengerInformation.Application.UseCases.PassengersUseCases.Commands
{
    public class UpdatePassengerCommand : IRequest<UpdatePassengerCommandResponse>
    {
        public Guid Id { get; set; }

        public string FlightNumber { get; set; }

        public PassengerInfoDto PassengerInfo { get; set; }

        public string? SeatNumber { get; set; }

        public List<Guid>? AffiliatedPassengerIds { get; set; } = new();
    }

    public class UpdatePassengerCommandResponse
    {
        public bool IsUpdated { get; set; }

        public UpdatePassengerCommandResponse(bool ısUpdated)
        {
            IsUpdated = ısUpdated;
        }
    }

    public class UpdatePassengerCommandHandler
    : IRequestHandler<UpdatePassengerCommand, UpdatePassengerCommandResponse>
    {
        private readonly IPassengerRepository _passengerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePassengerCommandHandler(
            IPassengerRepository passengerRepository,
            IUnitOfWork unitOfWork)
        {
            _passengerRepository = passengerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdatePassengerCommandResponse> Handle(UpdatePassengerCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var passenger = await _passengerRepository.GetByIdAsync(request.Id, cancellationToken);

                if (passenger is null)
                    throw new InvalidOperationException("Passenger not found.");

                passenger.UpdateFlightNumber(new FlightNumber(request.FlightNumber));

                var updatedInfo = new PassengerInfo(
                    request.PassengerInfo.Name,
                    request.PassengerInfo.Age,
                    request.PassengerInfo.Gender,
                    request.PassengerInfo.Nationality,
                    Enum.Parse<SeatType>(request.PassengerInfo.SeatType, true)
                );

                passenger.UpdateInfo(updatedInfo);

                if (updatedInfo.Age <= 2)
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
                    passenger.UpdateAffiliations(request.AffiliatedPassengerIds);

                _passengerRepository.Update(passenger);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new UpdatePassengerCommandResponse(true);
            }
            catch (Exception)
            {
                return new UpdatePassengerCommandResponse(false);

                throw;
            }
        }
    }
}
