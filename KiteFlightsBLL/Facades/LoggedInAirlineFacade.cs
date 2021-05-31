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
	public class LoggedInAirlineFacade : AnonymousUserFacade, ILoggedInAirlineFacade
	{
		public LoggedInAirlineFacade(NpgsqlConnection connection) : base(connection)
		{
		}

		#region helper methods
		private void VerifyFlightAirline(LoginToken<Airline> token, Flight flight)
		{
			// verifies the flight belongs to the airline
			if (token.User.Id != flight.Airline.Id)
			{
				throw new FlightNotOfAirlineException();
			}
		}

		private void VerifyTokensAirline(LoginToken<Airline> token, Airline airline)
		{
			// verifies the token's user is the passed airline
			if (token.User.Id != airline.Id)
			{
				throw new Exception("Airline does not match the token's user.");
			}
		}
		#endregion

		public IList<Ticket> GetAllMyTickets(LoginToken<Airline> token)
		{
			return _ticketDao.GetByAirlineId(token.User.Id);
		}

		public IList<Flight> GetAllMyFlights(LoginToken<Airline> token)
		{
			return _flightDao.GetByAirlineId(token.User.Id);
		}

		public int CreateFlight(LoginToken<Airline> token, Flight flight)
		{
			VerifyFlightAirline(token, flight);

			return _flightDao.Add(flight);
		}

		public void UpdateFlightDetails(LoginToken<Airline> token, Flight flight)
		{
			VerifyFlightAirline(token, flight);

			_flightDao.Update(flight);
		}

		public void CancelFlight(LoginToken<Airline> token, Flight flight)
		{
			VerifyFlightAirline(token, flight);

			_ticketDao.RemoveByFlightId(flight.Id);

			_flightDao.Remove((int)flight.Id);
		}

		public bool ChangeMyPassword(LoginToken<Airline> token, string oldPassword, string newPassword)
		{
			return _userDao.ChangePassword(token.User.User.Id, oldPassword, newPassword);
		}

		public void UpdateMyDetails(LoginToken<Airline> token, Airline airline)
		{
			VerifyTokensAirline(token, airline);

			_airlineDao.Update(airline);
		}

	}
}
