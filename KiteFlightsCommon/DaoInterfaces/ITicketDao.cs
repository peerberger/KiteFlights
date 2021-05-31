﻿using KiteFlightsCommon.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsCommon.DaoInterfaces
{
	public interface ITicketDao : ICrudDao<Ticket>
	{
		int RemoveByCustomerId(long customerId);
		int RemoveByAirlineId(long airlineId);
		int RemoveByCountryId(int countryId);
		IList<Ticket> GetByAirlineId(long airlineId);
		int RemoveByFlightId(long flightId);
	}
}
