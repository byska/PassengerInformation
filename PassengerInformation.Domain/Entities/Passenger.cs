using PassengerInformation.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassengerInformation.Domain.Entities
{
    public class Passenger : EntityBase
    {
        public FlightNumber FlightNumber { get; private set; }
        public PassengerInfo Info { get; private set; }
        public SeatNumber? SeatNumber { get; private set; }
        public List<Guid> AffiliatedPassengerIds { get; private set; } = new();

        protected Passenger() { }

        public Passenger(Guid id, FlightNumber flightNumber, PassengerInfo info)
        {
            Id = id != Guid.Empty ? id : Guid.NewGuid();
            FlightNumber = flightNumber ?? throw new ArgumentNullException(nameof(flightNumber));
            Info = info ?? throw new ArgumentNullException(nameof(info));
        }

        public void AssignSeat(SeatNumber seat)
        {
            SeatNumber = seat ?? throw new ArgumentNullException(nameof(seat));
        }

        public void UpdateAffiliations(List<Guid> ids)
        {
            if (ids.Count > 2)
                throw new InvalidOperationException("En fazla 2 bağlantılı yolcu olabilir.");

            AffiliatedPassengerIds = ids;
        }
        public void UpdateFlightNumber(FlightNumber flightNumber)
        {
            FlightNumber = flightNumber ?? throw new ArgumentNullException(nameof(flightNumber));
        }

        public void UpdateInfo(PassengerInfo info)
        {
            Info = info ?? throw new ArgumentNullException(nameof(info));
        }

    }

}
