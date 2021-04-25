using Npgsql;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KiteFlightsDAL.HelperClasses
{
	public class NpgsqlConnectionPool : IDisposable
	{
		#region Singleton stuff
		private static readonly object singletonKey = new object();
		private static NpgsqlConnectionPool instance = null;
		public static NpgsqlConnectionPool Instance
		{
			get
			{
				if (instance == null)
				{
					lock (singletonKey)
					{
						if (instance == null)
						{
							instance = new NpgsqlConnectionPool();
						}
					}
				}

				return instance;
			}
		}
		#endregion

		private object connectionsKey = new object();
		public const int MAX_CONNENCTIONS = 100;
		// should be in config file or passed as parameter(??) !!
		private const string connectionString = @"Host=localhost;Username=postgres;Password=admin;Database=kite_flights_db;";

		// if using BlockingCollection, you must implement IDisposable!
		public BlockingCollection<NpgsqlConnection> Connections { get; set; }


		private NpgsqlConnectionPool()
		{
			if (TestConnection())
			{
				Connections = new BlockingCollection<NpgsqlConnection>(MAX_CONNENCTIONS);
				Init();
			}
			else
			{
				throw new Exception("Connection string is wrong.");
			}
		}

		private void Init()
		{
			for (int i = 0; i < MAX_CONNENCTIONS; i++)
			{
				Connections.Add(new NpgsqlConnection(connectionString));
			}
		}

		public NpgsqlConnection GetConnection()
		{
			var connection = Connections.Take();

			return connection;
		}

		public void ReturnConnection(NpgsqlConnection connection)
		{
			Connections.Add(connection);
		}

		public void RestartPool()
		{
			// idk how to do that well yet
			throw new NotImplementedException();
		}

		public bool TestConnection()
		{
			//logger.Debug("Testing db access");
			try
			{
				using (var my_conn = new NpgsqlConnection(connectionString))
				{
					my_conn.Open();
					//logger.Debug("Testing db access. succeed!");
					return true;
				}
			}
			catch (Exception ex)
			{
				//logger.Fatal($"Testing db access. Failed!. Error: {ex}");
				return false;
			}
		}

		public void Dispose()
		{
			Connections.Dispose();
		}
	}
}