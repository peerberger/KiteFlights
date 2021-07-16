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
		public AnonymousUserFacade()
		{
		}

		public IList<Airline> GetAllAirlines()
		{
			return _airlineDao.GetAll();
		}

		public Flight GetFlightById(int id)
		{
			return _flightDao.GetById(id);
		}

		public IList<Flight> GetAllFlights()
		{
			return _flightDao.GetAll();
		}

		public Dictionary<Flight, int> GetAllFlightsWithVacancies()
		{
			return _flightDao.GetAllFlightsWithVacancies();
		}

		public IList<Flight> GetFlightsByOriginCountry(int countryId)
		{
			return _flightDao.GetFlightsByOriginCountry(countryId);
		}

		public IList<Flight> GetFlightsByDestinationCountry(int countryId)
		{
			return _flightDao.GetFlightsByDestinationCountry(countryId);
		}

		public IList<Flight> GetFlightsByDepatrureTime(DateTime departureTime)
		{
			return _flightDao.GetFlightsByDepatrureTime(departureTime);
		}

		public IList<Flight> GetFlightsByLandingTime(DateTime landingTime)
		{
			return _flightDao.GetFlightsByLandingTime(landingTime);
		}
	}
}
