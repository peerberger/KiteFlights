using KiteFlightsDAL.HelperClasses.CustomExceptions;
using Npgsql;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace KiteFlightsDAL
{
	//public class NpgsqlConnectionPool : IDisposable
	public class NpgsqlConnectionPool
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
		// todo: should be in config file or passed as parameter(?? is it possible in a singleton??) !!
		//private const string connectionString = @"Host=localhost;Username=postgres;Password=admin;Database=kite_flights_db;";
		private const string connectionString = @"Host=localhost;Username=postgres;Password=admin;Database=kite_flights_tests_db;";

		public BlockingCollection<NpgsqlConnection> Connections { get; }
		public Channel<NpgsqlConnection> ConnectionsAsync { get; }

		private NpgsqlConnectionPool()
		{
			TestConnection();

			Connections = new BlockingCollection<NpgsqlConnection>(MAX_CONNENCTIONS/2);
			ConnectionsAsync = Channel.CreateBounded<NpgsqlConnection>(MAX_CONNENCTIONS/2);
			Init();
			InitAsync();
		}

		private void Init()
		{
			Connections.Add(new NpgsqlConnection(connectionString));
		}
		private async Task InitAsync()
		{
			for (int i = 0; i < MAX_CONNENCTIONS; i++)
			{
				//Connections.Add(new NpgsqlConnection(connectionString));

				var connection = new NpgsqlConnection(connectionString);

				if (ConnectionsAsync.Writer.TryWrite(connection))
				{
					continue;
				}

				await ConnectionsAsync.Writer.WriteAsync(connection);
			}
		}

		public NpgsqlConnection GetConnection()
		{
			var connection = Connections.Take();

			connection.Open();

			return connection;
		}
		public async Task<NpgsqlConnection> GetConnectionAsync()
		{
			//var connection = Connections.Take();
			var connection = await ConnectionsAsync.Reader.ReadAsync();

			connection.Open();

			return connection;
		}

		public void ReturnConnection(NpgsqlConnection connection)
		{
			connection.Close();

			Connections.Add(connection);
		}
		public async Task ReturnConnectionAsync(NpgsqlConnection connection)
		{
			connection.Close();

			//Connections.Add(connection);
			ConnectionsAsync.Writer.WriteAsync(connection);
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
				throw new DbConnectionTestFailedException($"Database connection test failed.\r\n\tConnection string: {connectionString}", ex);
			}
		}

		//public void Dispose()
		////public async Task DisposeAsync()
		//{
		//	//Connections.Dispose();

		//	Connections.Writer.Complete();

		//	try
		//	{
		//		while (true)
		//		{
		//			var connection = await Connections.Reader.ReadAsync();

		//			connection.DisposeAsync();
		//		}
		//	}
		//	catch (ChannelClosedException) { }

		//	while (await Connections.Reader.WaitToReadAsync())
		//	{
		//		if (c.TryRead(out int item))
		//		{
		//			// process item...
		//		}
		//	}
		//}
	}
}
