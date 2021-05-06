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
	public class AnonymousUserFacade : FacadeBase, IAnonymousUserFacade
	{
		public AnonymousUserFacade(NpgsqlConnection connection) : base(connection)
		{
		}

		public IList<Airline> GetAllAirlines()
		{
			return _airlineDao.GetAll();
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
