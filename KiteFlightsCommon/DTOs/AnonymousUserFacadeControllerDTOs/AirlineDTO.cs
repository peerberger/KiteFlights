using KiteFlightsCommon.DTOs.CommonDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsCommon.DTOs.AnonymousUserFacadeControllerDTOs
{
	public class AirlineDTO
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public CountryDTO Country { get; set; }
	}
}
