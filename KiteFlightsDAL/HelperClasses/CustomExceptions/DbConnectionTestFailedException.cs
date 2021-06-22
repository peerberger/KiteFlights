using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsDAL.HelperClasses.CustomExceptions
{
	public class DbConnectionTestFailedException : Exception
	{
		public DbConnectionTestFailedException()
		{
		}
		public DbConnectionTestFailedException(string message) : base(message)
		{
		}
		public DbConnectionTestFailedException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
