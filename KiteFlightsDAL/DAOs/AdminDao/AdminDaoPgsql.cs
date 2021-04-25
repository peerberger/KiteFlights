using KiteFlightsDAL.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsDAL.DAOs.AdminDao
{
	public class AdminDaoPgsql : GenericDaoPgsql<Admin>, IAdminDao
	{
		public AdminDaoPgsql(string connectionString) : base(connectionString)
		{
		}
	}
}
