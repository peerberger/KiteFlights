using KiteFlightsBLL.Auth;
using KiteFlightsBLL.Facades;
using KiteFlightsCommon.POCOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Xunit.Extensions;

namespace KiteFlightsBLL.Tests
{
	public class LoggedInAdminFacadeTests : IClassFixture<LoggedInAdminFacadeFixture>
	{
		private LoggedInAdminFacadeFixture _fixture;

		private LoggedInAdminFacade Facade { get => _fixture.Facade; }
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
		}


		[Theory]
		[MemberData(nameof(Tokens))]
		public void GetAllCustomers_AllAdminLevels_Success(LoginToken<Admin> token)
		{
			// arrange
			var expected = JsonConvert.DeserializeObject<IList<Customer>>(File.ReadAllText(@"DbInitialData\customers.json"));

			// act
			var actual = Facade.GetAllCustomers(token);

			// assert
			Assert.Equal(expected, actual);
		}
	}
}
