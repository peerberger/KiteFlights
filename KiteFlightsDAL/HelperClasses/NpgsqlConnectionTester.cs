using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsDAL.HelperClasses
{
	public static class NpgsqlConnectionTester
	{
		public static bool Test(string connectionString)
		{
			try
			{
				using (var connection = new NpgsqlConnection(connectionString))
				{
					connection.Open();
					return true;
				}
			}
			catch (Exception ex)
			{
				// todo: add logging
				return false;
			}
		}
	}
}