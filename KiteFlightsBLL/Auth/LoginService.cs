using KiteFlightsBLL.CustomExceptions;
using KiteFlightsBLL.Facades;
using KiteFlightsCommon.DaoInterfaces;
using KiteFlightsCommon.POCOs;
using KiteFlightsCommon.POCOs.Interfaces;
using KiteFlightsDAL.DAOs.PocoDaos;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsBLL.Auth
{
	public class LoginService : ILoginService, IDisposable
	{
		// todo: remember to maybe edit this when adding the use of connection pool (keeping this might overload the connection pool)
		private string connectionString = @"Host=localhost;Username=postgres;Password=admin;Database=kite_flights_tests_db;";

		private readonly NpgsqlConnection _connection;

		private IUserDao _userDao;
		private ICustomerDao _customerDao;
		private IAirlineDao _airlineDao;
		private IAdminDao _adminDao;

		public LoginService(NpgsqlConnection connection)
		{
			_connection = connection;

			_userDao = new UserDaoPgsql(_connection);
			_customerDao = new CustomerDaoPgsql(_connection);
			_airlineDao = new AirlineDaoPgsql(_connection);
			_adminDao = new AdminDaoPgsql(_connection);
		}

		/// <summary>
		/// remember to dispose of the facade's connection when you're done 
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <param name="token"></param>
		/// <param name="facade"></param>
		/// <returns></returns>
		public bool TryLogin(string username, string password, out ILoginToken token, out FacadeBase facade)
		{
			// todo: add logging!!! (details about logging in the instructions)

			var result = false;

			if (username == "admin" && password == "9999")
			{
				result = GetTokenAndFacadeForMainAdmin(out token, out facade);
			}
			else
			{
				// verifying credentails and return the user role in order to instantiate the right token and facade
				var userRole = _userDao.GetUserRole(username, password);

				result = GetTokenAndFacadeByUser(userRole, username, out token, out facade);
			}

			return result;
		}

		private bool GetTokenAndFacadeForMainAdmin(out ILoginToken token, out FacadeBase facade)
		{
			Admin admin = new Admin { Level = 4 };

			token = new LoginToken<Admin>(admin);
			facade = new LoggedInAdminFacade(new NpgsqlConnection(connectionString));

			return true;
		}

		private bool GetTokenAndFacadeByUser(int userRole, string username, out ILoginToken token, out FacadeBase facade)
		{
			token = null;
			facade = null;

			var result = false;
			IUser user;

			switch (userRole)
			{
				case 1:
					user = _customerDao.GetByUsername(username);

					token = new LoginToken<Customer>(user as Customer);
					facade = new LoggedInCustomerFacade(new NpgsqlConnection(connectionString));

					result = true;
					break;

				case 2:
					user = _airlineDao.GetByUsername(username);

					token = new LoginToken<Airline>(user as Airline);
					facade = new LoggedInAirlineFacade(new NpgsqlConnection(connectionString));

					result = true;
					break;

				case 3:
					user = _adminDao.GetByUsername(username);

					token = new LoginToken<Admin>(user as Admin);
					facade = new LoggedInAdminFacade(new NpgsqlConnection(connectionString));

					result = true;
					break;

				default:
					// todo: log failure
					break;
			}

			return result;
		}

		// todo: remember to maybe edit this when adding the use of connection pool
		public void Dispose()
		{
			_connection.Dispose();
		}
	}
}
