using KiteFlightsDAL.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsDAL.DAOs.UserDao
{
	public class UserDaoPgsql : GenericDaoPgsql<User>, IUserDao
	{
		public UserDaoPgsql(string connectionString) : base(connectionString)
		{
		}
	}
}
