using PassengerInformation.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassengerInformation.Domain.ValueObjects
{
    public record PassengerInfo
    {
        public string Name { get; }
        public int Age { get; }
        public string Gender { get; }
        public string Nationality { get; }
        public SeatType SeatType { get; }

        public PassengerInfo(string name, int age, string gender, string nationality, SeatType seatType)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name boş olamaz.");
            if (age <= 0)
                throw new ArgumentException("Age pozitif olmalı.");
            if (string.IsNullOrWhiteSpace(gender))
                throw new ArgumentException("Gender boş olamaz.");
            if (string.IsNullOrWhiteSpace(nationality))
                throw new ArgumentException("Nationality boş olamaz.");

            Name = name;
            Age = age;
            Gender = gender;
            Nationality = nationality;
            SeatType = seatType;
        }
    }

}
