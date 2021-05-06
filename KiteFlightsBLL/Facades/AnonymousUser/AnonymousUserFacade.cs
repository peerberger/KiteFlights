using KiteFlightsCommon.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsBLL.Facades.AnonymousUser
{
	public class AnonymousUserFacade : FacadeBase, IAnonymousUserFacade
	{
		public IList<Airline> GetAllAirlines()
		{
			throw new NotImplementedException();
		}

		public Flight GetFlightById(int id)
		{
			throw new NotImplementedException();
		}

		public IList<Flight> GetAllFlights()
		{
			throw new NotImplementedException();
		}

		public Dictionary<Flight, int> GetAllFlightsWithVacancies()
		{
			throw new NotImplementedException();
		}

		public IList<Flight> GetFlightsByOriginCountry(int countryCode)
		{
			throw new NotImplementedException();
		}

		public IList<Flight> GetFlightsByDestinationCountry(int countryCode)
		{
			throw new NotImplementedException();
		}

		public IList<Flight> GetFlightsByDepatrureTime(DateTime departureTime)
		{
			throw new NotImplementedException();
		}

		public IList<Flight> GetFlightsByLandingTime(DateTime landingTime)
		{
			throw new NotImplementedException();
		}
	}
}
