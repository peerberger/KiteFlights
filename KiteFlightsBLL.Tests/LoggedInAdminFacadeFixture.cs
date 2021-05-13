using KiteFlightsBLL.Auth;
using KiteFlightsBLL.Facades;
using KiteFlightsCommon.DaoInterfaces;
using KiteFlightsCommon.FacadesInterfaces;
using KiteFlightsCommon.POCOs;
using KiteFlightsDAL.DAOs.PocoDaos;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsBLL.Tests
{
	public class LoggedInAdminFacadeFixture : IDisposable
	{
		// todo: move conenction string to config file
		string connectionString = @"Host=localhost;Username=postgres;Password=admin;Database=kite_flights_tests_db;";

		public NpgsqlConnection Connection { get; private set; }
		public ILoggedInAdminFacade Facade { get; private set; }
		public ICustomerDao CustomerDao { get; private set; }
		public IAirlineDao AirlineDao { get; private set; }


		public LoggedInAdminFacadeFixture()
		{
			Connection = new NpgsqlConnection(connectionString);

			Facade = new LoggedInAdminFacade(Connection);
			CustomerDao = new CustomerDaoPgsql(Connection);
			AirlineDao = new AirlineDaoPgsql(Connection);
		}

		public void Dispose()
		{
			Connection.Dispose();
		}
	}
}
