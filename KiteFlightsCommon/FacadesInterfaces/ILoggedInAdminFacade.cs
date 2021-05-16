using KiteFlightsBLL.Auth;
using KiteFlightsCommon.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsCommon.FacadesInterfaces
{
	public interface ILoggedInAdminFacade : IAnonymousUserFacade
	{
		// level 1 admin
		IList<Customer> GetAllCustomers(LoginToken<Admin> token);
		void UpdateCustomerDetails(LoginToken<Admin> token, Customer customer);
		void UpdateAirlineDetails(LoginToken<Admin> token, Airline airline);

		// level 2 admin
		void RemoveCustomer(LoginToken<Admin> token, Customer customer);
		void RemoveAirline(LoginToken<Admin> token, Airline airline);

		// level 3 admin
		void UpdateCountryDetails(LoginToken<Admin> token, Country country);
		void RemoveCountry(LoginToken<Admin> token, Country country);
		void UpdateAdminDetails(LoginToken<Admin> token, Admin admin);
		void RemoveAdmin(LoginToken<Admin> token, Admin admin);
		int CreateAdmin(LoginToken<Admin> token, Admin admin);
		int CreateCustomer(LoginToken<Admin> token, Customer customer);
		int CreateAirline(LoginToken<Admin> token, Airline airline);
	}
}
