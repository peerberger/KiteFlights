using KiteFlightsDAL.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsDAL.DAOs.CustomerDao
{
	public class CustomerDaoPgsql : GenericDaoPgsql<Customer>, ICustomerDao
	{
		public CustomerDaoPgsql(string connectionString) : base(connectionString)
		{
		}
	}
}
