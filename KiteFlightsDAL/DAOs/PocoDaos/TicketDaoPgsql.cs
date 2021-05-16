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

			return (int)SpExecuteScalar($"sp_{TableName}_remove_by_customer_id", parameters);
		}

		public int RemoveByAirlineId(long airlineId)
		{
			var parameters = new List<object> { airlineId };

			return (int)SpExecuteScalar($"sp_{TableName}_remove_by_airline_id", parameters);
		}

		public int RemoveByCountryId(int countryId)
		{
			var parameters = new List<object> { countryId };

			return (int)SpExecuteScalar($"sp_{TableName}_remove_by_country_id", parameters);
		}
	}
}
