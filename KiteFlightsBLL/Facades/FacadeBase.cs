using KiteFlightsCommon.DaoInterfaces;
using KiteFlightsDAL.DAOs.PocoDaos;
using Npgsql;
using System;

namespace KiteFlightsBLL.Facades
{
	public abstract class FacadeBase : IDisposable
	{
		private readonly NpgsqlConnection _connection;

		protected ICountryDao _countryDao;
		protected IUserDao _userDao;
		protected IAdminDao _adminDao;
		protected ICustomerDao _customerDao;
		protected IAirlineDao _airlineDao;
		protected IFlightDao _flightDao;
		protected ITicketDao _ticketDao;

		public FacadeBase(NpgsqlConnection connection)
		{
			_connection = connection;

			_countryDao = new CountryDaoPgsql(_connection);
			_userDao = new UserDaoPgsql(_connection);
			_adminDao = new AdminDaoPgsql(_connection);
			_customerDao = new CustomerDaoPgsql(_connection);
			_airlineDao = new AirlineDaoPgsql(_connection);
			_flightDao = new FlightDaoPgsql(_connection);
			_ticketDao = new TicketDaoPgsql(_connection);
		}

		public void Dispose()
		{
			_connection.Dispose();
		}
	}
}
