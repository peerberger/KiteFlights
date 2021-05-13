using KiteFlightsBLL.Auth;
using KiteFlightsBLL.Facades;
using KiteFlightsCommon.DaoInterfaces;
using KiteFlightsCommon.FacadesInterfaces;
using KiteFlightsCommon.POCOs;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace KiteFlightsBLL.Tests
{
	public class LoggedInAdminFacadeTests : IClassFixture<LoggedInAdminFacadeFixture>
	{
		private LoggedInAdminFacadeFixture _fixture;

		private NpgsqlConnection Connection { get => _fixture.Connection; }
		private ILoggedInAdminFacade Facade { get => _fixture.Facade; }
		private ICustomerDao CustomerDao { get => _fixture.CustomerDao; }
		private IAirlineDao AirlineDao { get => _fixture.AirlineDao; }
		public static IEnumerable<object[]> Tokens
		{
			get
			{
				return new[]
				{
					new object[] { new LoginToken<Admin>(new Admin { Level = 1 }) },
					new object[] { new LoginToken<Admin>(new Admin { Level = 2 }) },
					new object[] { new LoginToken<Admin>(new Admin { Level = 3 }) }
				};
			}
		}

		public LoggedInAdminFacadeTests(LoggedInAdminFacadeFixture fixture)
		{
			_fixture = fixture;
			ClearAndRepopulateDb();
		}

		#region helper methods
		private void ClearAndRepopulateDb()
		{
			string sp = @"CALL sp_clear_and_repopulate_db()";

			Connection.Open();

			using (var cmd = new NpgsqlCommand(sp, Connection))
			{
				cmd.ExecuteNonQuery();
			}

			Connection.Close();
		}

		private T GetTableInitialData<T>(string tableName)
		{
			var json = File.ReadAllText($@"DbInitialData\{tableName}.json");

			return JsonConvert.DeserializeObject<T>(json);
		}

		private IList<Customer> GetCustomersInitialData()
		{
			return GetTableInitialData<IList<Customer>>("customers");
		}

		private IList<Airline> GetAirlinesInitialData()
		{
			return GetTableInitialData<IList<Airline>>("airlines");
		}
		#endregion

		[Theory]
		[MemberData(nameof(Tokens))]
		public void GetAllCustomers_AllAdminLevels_Success(LoginToken<Admin> token)
		{
			// arrange
			var expected = GetCustomersInitialData();

			// act
			var actual = Facade.GetAllCustomers(token);

			// assert
			Assert.Equal(expected, actual);
		}

		[Theory]
		[MemberData(nameof(Tokens))]
		public void UpdateCustomerDetails_AllAdminLevels_Success(LoginToken<Admin> token)
		{
			// arrange
			var customer = GetCustomersInitialData().First();

			customer.FirstName = "Bob";

			// act
			Facade.UpdateCustomerDetails(token, customer);

			// assert
			var expected = customer;
			var actual = CustomerDao.GetById((int)customer.Id);

			Assert.Equal(expected, actual);
		}

		[Theory]
		[MemberData(nameof(Tokens))]
		public void UpdateAirlineDetails_AllAdminLevels_Success(LoginToken<Admin> token)
		{
			// arrange
			var airline = GetAirlinesInitialData().First();

			airline.Name = "Bob";

			// act
			Facade.UpdateAirlineDetails(token, airline);

			// assert
			var expected = airline;
			var actual = AirlineDao.GetById((int)airline.Id);

			Assert.Equal(expected, actual);
		}
	}
}
