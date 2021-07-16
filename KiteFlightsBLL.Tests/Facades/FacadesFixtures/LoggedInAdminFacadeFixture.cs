using KiteFlightsBLL.Auth;
using KiteFlightsBLL.Facades;
using KiteFlightsCommon.DaoInterfaces;
using KiteFlightsCommon.FacadesInterfaces;
using KiteFlightsCommon.POCOs;
using KiteFlightsDAL.DAOs.PocoDaos;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsBLL.Tests.Facades.FacadesFixtures
{
	public class LoggedInAdminFacadeFixture
	{
		public ILoggedInAdminFacade Facade { get; private set; }


		public LoggedInAdminFacadeFixture()
		{
			Facade = new LoggedInAdminFacade();
		}
	}
}
