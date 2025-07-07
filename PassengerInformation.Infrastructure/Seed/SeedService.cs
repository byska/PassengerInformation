using PassengerInformation.Domain.Entities;
using PassengerInformation.Domain.Enums;
using PassengerInformation.Domain.ValueObjects;
using PassengerInformation.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassengerInformation.Infrastructure.Seed
{
    public static class SeedService
    {
        public static void Seed(this PassengerDbContext context)
        {
            if (context.Passengers.Any())
                return;

            var passengers = new List<Passenger>();

            var ahmetId = Guid.NewGuid();
            var ayseId = Guid.NewGuid();
            var johnId = Guid.NewGuid();

            var ahmet = new Passenger(
                ahmetId,
                new FlightNumber("TK1234"),
                new PassengerInfo(
                    name: "Ahmet Yılmaz",
                    age: 35,
                    gender: "Male",
                    nationality: "Turkish",
                    seatType: SeatType.Economy
                )
            );
            ahmet.AssignSeat(new SeatNumber("12A"));
            ahmet.UpdateAffiliations(new List<Guid> { ayseId });

            var ayse = new Passenger(
                ayseId,
                new FlightNumber("TK1234"),
                new PassengerInfo(
                    name: "Ayşe Demir",
                    age: 28,
                    gender: "Female",
                    nationality: "Turkish",
                    seatType: SeatType.Business
                )
            );
            ayse.AssignSeat(new SeatNumber("12B"));
            ayse.UpdateAffiliations(new List<Guid> { ahmetId });

            var john = new Passenger(
                johnId,
                new FlightNumber("TK5678"),
                new PassengerInfo(
                    name: "John Doe",
                    age: 40,
                    gender: "Male",
                    nationality: "American",
                    seatType: SeatType.Economy
                )
            );

            john.UpdateAffiliations(new List<Guid>());

            passengers.Add(ahmet);
            passengers.Add(ayse);
            passengers.Add(john);

            context.Passengers.AddRange(passengers);
            context.SaveChanges();
        }
    }
}
