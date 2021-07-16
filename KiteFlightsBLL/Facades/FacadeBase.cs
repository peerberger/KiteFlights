using KiteFlightsCommon.DaoInterfaces;
using KiteFlightsDAL.DAOs.PocoDaos;
using Npgsql;
using System;

namespace KiteFlightsBLL.Facades
{
	public abstract class FacadeBase
	{
		protected ICountryDao _countryDao;
		protected IUserDao _userDao;
		protected IAdminDao _adminDao;
		protected ICustomerDao _customerDao;
		protected IAirlineDao _airlineDao;
		protected IFlightDao _flightDao;
		protected ITicketDao _ticketDao;

		public FacadeBase()
		{
			_countryDao = new CountryDaoPgsql();
			_userDao = new UserDaoPgsql();
			_adminDao = new AdminDaoPgsql();
			_customerDao = new CustomerDaoPgsql();
			_airlineDao = new AirlineDaoPgsql();
			_flightDao = new FlightDaoPgsql();
			_ticketDao = new TicketDaoPgsql();
		}
	}
}
