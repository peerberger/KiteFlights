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

			var changedSuccessfully = SpExecuteScalarWithDefaultReturnValue($"sp_{TableName}_change_password", false, parameters);

			return changedSuccessfully;
		}

		public int GetUserRole(string username, string password)
		{
			var parameters = new List<object> { username, password };

			var userRole = SpExecuteScalarWithDefaultReturnValue($"sp_{TableName}_get_user_role", -1, parameters);

			return userRole;
		}
	}
}
