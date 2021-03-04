using KiteFlightsDAL.HelperClasses;
using KiteFlightsDAL.POCOs;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsDAL.DAOs.CountryDao
{
	public class CountryDaoPgsql : ICountryDao, IDisposable
	{
		protected NpgsqlConnection _connection;

		public CountryDaoPgsql(string connectionString)
		{
			//todo: maybe reorganize the testing better? with exception? idk if there is actaully something to improve
			if (NpgsqlConnectionTester.Test(connectionString))
			{
				_connection = new NpgsqlConnection(connectionString);
			}
			else
			{
				throw new Exception("Connection to DB failed.");
			}
		}

		// getting
		public Country GetById(int id)
		{
			Country country = null;

			_connection.Open();

			try
			{
				using (var cmd = new NpgsqlCommand("SELECT * FROM sp_countries_get_by_id(@_id);", _connection))
				{
					cmd.Parameters.AddWithValue("@_id", id);

					using (var reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							country = new Country();

							country.Id = reader.GetInt32(0);
							country.Name = reader.GetString(1);
						}
						else
						{
							throw new ArgumentException("No record that matched the Id was found.");
						}
					}
				}
			}
			catch (Exception ex)
			{
				// todo: add logging
			}
			finally
			{
				_connection.Close();
			}

			return country;
		}

		public IList<Country> GetAll()
		{
			var countries = new List<Country>();

			_connection.Open();

			try
			{
				using (var cmd = new NpgsqlCommand("SELECT * FROM sp_countries_get_all()", _connection))
				{
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							var country = new Country();

							country.Id = reader.GetInt32(0);
							country.Name = reader.GetString(1);

							countries.Add(country);
						}
					}
				}
			}
			catch (Exception ex)
			{
				// todo: add logging
			}
			finally
			{
				_connection.Close();
			}

			return countries;
		}

		// adding
		public int Add(Country entity)
		{
			var newId = -1;

			_connection.Open();

			try
			{
				using (var cmd = new NpgsqlCommand("SELECT * FROM sp_countries_add(@_name);", _connection))
				{
					cmd.Parameters.AddWithValue("@_name", entity.Name);

					newId = (int)cmd.ExecuteScalar();
				}
			}
			catch (Exception ex)
			{
				// todo: add logging
			}
			finally
			{
				_connection.Close();
			}

			return newId;
		}

		// updating
		public bool Update(Country entity)
		{
			bool updated = false;

			_connection.Open();

			try
			{
				using (var cmd = new NpgsqlCommand("SELECT * FROM sp_countries_update(@_id, @_name);", _connection))
				{
					cmd.Parameters.AddWithValue("@_id", entity.Id);
					cmd.Parameters.AddWithValue("@_name", entity.Name);

					updated = (bool)cmd.ExecuteScalar();

					if (!updated)
					{
						throw new ArgumentException("No record that matched the entity's Id was found.");
					}
				}
			}
			catch (Exception ex)
			{
				// todo: add logging
			}
			finally
			{
				_connection.Close();
			}

			return updated;
		}

		// removing
		public bool Remove(Country entity)
		{
			bool removed = false;

			_connection.Open();

			try
			{
				using (var cmd = new NpgsqlCommand("SELECT * FROM sp_countries_remove(@_id);", _connection))
				{
					cmd.Parameters.AddWithValue("@_id", entity.Id);

					removed = (bool)cmd.ExecuteScalar();

					if (!removed)
					{
						throw new ArgumentException("No record that matched the entity's Id was found.");
					}
				}
			}
			catch (Exception ex)
			{
				// todo: add logging
			}
			finally
			{
				_connection.Close();
			}

			return removed;
		}

		// dispose
		public void Dispose()
		{
			_connection.Dispose();
		}
	}
}
