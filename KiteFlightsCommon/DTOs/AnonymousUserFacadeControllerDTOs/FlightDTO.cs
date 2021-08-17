using KiteFlightsCommon.DTOs.CommonDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsCommon.DTOs.AnonymousUserFacadeControllerDTOs
{
	public class FlightDTO
	{
		public long Id { get; set; }
		public AirlineDTO Airline { get; set; }
		public CountryDTO OriginCountry { get; set; }
		public CountryDTO DestinationCountry { get; set; }
		public DateTime DepartureTime { get; set; }
		public DateTime LandingTime { get; set; }
		public int RemainingTicketsNo { get; set; }
	}
}
