using KiteFlightsDAL.HelperClasses;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsDAL.DAOs
{
	public class BaseDaoNpgsql : IDisposable
	{
		protected NpgsqlConnection _connection;

		public BaseDaoNpgsql(string connectionString)
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

		protected List<TEntity> SpExecuteReader<TEntity>(string sp_name, object parameters = null) where TEntity : new()
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

		protected object SpExecuteScalar(string sp_name, object parameters = null)
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

		// dispose
		public void Dispose()
		{
			_connection.Dispose();
		}
	}
}
