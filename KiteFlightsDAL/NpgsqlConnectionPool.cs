using KiteFlightsDAL.HelperClasses.CustomExceptions;
using Npgsql;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsDAL
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

		public const int MAX_CONNENCTIONS = 100;
		// should be in config file or passed as parameter(?? is it possible in a singleton??) !!
		//private const string connectionString = @"Host=localhost;Username=postgres;Password=admin;Database=kite_flights_db;";
		string connectionString = @"Host=localhost;Username=postgres;Password=admin;Database=kite_flights_tests_d;";

		public BlockingCollection<NpgsqlConnection> Connections { get; set; }

		private NpgsqlConnectionPool()
		{
			TestConnection();

			Connections = new BlockingCollection<NpgsqlConnection>(MAX_CONNENCTIONS);
			Init();
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

			connection.Open();

			return connection;
		}

		public void ReturnConnection(NpgsqlConnection connection)
		{
			connection.Close();

			Connections.Add(connection);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <exception cref="DbConnectionTestFailedException"></exception>
		private void TestConnection()
		{
			try
			{
				using (var connection = new NpgsqlConnection(connectionString))
				{
					connection.Open();
					connection.Close();
					//logger.Debug("Testing db access. succeed!");
				}
			}
			catch (Exception ex)
			{
				throw new DbConnectionTestFailedException($"Database connection test failed.\r\n\t\t\tConnection string: {connectionString}", ex);
			}
		}

		public void Dispose()
		{
			Connections.Dispose();
		}
	}

}
