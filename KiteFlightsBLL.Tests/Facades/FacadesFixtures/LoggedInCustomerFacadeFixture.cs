using KiteFlightsBLL.Facades;
using KiteFlightsCommon.FacadesInterfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsBLL.Tests.Facades.FacadesFixtures
{
	public class LoggedInCustomerFacadeFixture
	{
		public ILoggedInCustomerFacade Facade { get; private set; }


		public LoggedInCustomerFacadeFixture()
		{
			Facade = new LoggedInCustomerFacade();
		}
	}
}
