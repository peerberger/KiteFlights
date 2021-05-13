using KiteFlightsBLL.Auth;
using KiteFlightsBLL.Facades;
using KiteFlightsCommon.POCOs;
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

		public LoggedInAdminFacade Facade { get; private set; }

		public LoggedInAdminFacadeFixture()
		{
			ClearAndRepopulateDb();

			Facade = new LoggedInAdminFacade(new NpgsqlConnection(connectionString));
		}

		private void ClearAndRepopulateDb()
		{
			string sp = @"CALL sp_clear_and_repopulate_db()";

			using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
			{
				connection.Open();

				using (var cmd = new NpgsqlCommand(sp, connection))
				{
					cmd.ExecuteNonQuery();
				}
			}
		}

		public void Dispose()
		{
			Facade.Dispose();
		}
	}
}
