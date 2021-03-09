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
	public class BaseDaoNpgsql<TEntity> : IDisposable where TEntity : new()
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

		private static NpgsqlParameter[] GetNpgsqlParameters(object parameters)
		{
			List<NpgsqlParameter> result = new List<NpgsqlParameter>();

			foreach (var prop in parameters.GetType().GetProperties())
			{
				result.Add(new NpgsqlParameter(prop.Name, prop.GetValue(parameters)));
				//result.Add(new NpgsqlParameter(null, prop.GetValue(parameters)));
			}

			return result.ToArray();
		}

		private static TEntity GenerateEntity(NpgsqlDataReader reader)
		{
			TEntity entity = new TEntity();

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

			return entity;
		}

		protected List<TEntity> SpExecuteReader(string spName, object parameters = null)
		{
			List<TEntity> result = new List<TEntity>();

			_connection.Open();

			try
			{
				using (var cmd = new NpgsqlCommand(spName, _connection))
				{
					cmd.CommandType = CommandType.StoredProcedure;

					if (parameters != null)
					{
						cmd.Parameters.AddRange(GetNpgsqlParameters(parameters));
					}

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							TEntity entity = GenerateEntity(reader);

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

		protected static object ExecuteReader(NpgsqlCommand cmd)
		{
			List<TEntity> result = new List<TEntity>();

			using (var reader = cmd.ExecuteReader())
			{
				while (reader.Read())
				{
					TEntity entity = GenerateEntity(reader);

					result.Add(entity);
				}
			}

			return result;
		}

		protected static object ExecuteScalar(NpgsqlCommand cmd)
		{
			return cmd.ExecuteScalar();
		}

		protected object ExecuteSp(Func<NpgsqlCommand, object> ExecuteCommand, string spName, object parameters = null)
		{
			object result = null;

			_connection.Open();

			try
			{
				using (var cmd = new NpgsqlCommand(spName, _connection))
				{
					cmd.CommandType = CommandType.StoredProcedure;

					if (parameters != null)
					{
						cmd.Parameters.AddRange(GetNpgsqlParameters(parameters));
					}

					result = ExecuteCommand(cmd);
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

		protected object SpExecuteScalar(string spName, object parameters = null)
		{
			object result = null;

			_connection.Open();

			try
			{
				using (var cmd = new NpgsqlCommand(spName, _connection))
				{
					cmd.CommandType = CommandType.StoredProcedure;

					if (parameters != null)
					{
						cmd.Parameters.AddRange(GetNpgsqlParameters(parameters));
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
