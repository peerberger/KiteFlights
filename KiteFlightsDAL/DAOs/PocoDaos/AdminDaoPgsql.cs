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
	public class AdminDaoPgsql : GenericDaoPgsql<Admin>, IAdminDao
	{
		public AdminDaoPgsql(NpgsqlConnection connection) : base(connection)
		{
		}

		public Admin GetByUsername(string username)
		{
			var parameters = new List<object> { username };

			return SpExecuteReaderReturningSingleRecord($"sp_{TableName}_get_by_username", parameters);
		}
	}
}
