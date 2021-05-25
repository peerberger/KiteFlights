using KiteFlightsCommon.DaoInterfaces;
using KiteFlightsDAL.DAOs.PocoDaos;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsBLL.Tests
{
	public class DalFixture : IDisposable
	{
		// todo: move conenction string to config file
		protected string connectionString = @"Host=localhost;Username=postgres;Password=admin;Database=kite_flights_tests_db;";
		private NpgsqlConnection connection;

		public ICountryDao CountryDao { get; private set; }
		public IAdminDao AdminDao { get; private set; }
		public ICustomerDao CustomerDao { get; private set; }
		public IAirlineDao AirlineDao { get; private set; }


		public DalFixture()
		{
			connection = new NpgsqlConnection(connectionString);

			CountryDao = new CountryDaoPgsql(connection);
			AdminDao = new AdminDaoPgsql(connection);
			CustomerDao = new CustomerDaoPgsql(connection);
			AirlineDao = new AirlineDaoPgsql(connection);
		}

		public void ClearAndRepopulateDb()
		{
			string sp = @"CALL sp_clear_and_repopulate_db()";

			connection.Open();

			using (var cmd = new NpgsqlCommand(sp, connection))
			{
				cmd.ExecuteNonQuery();
			}

			connection.Close();
		}

		public void Dispose()
		{
			connection.Dispose();
		}
	}
}
