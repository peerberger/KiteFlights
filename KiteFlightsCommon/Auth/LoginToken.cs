﻿using KiteFlightsCommon.POCOs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsBLL.Auth
{
	public class LoginToken<T> : ILoginToken where T : IUser
	{
		public T User { get; set; }

		public LoginToken(T user)
		{
			User = user;
		}
	}
}
