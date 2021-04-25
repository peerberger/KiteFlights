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
	}
}
