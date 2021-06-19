using KiteFlightsCommon.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsCommon.DaoInterfaces
{
	public interface IUserDao : ICrudDao<User>
	{
		bool ChangePassword(long userId, string oldPassword, string newPassword);
		int GetUserRole(string username, string password);
	}
}
