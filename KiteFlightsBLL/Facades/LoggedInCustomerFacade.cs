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
	public class LoggedInCustomerFacade : AnonymousUserFacade, ILoggedInCustomerFacade
	{
		public LoggedInCustomerFacade(NpgsqlConnection connection) : base(connection)
		{
		}

		public IList<Flight> GetAllMyFlights(LoginToken<Customer> token)
		{
			return _flightDao.GetFlightsByCustomer(token.User.Id);
		}

		public Ticket PurchaseTicket(LoginToken<Customer> token, Flight flight)
		{
			return _ticketDao.Add(flight.Id, token.User.Id);
		}

		public void CancelTicket(LoginToken<Customer> token, Ticket ticket)
		{
			if (token.User.Id != ticket.Customer.Id)
			{
				throw new Exception("Ticket does not belong to the token's user (customer).");
			}

			_ticketDao.Remove((int)ticket.Id);
		}
	}
}
