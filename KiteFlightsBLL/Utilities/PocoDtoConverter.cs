using KiteFlightsCommon.DTOs.AnonymousUserFacadeControllerDTOs;
using KiteFlightsCommon.DTOs.CommonDTOs;
using KiteFlightsCommon.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsBLL.Utilities
{
	public static class PocoDtoConverter
	{
		public static CountryDTO CountryPocoToDto(Country country)
		{
			return new CountryDTO
			{
				Id = country.Id,
				Name = country.Name
			};
		}

		public static AirlineDTO AirlinePocoToDto(Airline airline)
		{
			return new AirlineDTO
			{
				Id = airline.Id,
				Name = airline.Name,
				Country = CountryPocoToDto(airline.Country)
			};
		}

		public static IEnumerable<AirlineDTO> AirlineCollectionPocoToDto(IEnumerable<Airline> airlines)
		{
			return airlines.Select(a => AirlinePocoToDto(a));
		}

		public static FlightDTO FlightPocoToDto(Flight flight)
		{
			return new FlightDTO
			{
				Id = flight.Id,
				Airline = AirlinePocoToDto(flight.Airline),
				OriginCountry = CountryPocoToDto(flight.OriginCountry),
				DestinationCountry = CountryPocoToDto(flight.DestinationCountry),
				DepartureTime = flight.DepartureTime,
				LandingTime = flight.LandingTime,
				RemainingTicketsNo = flight.RemainingTicketsNo
			};
		}

		public static IEnumerable<FlightDTO> FlightCollectionPocoToDto(IEnumerable<Flight> flights)
		{
			return flights.Select(f => FlightPocoToDto(f));
		}
	}
}
