﻿using KiteFlightsCommon.DaoInterfaces;
using KiteFlightsCommon.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsDAL.DAOs.PocoDaos
{
	public class FlightDaoPgsql : GenericDaoPgsql<Flight>, IFlightDao
	{
		public FlightDaoPgsql(string connectionString) : base(connectionString)
		{
		}

		public Dictionary<Flight, int> GetAllFlightsWithVacancies()
		{
			var spResult = SpExecuteReader($"sp_{TableName}_get_all_flights_with_vacancies");

			return spResult.ToDictionary(flight => flight, flight => flight.RemainingTicketsNo);
		}

		public IList<Flight> GetFlightsByOriginCountry(int countryId)
		{
			var parameters = new List<object> { countryId };

			return SpExecuteReader($"sp_{TableName}_get_by_origin_country", parameters);
		}

		public IList<Flight> GetFlightsByDestinationCountry(int countryId)
		{
			var parameters = new List<object> { countryId };

			return SpExecuteReader($"sp_{TableName}_get_by_destination_country", parameters);
		}

		public IList<Flight> GetFlightsByDepatrureTime(DateTime departureTime)
		{
			var parameters = new List<object> { departureTime };

			return SpExecuteReader($"sp_{TableName}_get_by_departure_time", parameters);
		}

		public IList<Flight> GetFlightsByLandingTime(DateTime landingTime)
		{
			var parameters = new List<object> { landingTime };

			return SpExecuteReader($"sp_{TableName}_get_by_landing_time", parameters);
		}

		public IList<Flight> GetFlightsByCustomer(long customerId)
		{
			var parameters = new List<object> { customerId };

			return SpExecuteReader($"sp_{TableName}_get_by_customer", parameters);
		}

	}
}