using KiteFlightsDAL.DAOs;
using KiteFlightsDAL.DAOs.CountryDao;
using KiteFlightsDAL.DAOs.UserDao;
using KiteFlightsDAL.POCOs;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ConsoleApp
{
	class Program
	{
		private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		static void Main(string[] args)
		{
			var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
			XmlConfigurator.Configure(logRepository, new FileInfo("Log4Net.config"));

			string connectionString = @"Host=localhost;Username=postgres;Password=admin;Database=kite_flights_db;";

			logger.Info("lalala");

			using (var dao = new GenericDaoPgsql<Country>(connectionString))
			{
				Country country = dao.GetById(4);

				IList<Country> countries = dao.GetAll();
			
				var id = dao.Add(new Country { Name = "lala" });
			}

			using (var dao = new GenericDaoPgsql<User>(connectionString))
			{
				User user = dao.GetById(3);

				IList<User> users = dao.GetAll();

			}


			using (var dao = new CountryDaoPgsql(connectionString))
			{
				Country country = dao.GetById(4);

				IList<Country> countries = dao.GetAll();

				//var id = dao.Add(new Country { Name = "lala" });

				var updated = dao.Update(new Country { Id = 13, Name = "UK" });

				var removed = dao.Remove(new Country { Id = 58 });
			}

			using (var dao = new UserDaoPgsql(connectionString))
			{
				User user = dao.GetById(3);

				IList<User> users = dao.GetAll();

				//var id = dao.Add(new User { Username = "gaga", Password = "gaga", Email = "gaga", UserRole = 1 });

				var updated = dao.Update(new User { Id = 6, Username = "lala", Password = "lala", Email = "lala", UserRole = 1 });

				var removed = dao.Remove(new User { Id = 9 });
			}
		}
	}
}
