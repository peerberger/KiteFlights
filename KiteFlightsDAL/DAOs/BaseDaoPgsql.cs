using KiteFlightsDAL.HelperClasses.ExtensionMethods;
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
	// when writing the xml doc comments, copy and paste in oneNote
	public class BaseDaoPgsql<TEntity> : IDisposable where TEntity : new()
	{
		// all inheriting DAOs must be of the same connection (the same db).
		// if you want a DAO that connects to a different db but still inherit from this class,
		// just drop the static from: _connection, Sp(), SpExecuteReader(), and SpExecuteScalar()
		protected static NpgsqlConnection _connection;

		// todo: maybe make the ctor static somehow?? only static ctors must be parameterless
		public BaseDaoPgsql(string connectionString)
		{
			//todo: maybe reorganize the testing better? with exception? idk if there is actually something to improve
			if (TestConnection(connectionString))
			{
				_connection = new NpgsqlConnection(connectionString);
			}
			else
			{
				throw new Exception("Connection to DB failed.");
			}
		}

		#region main logic
		// general logic
		private static object Sp(Func<NpgsqlCommand, object> ExecuteCommand, string spName, object parameters = null)
		{
			object result = null;

			try
			{
				_connection.Open();

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

		private static NpgsqlParameter[] GetNpgsqlParameters(object parameters)
		{
			List<NpgsqlParameter> result = new List<NpgsqlParameter>();

			//foreach (var prop in parameters.GetType().GetProperties())
			//{
			//	result.Add(new NpgsqlParameter(prop.Name, prop.GetValue(parameters)));
			//	//result.Add(new NpgsqlParameter(null, prop.GetValue(parameters)));
			//}

			foreach (var prop in (IDictionary<String, Object>)parameters)
			{
				result.Add(new NpgsqlParameter(prop.Key, prop.Value));
			}

			return result.ToArray();
		}

		private static TEntity GenerateEntity(NpgsqlDataReader reader)
		{
			TEntity entity = new TEntity();

			foreach (var prop in entity.GetType().GetProperties())
			{
				string columnName = prop.Name;

				//var attributes = (ColumnAttribute[])prop.GetCustomAttributes(typeof(ColumnAttribute), true);

				//if (attributes.Length > 0)
				//{
				//	columnName = attributes[0].Name;
				//}

				if (prop.TryGetAttributeValue((ColumnAttribute columnAttribute) => columnAttribute.Name, out string columnAttributeName))
				{
					columnName = columnAttributeName;
				}



				var value = reader[columnName];

				prop.SetValue(entity, value);
			}

			return entity;
		}

		// delegates for Sp()
		private static object ExecuteReader(NpgsqlCommand cmd)
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

		private static object ExecuteScalar(NpgsqlCommand cmd)
		{
			return cmd.ExecuteScalar();
		}
		#endregion

		#region helper methods
		// encapsulations for sp()
		protected static List<TEntity> SpExecuteReader(string spName, object parameters = null)
		{
			return Sp(ExecuteReader, spName, parameters) as List<TEntity>;
		}

		protected static object SpExecuteScalar(string spName, object parameters = null)
		{
			return Sp(ExecuteScalar, spName, parameters);
		}

		// test methods
		private static bool TestConnection(string connectionString)
		{
			try
			{
				using (var connection = new NpgsqlConnection(connectionString))
				{
					connection.Open();
					return true;
				}
			}
			catch (Exception ex)
			{
				// todo: add logging
				return false;
			}
		}
		#endregion

		// dispose
		public void Dispose()
		{
			_connection.Dispose();
		}
	}
}
