﻿using KiteFlightsCommon.DaoInterfaces;
using KiteFlightsCommon.POCOs;
using log4net;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsDAL.DAOs.PocoDaos
{
	public class CountryDaoPgsql : GenericDaoPgsql<Country>, ICountryDao
	{
		public CountryDaoPgsql(string connectionString) : base(connectionString)
		{
		}
	}
}