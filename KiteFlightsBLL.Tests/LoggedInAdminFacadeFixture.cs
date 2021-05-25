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
		private string connectionString = @"Host=localhost;Username=postgres;Password=admin;Database=kite_flights_tests_db;";
		private NpgsqlConnection connection;

		public ILoggedInAdminFacade Facade { get; private set; }


		public LoggedInAdminFacadeFixture()
		{
			connection = new NpgsqlConnection(connectionString);

			Facade = new LoggedInAdminFacade(connection);
		}

		public void Dispose()
		{
			connection.Dispose();
		}
	}
}
