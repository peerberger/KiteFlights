using KiteFlightsBLL.Auth;
using KiteFlightsCommon.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsCommon.FacadesInterfaces
{
	public interface ILoggedInAirlineFacade : IAnonymousUserFacade
	{
		IList<Ticket> GetAllMyTickets(LoginToken<Airline> token);
		IList<Flight> GetAllMyFlights(LoginToken<Airline> token);
		int CreateFlight(LoginToken<Airline> token, Flight flight);
		void UpdateFlightDetails(LoginToken<Airline> token, Flight flight);
		void CancelFlight(LoginToken<Airline> token, Flight flight);
		bool ChangeMyPassword(LoginToken<Airline> token, string oldPassword, string newPassword);
		void UpdateMyDetails(LoginToken<Airline> token, Airline airline);
	}
}
