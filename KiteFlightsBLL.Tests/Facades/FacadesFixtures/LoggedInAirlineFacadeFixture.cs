using KiteFlightsBLL.Facades;
using KiteFlightsCommon.FacadesInterfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsBLL.Tests.Facades.FacadesFixtures
{
	public class LoggedInAirlineFacadeFixture : IDisposable
	{
		// todo: move conenction string to config file
		private string connectionString = @"Host=localhost;Username=postgres;Password=admin;Database=kite_flights_tests_db;";
		private NpgsqlConnection connection;

		public ILoggedInAirlineFacade Facade { get; private set; }


		public LoggedInAirlineFacadeFixture()
		{
			connection = new NpgsqlConnection(connectionString);

			Facade = new LoggedInAirlineFacade(connection);
		}

		public void Dispose()
		{
			connection.Dispose();
		}
	}
}
