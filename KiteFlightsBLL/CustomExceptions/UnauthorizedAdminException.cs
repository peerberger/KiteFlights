using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsBLL.CustomExceptions
{
	public class UnauthorizedAdminException : Exception
	{
		public UnauthorizedAdminException()
		{
		}
		public UnauthorizedAdminException(string message) : base(message)
		{
		}
		public UnauthorizedAdminException(string message, Exception innerException) : base(message, innerException)
		{
		}
		public UnauthorizedAdminException(int level)
		{
			Level = level;
			Message = $"Admins below level {Level} are unauthorized to perform this action.";
		}

		public override string Message { get; }
		public virtual int? Level { get; }
	}
}
