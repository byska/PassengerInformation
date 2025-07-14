using MediatR;
using PassengerInformation.Application.Abstractions;
using PassengerInformation.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassengerInformation.Application.UseCases.PassengersUseCases.Commands
{
    public class AssignSeatCommand : IRequest<AssignSeatCommandResponse>
    {
        public Guid PassengerId { get; set; }
        public string SeatNumber { get; set; }
    }
    public class AssignSeatCommandResponse
    {
        public AssignSeatCommandResponse(bool ısAssigned, string message)
        {
            IsAssigned = ısAssigned;
            Message = message;
        }

        public bool IsAssigned { get; set; }
        public string? Message { get; set; }

    }
    public class AssignSeatCommandHandler
    : IRequestHandler<AssignSeatCommand, AssignSeatCommandResponse>
    {
        private readonly IPassengerRepository _passengerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssignSeatCommandHandler(
            IPassengerRepository passengerRepository,
            IUnitOfWork unitOfWork)
        {
            _passengerRepository = passengerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AssignSeatCommandResponse> Handle(AssignSeatCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var passenger = await _passengerRepository.GetByIdAsync(request.PassengerId, cancellationToken);

                if (passenger is null)
                    throw new InvalidOperationException("Passenger not found.");

                if (passenger.Info.Age <= 2)
                    throw new InvalidOperationException("Infant passengers (0–2) cannot have a seat.");

                passenger.AssignSeat(new SeatNumber(request.SeatNumber));

                _passengerRepository.Update(passenger);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new AssignSeatCommandResponse(true,null);
            }
            catch (Exception ex)
            {
                return new AssignSeatCommandResponse(false, ex.Message);
            }
            
        }
    }
}
