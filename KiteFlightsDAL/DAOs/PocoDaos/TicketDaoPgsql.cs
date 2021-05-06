using KiteFlightsCommon.DaoInterfaces;
using KiteFlightsCommon.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsDAL.DAOs.PocoDaos
{
	public class TicketDaoPgsql : GenericDaoPgsql<Ticket>, ITicketDao
	{
		public TicketDaoPgsql(string connectionString) : base(connectionString)
		{
		}
	}
}
