using KiteFlightsBLL.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsBLL.Auth
{
	public interface ILoginService : IDisposable
	{
		bool TryLogin(string username, string password, out ILoginToken token, out FacadeBase facade);
	}
}
