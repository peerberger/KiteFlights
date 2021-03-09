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
				Country country = dao.GetById(4);
				//Country country2 = dao.SpExecuteReader<Country>("sp_countries_get_by_id", new { _id = 4 })[0]; // todo: take care of that if this returns an empty collection, trying to access the 0 index will throw an exception

				IList<Country> countries = dao.GetAll();
				//IList<Country> countries2 = dao.SpExecuteReader<Country>("sp_countries_get_all");

				//var id = dao.Add(new Country { Name = "lala" });
				//var id2 = (int)dao.SpExecuteScalar("sp_countries_add", new { _name = "lala" }); // todo: take care of that if this returns null, trying to cast it will throw an exception

				var updated = dao.Update(new Country { Id = 13, Name = "UK" });
				//var updated2 = (bool)dao.SpExecuteScalar("sp_countries_update", new { _id = 13, _name = "UK" }); ; // todo: take care of that if this returns null, trying to cast it will throw an exception

				var removed = dao.Remove(new Country { Id = 55 });
				//var removed2 = (bool)dao.SpExecuteScalar("sp_countries_remove", new { _id = 50 });
			}
		}
	}
}
