using KiteFlightsDAL.HelperClasses.ExtensionMethods;
using KiteFlightsDAL.POCOs.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsDAL.DAOs
{
	// when writing the xml doc comments, copy and paste in oneNote
	public class BaseDaoPgsql<TEntity> : IDisposable where TEntity : IPoco, new()
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
		private static object Sp(Func<NpgsqlCommand, object> ExecuteCommand, string spName, Dictionary<string, object> parameters = null)
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

		private static NpgsqlParameter[] GetNpgsqlParameters(Dictionary<string, object> parameters)
		{
			return parameters.Select(kvp => new NpgsqlParameter(kvp.Key, kvp.Value)).ToArray();
		}

		// todo: maybe you can improve the 'i' thing - make it simpler to go through the columns by ordinal number
		//private static Entity GenerateEntity<Entity>(NpgsqlDataReader reader, ref int i) where Entity : IPoco, new()
		//{
		//	Entity entity = new Entity();

		//	foreach (var prop in entity.GetType().GetProperties())
		//	{
		//		Type type = prop.PropertyType;
		//		object value = null;

		//		// If property is a contained POCO,
		//		// call recursively for GenerateEntity() on it.
		//		if (typeof(IPoco).IsAssignableFrom(type))
		//		{
		//			// call to GenerateEntity<type>(reader, i);
		//			value = typeof(BaseDaoPgsql<TEntity>)
		//						.GetMethod(nameof(GenerateEntity), BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
		//						.MakeGenericMethod(type)
		//						.Invoke(null, new object[] { reader, i });
		//		}
		//		else
		//		{
		//			value = reader.GetValue(i);
		//		}

		//		i++;

		//		prop.SetValue(entity, value);
		//	}

		//	return entity;
		//}

		// todo: maybe you can improve the 'i' thing - make it simpler to go through the columns by ordinal number
		private static Entity GenerateEntity<Entity>(NpgsqlDataReader reader, int ordinal = 0) where Entity : IPoco, new()
		{
			Entity entity = new Entity();
			var props = entity.GetType().GetProperties();
			var columnOffset = 0;

			for (int i = ordinal; i < props.Length + ordinal; i++)
			{
				Type type = props[i - ordinal].PropertyType;
				object value = null;

				// If property is a contained POCO,
				// call recursively for GenerateEntity() on it.
				if (typeof(IPoco).IsAssignableFrom(type))
				{
					// call to GenerateEntity<type>(reader, i);
					value = typeof(BaseDaoPgsql<TEntity>)
								.GetMethod(nameof(GenerateEntity), BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
								.MakeGenericMethod(type)
								//.Invoke(null, new object[] { reader, i });
								.Invoke(null, new object[] { reader, i + columnOffset});
					props[i - ordinal].SetValue(entity, value);
					columnOffset += value.GetType().GetProperties().Length - 1;
				}
				else
				{
					value = reader.GetValue(i);
					props[i - ordinal].SetValue(entity, value);
				}

				//props[i - ordinal].SetValue(entity, value);
			}

			return entity;
		}

		//private static Entity GenerateEntity<Entity>(NpgsqlDataReader reader) where Entity : IPoco, new()
		//{
		//	Entity entity = new Entity();

		//	foreach (var prop in entity.GetType().GetProperties())
		//	{

		//	}
		//}

		// delegates for Sp()
		private static object ExecuteReader(NpgsqlCommand cmd)
		{
			List<TEntity> result = new List<TEntity>();

			using (var reader = cmd.ExecuteReader())
			{
				while (reader.Read())
				{
					//int i = 0;
					//TEntity entity = GenerateEntity<TEntity>(reader, ref i);
					TEntity entity = GenerateEntity<TEntity>(reader);

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
		protected static List<TEntity> SpExecuteReader(string spName, Dictionary<string, object> parameters = null)
		{
			return Sp(ExecuteReader, spName, parameters) as List<TEntity>;
		}

		protected static object SpExecuteScalar(string spName, Dictionary<string, object> parameters = null)
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
