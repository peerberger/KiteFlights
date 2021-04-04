using KiteFlightsDAL.POCOs;
using log4net;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsDAL.DAOs.CountryDao
{
	public class CountryDaoPgsql : GenericDaoPgsql<Country>, ICountryDao
	{
		private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public CountryDaoPgsql(string connectionString) : base(connectionString)
		{
		}
	}
}
