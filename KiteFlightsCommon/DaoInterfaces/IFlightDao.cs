using KiteFlightsCommon.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsCommon.DaoInterfaces
{
	public interface IFlightDao : ICrudDao<Flight>
	{
		Dictionary<Flight, int> GetAllFlightsWithVacancies();
		IList<Flight> GetFlightsByOriginCountry(int countryId);
		IList<Flight> GetFlightsByDestinationCountry(int countryId);
		IList<Flight> GetFlightsByDepatrureTime(DateTime departureTime);
		IList<Flight> GetFlightsByLandingTime(DateTime landingTime);
		IList<Flight> GetFlightsByCustomer(long customerId);
		int RemoveByAirlineId(long airlineId);
		int RemoveByCountryId(int countryId);
		IList<Flight> GetByAirlineId(long airlineId);
	}
}
