using KiteFlightsCommon.POCOs.Interfaces;
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
	public class BaseDaoPgsql<TEntity> where TEntity : IPoco, new()
	{
		// all inheriting DAOs must be of the same connection (the same db).
		// if you want a DAO that connects to a different db but still inherit from this class,
		// just drop the static from: _connection, Sp(), SpExecuteReader(), and SpExecuteScalar()
		protected static NpgsqlConnection _connection;
		private static readonly object key = new object();
		private static int i = 0;

		// todo: maybe make the ctor static somehow?? its just that static ctors must be parameterless
		public BaseDaoPgsql(NpgsqlConnection connection)
		{
			_connection = connection;
		}

		#region main logic
		// general logic
		private static object Sp(Func<NpgsqlCommand, object> ExecuteCommand, string spName, List<object> parameters = null)
		{
			object result = null;

			using (var cmd = new NpgsqlCommand(spName, _connection))
			{
				cmd.CommandType = CommandType.StoredProcedure;

				if (parameters != null)
				{
					cmd.Parameters.AddRange(GetNpgsqlParameters(parameters));
				}

				result = ExecuteCommand(cmd);
			}

			return result;
		}

		private static NpgsqlParameter[] GetNpgsqlParameters(List<object> parameters)
		{
			return parameters.Select(param => new NpgsqlParameter(null, param)).ToArray();
		}

		// this relies on the order of columns in the result from the sp
		private static PocoEntity GenerateEntity<PocoEntity>(NpgsqlDataReader reader) where PocoEntity : IPoco, new()
		{
			PocoEntity entity = new PocoEntity();

			foreach (var prop in entity.GetType().GetProperties())
			{
				Type type = prop.PropertyType;
				object value;

				// If property is a contained POCO,
				// recursively invoke GenerateEntity() upon it.
				if (typeof(IPoco).IsAssignableFrom(type))
				{
					value = InvokeGenerateEntity(reader, type);
				}
				else
				{
					value = reader.GetValue(i);
					i++;
				}

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
					TEntity entity = GenerateEntitySafely(reader);

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
		protected static List<TEntity> SpExecuteReader(string spName, List<object> parameters = null)
		{
			var spResult = Sp(ExecuteReader, spName, parameters) as List<TEntity>;
			
			return spResult;
		}

		protected static TEntity SpExecuteReaderReturningSingleRecord(string spName, List<object> parameters = null)
		{
			var spResult = SpExecuteReader(spName, parameters);

			return spResult.FirstOrDefault();
		}

		protected static object SpExecuteScalar(string spName, List<object> parameters = null)
		{
			return Sp(ExecuteScalar, spName, parameters);
		}

		// encapsulations for GenerateEntity<PocoEntity>()
		private static TEntity GenerateEntitySafely(NpgsqlDataReader reader)
		{
			TEntity entity;

			lock (key)
			{
				entity = GenerateEntity<TEntity>(reader);
				i = 0;
			}

			return entity;
		}

		private static object InvokeGenerateEntity(NpgsqlDataReader reader, Type type)
		{
			// all of this is to invoke a generic method with a dynamic type
			var entity = typeof(BaseDaoPgsql<TEntity>)
								.GetMethod(nameof(GenerateEntity), BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
								.MakeGenericMethod(type)
								.Invoke(null, new object[] { reader });

			return entity;
		}
		#endregion
	}
}
