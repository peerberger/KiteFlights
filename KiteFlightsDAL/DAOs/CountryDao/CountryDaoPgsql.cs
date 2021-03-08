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
				using (var cmd = new NpgsqlCommand("SELECT * FROM sp_countries_get_by_id(@id);", _connection))
				{
					cmd.Parameters.AddWithValue("id", id);

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

		private NpgsqlParameter[] GetParametersFromDataHolder(object parameters)
		{
			List<NpgsqlParameter> result = new List<NpgsqlParameter>();

			foreach (var prop in parameters.GetType().GetProperties())
			{
				result.Add(new NpgsqlParameter(prop.Name, prop.GetValue(parameters)));
				//result.Add(new NpgsqlParameter(null, prop.GetValue(parameters)));
			}

			return result.ToArray();
		}

		public List<TEntity> SpExecuteReader<TEntity>(string sp_name, object parameters = null) where TEntity : new()
		{
			List<TEntity> result = new List<TEntity>();

			_connection.Open();

			try
			{
				using (var cmd = new NpgsqlCommand(sp_name, _connection))
				{
					cmd.CommandType = CommandType.StoredProcedure;

					if (parameters != null)
					{
						cmd.Parameters.AddRange(GetParametersFromDataHolder(parameters));
					}

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							TEntity entity = new TEntity();
							//Type type = typeof(TEntity);

							foreach (var prop in entity.GetType().GetProperties())
							{
								string columnName = prop.Name;

								var attributes = (ColumnAttribute[])prop.GetCustomAttributes(typeof(ColumnAttribute), true);

								if (attributes.Length > 0)
								{
									columnName = attributes[0].Name;
								}

								var value = reader[columnName];

								prop.SetValue(entity, value);
							}

							result.Add(entity);
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

			return result;
		}

		public object SpExecuteScalar(string sp_name, object parameters = null)
		{
			object result = null;

			_connection.Open();

			try
			{
				using (var cmd = new NpgsqlCommand(sp_name, _connection))
				{
					cmd.CommandType = CommandType.StoredProcedure;

					if (parameters != null)
					{
						cmd.Parameters.AddRange(GetParametersFromDataHolder(parameters));
					}

					result = cmd.ExecuteScalar();
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

			return result;
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
				using (var cmd = new NpgsqlCommand("SELECT * FROM sp_countries_add(@name);", _connection))
				{
					cmd.Parameters.AddWithValue("name", entity.Name);

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
				using (var cmd = new NpgsqlCommand("SELECT * FROM sp_countries_update(@id, @name);", _connection))
				{
					cmd.Parameters.AddWithValue("id", entity.Id);
					cmd.Parameters.AddWithValue("name", entity.Name);

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
				using (var cmd = new NpgsqlCommand("SELECT * FROM sp_countries_remove(@id);", _connection))
				{
					cmd.Parameters.AddWithValue("id", entity.Id);

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
