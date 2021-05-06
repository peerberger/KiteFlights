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
		IList<Flight> GetFlightsByOriginCountry(int countryCode);
		IList<Flight> GetFlightsByDestinationCountry(int countryCode);
		IList<Flight> GetFlightsByDepatrureTime(DateTime departureTime);
		IList<Flight> GetFlightsByLandingTime(DateTime landingTime);
		IList<Flight> GetFlightsByCustomer(long customerId);
	}
}
