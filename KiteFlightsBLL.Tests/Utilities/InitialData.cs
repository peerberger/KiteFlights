using KiteFlightsCommon.POCOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsBLL.Tests.Utilities
{
	public static class InitialData
	{
		private static T GetInitialData<T>(string tableName)
		{
			var json = File.ReadAllText($@"DbInitialData\{tableName}.json");

			return JsonConvert.DeserializeObject<T>(json);
		}

		public static IList<Country> Countries()
		{
			return GetInitialData<IList<Country>>("countries");
		}

		public static IList<Admin> Admins()
		{
			return GetInitialData<IList<Admin>>("admins");
		}

		public static IList<Customer> Customers()
		{
			return GetInitialData<IList<Customer>>("customers");
		}

		public static IList<Airline> Airlines()
		{
			return GetInitialData<IList<Airline>>("airlines");
		}

		public static IList<Flight> Flights()
		{
			return GetInitialData<IList<Flight>>("flights");
		}

		public static IList<Ticket> Tickets()
		{
			return GetInitialData<IList<Ticket>>("tickets");
		}
	}
}
