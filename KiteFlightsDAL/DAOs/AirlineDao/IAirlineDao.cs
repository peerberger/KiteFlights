using KiteFlightsDAL.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsDAL.DAOs.AirlineDao
{
	public interface IAirlineDao : ICrudDao<Airline>
	{
		Airline GetByUsername(string username);
		IList<Airline> GetByCountry(int countryId);
	}
}
