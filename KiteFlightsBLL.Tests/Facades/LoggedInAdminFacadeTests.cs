using KiteFlightsBLL.Auth;
using KiteFlightsBLL.CustomExceptions;
using KiteFlightsBLL.Tests.CommonFixtures;
using KiteFlightsBLL.Tests.Facades.FacadesFixtures;
using KiteFlightsBLL.Tests.Utilities;
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

namespace KiteFlightsBLL.Tests.Facades
{
	[Collection("DalFixtureCollection")]
	public class LoggedInAdminFacadeTests : IClassFixture<LoggedInAdminFacadeFixture>
	{
		private DalFixture _dalFixture;
		private LoggedInAdminFacadeFixture _facadeFixture;

		private ICountryDao CountryDao { get => _dalFixture.CountryDao; }
		private ICustomerDao CustomerDao { get => _dalFixture.CustomerDao; }
		private IAdminDao AdminDao { get => _dalFixture.AdminDao; }
		private IAirlineDao AirlineDao { get => _dalFixture.AirlineDao; }

		private ILoggedInAdminFacade Facade { get => _facadeFixture.Facade; }

		public LoggedInAdminFacadeTests(
			DalFixture dalFixture,
			LoggedInAdminFacadeFixture facadeFixture)
		{
			_dalFixture = dalFixture;
			_facadeFixture = facadeFixture;
			
			_dalFixture.ClearAndRepopulateDb();
		}

		#region helper methods
		#region GetTokensOfLevels() and its encapsulations 
		public static IEnumerable<object[]> GetTokensOfLevels(params int[] levels)
		{
			foreach (var level in levels)
			{
				yield return new object[] { new LoginToken<Admin>(new Admin { Level = level }) };
			}
		}

		public static IEnumerable<object[]> GetTokensOfLevels_1()
		{
			return GetTokensOfLevels(1);
		}

		public static IEnumerable<object[]> GetTokensOfLevels_1_2()
		{
			return GetTokensOfLevels(1, 2);
		}

		public static IEnumerable<object[]> GetTokensOfLevels_1_2_3()
		{
			return GetTokensOfLevels(1, 2, 3);
		}

		public static IEnumerable<object[]> GetTokensOfLevels_2_3()
		{
			return GetTokensOfLevels(2, 3);
		}

		public static IEnumerable<object[]> GetTokensOfLevels_3()
		{
			return GetTokensOfLevels(3);
		}
		#endregion
		#endregion

		#region level 1 admin
		[Theory]
		[MemberData(nameof(GetTokensOfLevels_1_2_3))]
		public void GetAllCustomers_AllAdminLevels_Success(LoginToken<Admin> token)
		{
			// arrange
			var expected = InitialData.Customers();

			// act
			var actual = Facade.GetAllCustomers(token);

			// assert
			Assert.Equal(expected, actual);
		}

		[Theory]
		[MemberData(nameof(GetTokensOfLevels_1_2_3))]
		public void UpdateCustomerDetails_AllAdminLevels_Success(LoginToken<Admin> token)
		{
			// arrange
			var customer = InitialData.Customers().First();

			customer.FirstName = "Bob";

			// act
			Facade.UpdateCustomerDetails(token, customer);

			// assert
			var expected = customer;
			var actual = CustomerDao.GetById((int)customer.Id);

			Assert.Equal(expected, actual);
		}

		[Theory]
		[MemberData(nameof(GetTokensOfLevels_1_2_3))]
		public void UpdateAirlineDetails_AllAdminLevels_Success(LoginToken<Admin> token)
		{
			// arrange
			var airline = InitialData.Airlines().First();

			airline.Name = "Bob";

			// act
			Facade.UpdateAirlineDetails(token, airline);

			// assert
			var expected = airline;
			var actual = AirlineDao.GetById((int)airline.Id);

			Assert.Equal(expected, actual);
		}
		#endregion

		#region level 2 admin
		[Theory]
		[MemberData(nameof(GetTokensOfLevels_1))]
		public void RemoveCustomer_AdminLevels_1_Failure(LoginToken<Admin> token)
		{
			// arrange
			var customer = InitialData.Customers().First();

			// assert
			Assert.Throws<UnauthorizedAdminException>(() => Facade.RemoveCustomer(token, customer));
		}

		[Theory]
		[MemberData(nameof(GetTokensOfLevels_2_3))]
		public void RemoveCustomer_AdminLevels_2_3_Success(LoginToken<Admin> token)
		{
			// arrange
			var customer = InitialData.Customers().First();

			// act
			Facade.RemoveCustomer(token, customer);

			// assert
			var expected = CustomerDao.GetById((int)customer.Id);
			var actual = default(Customer);

			Assert.Equal(expected, actual);
		}

		[Theory]
		[MemberData(nameof(GetTokensOfLevels_1))]
		public void RemoveAirline_AdminLevels_1_Failure(LoginToken<Admin> token)
		{
			// arrange
			var airline = InitialData.Airlines().First();

			// assert
			Assert.Throws<UnauthorizedAdminException>(() => Facade.RemoveAirline(token, airline));
		}

		[Theory]
		[MemberData(nameof(GetTokensOfLevels_2_3))]
		public void RemoveAirline_AdminLevels_2_3_Success(LoginToken<Admin> token)
		{
			// arrange
			var airline = InitialData.Airlines().First();

			// act
			Facade.RemoveAirline(token, airline);

			// assert
			var expected = AirlineDao.GetById((int)airline.Id);
			var actual = default(Airline);

			Assert.Equal(expected, actual);
		}
		#endregion

		#region level 3 admin
		[Theory]
		[MemberData(nameof(GetTokensOfLevels_1_2))]
		public void UpdateCountryDetails_AdminLevels_1_2_Failure(LoginToken<Admin> token)
		{
			// arrange
			var country = InitialData.Countries().First();

			country.Name = "Narnia";

			// assert
			Assert.Throws<UnauthorizedAdminException>(() => Facade.UpdateCountryDetails(token, country));
		}

		[Theory]
		[MemberData(nameof(GetTokensOfLevels_3))]
		public void UpdateCountryDetails_AdminLevels_3_Success(LoginToken<Admin> token)
		{
			// arrange
			var country = InitialData.Countries().First();

			country.Name = "Narnia";

			// act
			Facade.UpdateCountryDetails(token, country);

			// assert
			var expected = country;
			var actual = CountryDao.GetById(country.Id);

			Assert.Equal(expected, actual);
		}

		[Theory]
		[MemberData(nameof(GetTokensOfLevels_1_2))]
		public void RemoveCountry_AdminLevels_1_2_Failure(LoginToken<Admin> token)
		{
			// arrange
			var country = InitialData.Countries().First();

			// assert
			Assert.Throws<UnauthorizedAdminException>(() => Facade.RemoveCountry(token, country));
		}

		[Theory]
		[MemberData(nameof(GetTokensOfLevels_3))]
		public void RemoveCountry_AdminLevels_3_Success(LoginToken<Admin> token)
		{
			// arrange
			var country = InitialData.Countries().First();

			// act
			Facade.RemoveCountry(token, country);

			// assert
			var expected = CountryDao.GetById(country.Id);
			var actual = default(Country);

			Assert.Equal(expected, actual);
		}

		[Theory]
		[MemberData(nameof(GetTokensOfLevels_1_2))]
		public void UpdateAdminDetails_AdminLevels_1_2_Failure(LoginToken<Admin> token)
		{
			// arrange
			var admin = InitialData.Admins().First();

			admin.FirstName = "Bob";

			// assert
			Assert.Throws<UnauthorizedAdminException>(() => Facade.UpdateAdminDetails(token, admin));
		}

		[Theory]
		[MemberData(nameof(GetTokensOfLevels_3))]
		public void UpdateAdminDetails_AdminLevels_3_ParamAdminLevel3_Failure(LoginToken<Admin> token)
		{
			// arrange
			var admin = InitialData.Admins().Last();

			admin.FirstName = "Bob";

			// act
			Assert.Throws<Exception>(() => Facade.UpdateAdminDetails(token, admin));
		}

		[Theory]
		[MemberData(nameof(GetTokensOfLevels_3))]
		public void UpdateAdminDetails_AdminLevels_3_Success(LoginToken<Admin> token)
		{
			// arrange
			var admin = InitialData.Admins().First();

			admin.FirstName = "Bob";

			// act
			Facade.UpdateAdminDetails(token, admin);

			// assert
			var expected = admin;
			var actual = AdminDao.GetById(admin.Id);

			Assert.Equal(expected, actual);
		}

		[Theory]
		[MemberData(nameof(GetTokensOfLevels_1_2))]
		public void RemoveAdmin_AdminLevels_1_2_Failure(LoginToken<Admin> token)
		{
			// arrange
			var admin = InitialData.Admins().First();

			// assert
			Assert.Throws<UnauthorizedAdminException>(() => Facade.RemoveAdmin(token, admin));
		}

		[Theory]
		[MemberData(nameof(GetTokensOfLevels_3))]
		public void RemoveAdmin_AdminLevels_3_ParamAdminLevel3_Failure(LoginToken<Admin> token)
		{
			// arrange
			var admin = InitialData.Admins().Last();

			// act
			Assert.Throws<Exception>(() => Facade.RemoveAdmin(token, admin));
		}

		[Theory]
		[MemberData(nameof(GetTokensOfLevels_3))]
		public void RemoveAdmin_AdminLevels_3_Success(LoginToken<Admin> token)
		{
			// arrange
			var admin = InitialData.Admins().First();

			// act
			Facade.RemoveAdmin(token, admin);

			// assert
			var expected = AdminDao.GetById(admin.Id);
			var actual = default(Admin);

			Assert.Equal(expected, actual);
		}

		[Theory]
		[MemberData(nameof(GetTokensOfLevels_1_2))]
		public void CreateAdmin_AdminLevels_1_2_Failure(LoginToken<Admin> token)
		{
			// arrange
			var admin = NewPocoGenerator.Admin();

			// assert
			Assert.Throws<UnauthorizedAdminException>(() => Facade.CreateAdmin(token, admin));
		}

		[Theory]
		[MemberData(nameof(GetTokensOfLevels_3))]
		public void CreateAdmin_AdminLevels_3_Success(LoginToken<Admin> token)
		{
			// arrange
			var admin = NewPocoGenerator.Admin();

			// act
			var id = Facade.CreateAdmin(token, admin);

			// assert
			admin.Id = 4;
			admin.User.Id = 10;
			var expected = admin;
			var actual = AdminDao.GetById(id);

			Assert.Equal(expected, actual);
		}

		[Theory]
		[MemberData(nameof(GetTokensOfLevels_1_2))]
		public void CreateCustomer_AdminLevels_1_2_Failure(LoginToken<Admin> token)
		{
			// arrange
			var customer = NewPocoGenerator.Customer();

			// assert
			Assert.Throws<UnauthorizedAdminException>(() => Facade.CreateCustomer(token, customer));
		}

		[Theory]
		[MemberData(nameof(GetTokensOfLevels_3))]
		public void CreateCustomer_AdminLevels_3_Success(LoginToken<Admin> token)
		{
			// arrange
			var customer = NewPocoGenerator.Customer();

			// act
			var id = Facade.CreateCustomer(token, customer);

			// assert
			customer.Id = 4;
			customer.User.Id = 10;
			var expected = customer;
			var actual = CustomerDao.GetById(id);

			Assert.Equal(expected, actual);
		}

		[Theory]
		[MemberData(nameof(GetTokensOfLevels_1_2))]
		public void CreateAirline_AdminLevels_1_2_Failure(LoginToken<Admin> token)
		{
			// arrange
			var airline = NewPocoGenerator.Airline();

			// assert
			Assert.Throws<UnauthorizedAdminException>(() => Facade.CreateAirline(token, airline));
		}

		[Theory]
		[MemberData(nameof(GetTokensOfLevels_3))]
		public void CreateAirline_AdminLevels_3_Success(LoginToken<Admin> token)
		{
			// arrange
			var airline = NewPocoGenerator.Airline();

			// act
			var id = Facade.CreateAirline(token, airline);

			// assert
			airline.Id = 4;
			airline.User.Id = 10;
			var expected = airline;
			var actual = AirlineDao.GetById(id);

			Assert.Equal(expected, actual);
		}
		#endregion
	}
}
