using KiteFlightsDAL.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsDAL.DAOs.AirlineDao
{
	public class AirlineDaoPgsql : GenericDaoPgsql<Airline>, IAirlineDao
	{
		public AirlineDaoPgsql(string connectionString) : base(connectionString)
		{
		}

		public Airline GetByUsername(string username)
		{
			var parameters = new List<object> { username };

			return SpExecuteReaderReturningSingleRecord($"sp_{TableName}_get_by_username", parameters);
		}
	}
}
