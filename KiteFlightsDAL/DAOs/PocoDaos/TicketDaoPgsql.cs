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
	public class TicketDaoPgsql : GenericDaoPgsql<Ticket>, ITicketDao
	{
		public TicketDaoPgsql(NpgsqlConnection connection) : base(connection)
		{
		}

		public int RemoveByCustomerId(long customerId)
		{
			var parameters = new List<object> { customerId };

			var spResult = SpExecuteScalar($"sp_{TableName}_remove_by_customer_id", parameters);

			var removedTicketsCount = CheckIfSpResultNullAndReturnValue(spResult, 0);

			return removedTicketsCount;
		}

		public int RemoveByAirlineId(long airlineId)
		{
			var parameters = new List<object> { airlineId };

			var spResult = SpExecuteScalar($"sp_{TableName}_remove_by_airline_id", parameters);

			var removedTicketsCount = CheckIfSpResultNullAndReturnValue(spResult, 0);

			return removedTicketsCount;
		}

		public int RemoveByCountryId(int countryId)
		{
			var parameters = new List<object> { countryId };

			var spResult = SpExecuteScalar($"sp_{TableName}_remove_by_country_id", parameters);

			var removedTicketsCount = CheckIfSpResultNullAndReturnValue(spResult, 0);

			return removedTicketsCount;
		}

		public IList<Ticket> GetByAirlineId(long airlineId)
		{
			var parameters = new List<object> { airlineId };

			return SpExecuteReader($"sp_{TableName}_get_by_airline_id", parameters);
		}

		public int RemoveByFlightId(long flightId)
		{
			var parameters = new List<object> { flightId };

			var spResult = SpExecuteScalar($"sp_{TableName}_remove_by_flight_id", parameters);

			var removedTicketsCount = CheckIfSpResultNullAndReturnValue(spResult, 0);

			return removedTicketsCount;
		}

		public Ticket Add(long flightId, long customerId)
		{
			var parameters = new List<object> { flightId, customerId };

			return SpExecuteReaderReturningSingleRecord($"sp_{TableName}_add", parameters);
		}
	}
}
