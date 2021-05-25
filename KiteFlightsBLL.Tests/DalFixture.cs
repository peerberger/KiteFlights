using KiteFlightsCommon.DaoInterfaces;
using KiteFlightsCommon.POCOs;
using KiteFlightsDAL.DAOs.PocoDaos;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
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

		#region helper methods
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

		#region GetTableInitialData() and its encapsulations
		public T GetTableInitialData<T>(string tableName)
		{
			var json = File.ReadAllText($@"DbInitialData\{tableName}.json");

			return JsonConvert.DeserializeObject<T>(json);
		}

		public IList<Country> GetCountriesInitialData()
		{
			return GetTableInitialData<IList<Country>>("countries");
		}

		public IList<Admin> GetAdminsInitialData()
		{
			return GetTableInitialData<IList<Admin>>("admins");
		}

		public IList<Customer> GetCustomersInitialData()
		{
			return GetTableInitialData<IList<Customer>>("customers");
		}

		public IList<Airline> GetAirlinesInitialData()
		{
			return GetTableInitialData<IList<Airline>>("airlines");
		}
		#endregion
		#endregion

		public void Dispose()
		{
			connection.Dispose();
		}
	}
}
