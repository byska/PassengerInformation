using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PassengerInformation.Domain.ValueObjects
{
    public record FlightNumber
    {
        public string Value { get; }

        public FlightNumber(string value)
        {
            if (!Regex.IsMatch(value, "^[A-Z]{2}[0-9]{4}$"))
                throw new ArgumentException("Flight number formatı AANNNN olmalı (ör. TK1234).");

            Value = value;
        }

        public override string ToString() => Value;
    }

}
