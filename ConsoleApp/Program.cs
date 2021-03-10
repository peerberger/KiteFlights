using KiteFlightsDAL.DAOs.CountryDao;
using KiteFlightsDAL.DAOs.UserDao;
using KiteFlightsDAL.POCOs;
using System;
using System.Collections.Generic;

namespace ConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			string connectionString = @"Host=localhost;Username=postgres;Password=admin;Database=kite_flights_db;";

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
