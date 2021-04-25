using KiteFlightsDAL.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsDAL.DAOs.FlightDao
{
	public class FlightDaoPgsql : GenericDaoPgsql<Flight>, IFlightDao
	{
		public FlightDaoPgsql(string connectionString) : base(connectionString)
		{
		}
	}
}
