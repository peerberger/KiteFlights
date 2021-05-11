using KiteFlightsBLL.Auth;
using KiteFlightsCommon.FacadesInterfaces;
using KiteFlightsCommon.POCOs;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsBLL.Facades
{
	public class LoggedInAdminFacade : AnonymousUserFacade, ILoggedInAdminFacade
	{
		public LoggedInAdminFacade(NpgsqlConnection connection) : base(connection)
		{
		}

		private void CheckAdminLevel(LoginToken<Admin> token, int level)
		{
			if (!(token.User.Level >= level))
			{
				throw new Exception($"Admins below level {level} are unauthorized to perform this action");
			}
		}

		private void VerifyAdminLevel1(LoginToken<Admin> token)
		{
			CheckAdminLevel(token, 1);
		}

		private void VerifyAdminLevel2(LoginToken<Admin> token)
		{
			CheckAdminLevel(token, 2);
		}

		private void VerifyAdminLevel3(LoginToken<Admin> token)
		{
			CheckAdminLevel(token, 3);
		}

		// level 1 admin
		public IList<Customer> GetAllCustomers(LoginToken<Admin> token)
		{
			throw new NotImplementedException();
		}

		public void UpdateCustomerDetails(LoginToken<Admin> token, Customer customer)
		{
			throw new NotImplementedException();
		}

		public void UpdateAirlineDetails(LoginToken<Admin> token, Airline airline)
		{
			throw new NotImplementedException();
		}


		// level 2 admin
		public void RemoveCustomer(LoginToken<Admin> token, Customer customer)
		{
			throw new NotImplementedException();
		}

		public void RemoveAirline(LoginToken<Admin> token, Airline airline)
		{
			throw new NotImplementedException();
		}


		// level 3 admin
		public void UpdateCountryDetails(LoginToken<Admin> token, Country country)
		{
			throw new NotImplementedException();
		}

		public void RemoveCountry(LoginToken<Admin> token, Country country)
		{
			throw new NotImplementedException();
		}

		public void UpdateAdminDetails(LoginToken<Admin> token, Admin admin)
		{
			throw new NotImplementedException();
		}

		public void RemoveAdmin(LoginToken<Admin> token, Admin admin)
		{
			throw new NotImplementedException();
		}

		public void CreateAdmin(LoginToken<Admin> token, Admin admin)
		{
			throw new NotImplementedException();
		}

		public void CreateCustomer(LoginToken<Admin> token, Customer customer)
		{
			throw new NotImplementedException();
		}

		public void CreateAirline(LoginToken<Admin> token, Airline airline)
		{
			throw new NotImplementedException();
		}
	}
}
