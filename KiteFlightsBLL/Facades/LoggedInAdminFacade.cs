using KiteFlightsBLL.Auth;
using KiteFlightsBLL.CustomExceptions;
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
				throw new UnauthorizedAdminException(level);
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
			VerifyAdminLevel1(token);

			return _customerDao.GetAll();
		}

		public void UpdateCustomerDetails(LoginToken<Admin> token, Customer customer)
		{
			VerifyAdminLevel1(token);

			_customerDao.Update(customer);
		}

		public void UpdateAirlineDetails(LoginToken<Admin> token, Airline airline)
		{
			VerifyAdminLevel1(token);

			_airlineDao.Update(airline);
		}


		// level 2 admin
		public void RemoveCustomer(LoginToken<Admin> token, Customer customer)
		{
			VerifyAdminLevel2(token);

			_ticketDao.RemoveByCustomerId(customer.Id);

			_customerDao.Remove((int)customer.Id);
		}

		public void RemoveAirline(LoginToken<Admin> token, Airline airline)
		{
			VerifyAdminLevel2(token);

			_ticketDao.RemoveByAirlineId(airline.Id);
			
			_flightDao.RemoveByAirlineId(airline.Id);

			_airlineDao.Remove((int)airline.Id);
		}


		// level 3 admin
		public void UpdateCountryDetails(LoginToken<Admin> token, Country country)
		{
			VerifyAdminLevel3(token);

			_countryDao.Update(country);
		}

		public void RemoveCountry(LoginToken<Admin> token, Country country)
		{
			VerifyAdminLevel3(token);

			_ticketDao.RemoveByCountryId(country.Id);

			_flightDao.RemoveByCountryId(country.Id);

			_airlineDao.RemoveByCountryId(country.Id);

			_countryDao.Remove(country.Id);
		}

		public void UpdateAdminDetails(LoginToken<Admin> token, Admin admin)
		{
			VerifyAdminLevel3(token);

			if (admin.Level < token.User.Level)
			{
				_adminDao.Update(admin);
			}
			else
			{
				throw new Exception($"Level {token.User.Level} admins can only alter admins of a lower level");
			}
		}

		public void RemoveAdmin(LoginToken<Admin> token, Admin admin)
		{
			VerifyAdminLevel3(token);

			if (admin.Level < token.User.Level)
			{
				_adminDao.Remove(admin.Id);
			}
			else
			{
				throw new Exception($"Level {token.User.Level} admins can only alter admins of a lower level");
			}
		}

		public int CreateAdmin(LoginToken<Admin> token, Admin admin)
		{
			VerifyAdminLevel3(token);

			return _adminDao.Add(admin);
		}

		public int CreateCustomer(LoginToken<Admin> token, Customer customer)
		{
			VerifyAdminLevel3(token);

			return _customerDao.Add(customer);
		}

		public int CreateAirline(LoginToken<Admin> token, Airline airline)
		{
			VerifyAdminLevel3(token);

			return _airlineDao.Add(airline);
		}
	}
}
