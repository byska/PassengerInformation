using MediatR;
using PassengerInformation.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassengerInformation.Application.UseCases.PassengersUseCases.Commands
{
    public class DeletePassengerCommand : IRequest<DeletePassengerCommandResponse>
    {
        public Guid PassengerId { get; }

        public DeletePassengerCommand(Guid passengerId)
        {
            PassengerId = passengerId;
        }
    }

    public class DeletePassengerCommandResponse
    {
        public bool IsDeleted { get; set; }
        public string? Message { get; set; }

        public DeletePassengerCommandResponse(bool ısDeleted, string message)
        {
            IsDeleted = ısDeleted;
            Message = message;
        }
    }


    public class DeletePassengerCommandHandler
 : IRequestHandler<DeletePassengerCommand, DeletePassengerCommandResponse>
    {
        private readonly IPassengerRepository _passengerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeletePassengerCommandHandler(
            IPassengerRepository passengerRepository,
            IUnitOfWork unitOfWork)
        {
            _passengerRepository = passengerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeletePassengerCommandResponse> Handle(DeletePassengerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var passenger = await _passengerRepository.GetByIdAsync(request.PassengerId, cancellationToken);

                if (passenger is null)
                    throw new InvalidOperationException("Passenger not found.");


                if (passenger.Info.Age > 2)
                {
                    var affiliatedToOthers =
                        await _passengerRepository.GetByFlightNumberAsync(passenger.FlightNumber.Value, cancellationToken);

                    var stillLinked = affiliatedToOthers.Any(p =>
                        p.AffiliatedPassengerIds.Contains(passenger.Id));

                    if (stillLinked)
                        throw new InvalidOperationException("This passenger is affiliated with others. Remove affiliations first.");
                }
                passenger.DeleteDate = DateTime.UtcNow;
                passenger.IsActive = false; 
                
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new DeletePassengerCommandResponse(true,null);
            }
            catch (Exception ex)
            {
                return new DeletePassengerCommandResponse(false,ex.Message);

            }
           
        }
    }
}
