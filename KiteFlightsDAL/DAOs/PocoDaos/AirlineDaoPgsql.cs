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
	public class AirlineDaoPgsql : GenericDaoPgsql<Airline>, IAirlineDao
	{
		public AirlineDaoPgsql()
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

		public int RemoveByCountryId(int countryId)
		{
			var parameters = new List<object> { countryId };

			var removedCountriesCount = SpExecuteScalarWithDefaultReturnValue($"sp_{TableName}_remove_by_country_id", 0, parameters);

			return removedCountriesCount;
		}
	}
}
