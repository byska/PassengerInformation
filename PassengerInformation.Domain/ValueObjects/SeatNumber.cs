using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PassengerInformation.Domain.ValueObjects
{
    public record SeatNumber
    {
        public string Value { get; }

        public SeatNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(Value))
                throw new ArgumentException("Koltuk numarası boş olamaz.");

            Value = value;
        }

        public override string ToString() => Value;
    }
}
