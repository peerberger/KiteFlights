using KiteFlightsBLL.Auth;
using KiteFlightsCommon.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsCommon.FacadesInterfaces
{
	public interface ILoggedInCustomerFacade : IAnonymousUserFacade
	{
		IList<Flight> GetAllMyFlights(LoginToken<Customer> token);
		Ticket PurchaseTicket(LoginToken<Customer> token, Flight flight);
		void CancelTicket(LoginToken<Customer> token, Ticket ticket);
	}
}
