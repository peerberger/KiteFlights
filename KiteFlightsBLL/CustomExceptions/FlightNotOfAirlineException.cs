using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsBLL.CustomExceptions
{
	public class FlightNotOfAirlineException : Exception
	{
		public FlightNotOfAirlineException()
		{
		}
		public FlightNotOfAirlineException(string message) : base(message)
		{
		}
		public FlightNotOfAirlineException(string message, Exception innerException) : base(message, innerException)
		{
		}
		public FlightNotOfAirlineException(long flightId, long airlineId)
		{
			Message = $"Flight {flightId} does not belong to Airline {airlineId}.";
		}

		public override string Message { get; }
	}
}
