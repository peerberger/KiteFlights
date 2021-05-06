using KiteFlightsCommon.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsCommon.FacadesInterfaces
{
	public interface IAnonymousUserFacade
	{
		IList<Airline> GetAllAirlines();
		Flight GetFlightById(int id);
		IList<Flight> GetAllFlights();
		Dictionary<Flight, int> GetAllFlightsWithVacancies();
		IList<Flight> GetFlightsByOriginCountry(int countryId);
		IList<Flight> GetFlightsByDestinationCountry(int countryId);
		IList<Flight> GetFlightsByDepatrureTime(DateTime departureTime);
		IList<Flight> GetFlightsByLandingTime(DateTime landingTime);
	}
}
