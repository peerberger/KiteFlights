using KiteFlightsCommon.DaoInterfaces;
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
	}
}
