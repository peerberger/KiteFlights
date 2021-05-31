using KiteFlightsCommon.DaoInterfaces;
using KiteFlightsCommon.POCOs;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsDAL.DAOs.PocoDaos
{
	public class UserDaoPgsql : GenericDaoPgsql<User>, IUserDao
	{
		public UserDaoPgsql(NpgsqlConnection connection) : base(connection)
		{
		}

		public bool ChangePassword(long userId, string oldPassword, string newPassword)
		{
			var parameters = new List<object> { userId, oldPassword, newPassword };

			return (bool)SpExecuteScalar($"sp_{TableName}_change_password", parameters);
		}
	}
}
