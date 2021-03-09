using KiteFlightsDAL.HelperClasses;
using KiteFlightsDAL.POCOs;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsDAL.DAOs.CountryDao
{
	public class CountryDaoPgsql : BaseDaoNpgsql, ICountryDao
	{
		public CountryDaoPgsql(string connectionString) : base(connectionString)
		{

		}

		// getting
		public Country GetById(int id)
		{
			Country country = null;

			try
			{
				var spResult = SpExecuteReader<Country>("sp_countries_get_by_id", new { _id = id });

				// check if any records were found
				if (spResult.Count > 0)
				{
					country = spResult.First();
				}
				else
				{
					throw new ArgumentException("No record that matched the Id was found.");
				}
			}
			catch (Exception ex)
			{
				// todo: add logging
			}

			return country;
		}

		public IList<Country> GetAll()
		{
			return SpExecuteReader<Country>("sp_countries_get_all");
		}

		// adding
		public int Add(Country entity)
		{
			var newId = -1;

			var spResult = SpExecuteScalar("sp_countries_add", new { _name = entity.Name });

			newId = spResult != null ? (int)spResult : newId;

			return newId;
		}

		// updating
		public bool Update(Country entity)
		{
			bool updated = false;

			try
			{
				var spResult = SpExecuteScalar("sp_countries_update", new { _id = entity.Id, _name = entity.Name });

				updated = spResult != null ? (bool)spResult : updated;

				if (!updated)
				{
					throw new ArgumentException("No record that matched the entity's Id was found.");
				}
			}
			catch (Exception ex)
			{
				// todo: add logging
			}

			return updated;
		}

		// removing
		public bool Remove(Country entity)
		{
			bool removed = false;

			try
			{
				var spResult = SpExecuteScalar("sp_countries_remove", new { _id = entity.Id });

				removed = spResult != null ? (bool)spResult : removed;
		
				if (!removed)
				{
					throw new ArgumentException("No record that matched the entity's Id was found.");
				}
			}
			catch (Exception ex)
			{
				// todo: add logging
			}

			return removed;
		}
	}
}
