using KiteFlightsCommon.DaoInterfaces;
using KiteFlightsCommon.POCOs;
using KiteFlightsDAL;
using KiteFlightsDAL.DAOs.PocoDaos;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsBLL.Tests.CommonFixtures
{
	public class DalFixture
	{
		protected static NpgsqlConnectionPool _connectionPool = NpgsqlConnectionPool.Instance;

		public ICountryDao CountryDao { get; private set; }
		public IAdminDao AdminDao { get; private set; }
		public ICustomerDao CustomerDao { get; private set; }
		public IAirlineDao AirlineDao { get; private set; }
		public IFlightDao FlightDao { get; private set; }
		public ITicketDao TicketDao { get; private set; }


		public DalFixture()
		{
			CountryDao = new CountryDaoPgsql();
			AdminDao = new AdminDaoPgsql();
			CustomerDao = new CustomerDaoPgsql();
			AirlineDao = new AirlineDaoPgsql();
			FlightDao = new FlightDaoPgsql();
			TicketDao = new TicketDaoPgsql();
		}

		public void ClearAndRepopulateDb()
		{
			string sp = @"CALL sp_clear_and_repopulate_db()";

			var connection = _connectionPool.GetConnection();

			using (var cmd = new NpgsqlCommand(sp, connection))
			{
				cmd.ExecuteNonQuery();
			}

			_connectionPool.ReturnConnection(connection);
		}
	}
}
