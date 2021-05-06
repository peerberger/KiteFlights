using KiteFlightsCommon.DaoInterfaces;
using KiteFlightsCommon.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsDAL.DAOs.PocoDaos
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

		public IList<Airline> GetByCountry(int countryId)
		{
			var parameters = new List<object> { countryId };

			return SpExecuteReader($"sp_{TableName}_get_by_country", parameters);
		}
	}
}
