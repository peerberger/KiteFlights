using KiteFlightsCommon.DaoInterfaces;
using KiteFlightsCommon.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsDAL.DAOs.PocoDaos
{
	public class UserDaoPgsql : GenericDaoPgsql<User>, IUserDao
	{
		public UserDaoPgsql(string connectionString) : base(connectionString)
		{
		}
	}
}
