using KiteFlightsCommon.DaoInterfaces;
using KiteFlightsCommon.POCOs;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsDAL.DAOs.PocoDaos
{
	public class FlightDaoPgsql : GenericDaoPgsql<Flight>, IFlightDao
	{
		public FlightDaoPgsql(NpgsqlConnection connection) : base(connection)
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

		public int RemoveByAirlineId(long airlineId)
		{
			var parameters = new List<object> { airlineId };

			var spResult = SpExecuteScalar($"sp_{TableName}_remove_by_airline_id", parameters);
			
			var removedFlightsCount = CheckIfSpResultNullAndReturnValue(spResult, 0);

			return removedFlightsCount;
		}

		public int RemoveByCountryId(int countryId)
		{
			var parameters = new List<object> { countryId };

			var spResult = SpExecuteScalar($"sp_{TableName}_remove_by_country_id", parameters);

			var removedFlightsCount = CheckIfSpResultNullAndReturnValue(spResult, 0);

			return removedFlightsCount;
		}

		public IList<Flight> GetByAirlineId(long airlineId)
		{
			var parameters = new List<object> { airlineId };

			return SpExecuteReader($"sp_{TableName}_get_by_airline_id", parameters);
		}
	}
}
