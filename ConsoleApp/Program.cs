using KiteFlightsDAL.DAOs.CountryDao;
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
				Country country = dao.GetById(3);

				IList<Country> countries = dao.GetAll();

				var id = dao.Add(new Country { Name = "lala" });

				var updated = dao.Update(new Country { Id = 13, Name = "UK" });

				var removed = dao.Remove(new Country { Id = 1333, Name = "UK" });
			}
		}
	}
}
