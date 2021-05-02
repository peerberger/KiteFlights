using KiteFlightsDAL.DAOs;
using KiteFlightsDAL.DAOs.AdminDao;
using KiteFlightsDAL.DAOs.AirlineDao;
using KiteFlightsDAL.DAOs.CountryDao;
using KiteFlightsDAL.DAOs.CustomerDao;
using KiteFlightsDAL.DAOs.FlightDao;
using KiteFlightsDAL.DAOs.TicketDao;
using KiteFlightsDAL.DAOs.UserDao;
using KiteFlightsDAL.HelperClasses;
using KiteFlightsDAL.POCOs;
using log4net;
using log4net.Config;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
	class Program
	{
		private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private static ManualResetEvent gate = new ManualResetEvent(false);

		//public static void Foo(NpgsqlConnectionPool pool)
		//{
		//	gate.WaitOne();


		//	var connection = pool.GetConnection();
		//	Thread.Sleep(5000);
		//	pool.ReturnConnection(connection);

		//	var name = Thread.CurrentThread.Name;

		//	if (name=="Thread_150")
		//	{
		//		//Thread.Sleep(5000);
		//		//pool.RestartPool();

		//		//pool.ReturnConnection(new NpgsqlConnection(@"Host=localhost;Username=postgres;Password=admin;Database=kite_flights_db;"));
		//	}

		//	Console.WriteLine($"{name} ended");
		//}

		static void Main(string[] args)
		{
			#region connection pool
			//var pool = NpgsqlConnectionPool.Instance;

			//for (int i = 0; i < 200; i++)
			//{
			//	Thread thread = new Thread(() => Foo(pool));
			//	thread.Name = "Thread_" + i;
			//	thread.Start();
			//}

			//gate.Set();
			////Thread.Sleep(5000);
			////Console.WriteLine("***********************");
			#endregion

			//var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
			//XmlConfigurator.Configure(logRepository, new FileInfo("Log4Net.config"));

			string connectionString = @"Host=localhost;Username=postgres;Password=admin;Database=kite_flights_db;";

			//logger.Info("lalala");

			//using (var dao = new CountryDaoPgsql(connectionString))
			//{
			//	var country = dao.GetById(4);

			//	var countries = dao.GetAll();

			//	var id = dao.Add(new Country { Name = "lala" } );

			//	//var updated = dao.Update(new Country { Id = 13, Name = "UK" });

			//	//var removed = dao.Remove(new Country { Id = 60 });
			//}

			//using (var dao = new UserDaoPgsql(connectionString))
			//{
			//	var user = dao.GetById(3);

			//	var users = dao.GetAll();

			//	var id = dao.Add(new User { Username = "gaga", Password = "gaga1", Email = "ga@ga", UserRole = 1 });

			//	//var updated = dao.Update(new User { Id = 6, Username = "lala", Password = "lala", Email = "lala", UserRole = 1 });

			//	//var removed = dao.Remove(new User { Id = 11 });
			//}

			//using (var dao = new AdminDaoPgsql(connectionString))
			//{
			//	var admins = dao.GetAll();

			//	var admin = dao.GetById(3);

			//	var id = dao.Add(new Admin { FirstName = "bill", LastName = "gaits", Level = 1, User = new User { Username = "billy", Password = "gaits1", Email = "bill@ms", UserRole = 3 } });
			//}

			//using (var dao = new CustomerDaoPgsql(connectionString))
			//{
			//	var customers = dao.GetAll();

			//	var customer = dao.GetById(2);

			//	var id = dao.Add(new Customer { FirstName = "pepper", LastName = "berger", Address = "pepe st.", PhoneNo = "13", CreditCardNo = "1313", User = new User { Username = "pepe", Password = "pepe1", Email = "pe@pe", UserRole = 1 } });
			//}

			//using (var dao = new AirlineDaoPgsql(connectionString))
			//{
			//	var airlines = dao.GetAll();

			//	var airline = dao.GetById(2);

			//	var id = dao.Add(new Airline { Name = "delta", Country = new Country { Id = 4 }, User = new User { Username = "delta", Password = "delta1", Email = "del@ta", UserRole = 2 } });
			//}

			//using (var dao = new FlightDaoPgsql(connectionString))
			//{
			//	var flights = dao.GetAll();

			//	var flight = dao.GetById(2);

			//	var id = dao.Add(new Flight
			//	{
			//		Airline = new Airline { Id = 2 },
			//		OriginCountry = new Country { Id = 1, },
			//		DestinationCountry = new Country { Id = 4, },
			//		DepartureTime = new DateTime(2021, 3, 10, 16, 34, 39, 941),
			//		LandingTime = new DateTime(2021, 3, 10, 16, 34, 39, 941),
			//		RemainingTicketsNo = 30
			//	});
			//}

			using (var dao = new TicketDaoPgsql(connectionString))
			{
				var tickets = dao.GetAll();

				var ticket = dao.GetById(2);

				var id = dao.Add(new Ticket { Flight = new Flight { Id = 4 }, Customer = new Customer { Id = 5 } });
			}
		}
	}
}
