﻿using KiteFlightsCommon.POCOs.Interfaces;
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
		private static NpgsqlConnectionPool _connectionPool = NpgsqlConnectionPool.Instance;
		private static readonly object key = new object();
		private static int i = 0;

		static BaseDaoPgsql()
		{
		}

		#region main logic
		// general logic
		private static object Sp(Func<NpgsqlCommand, object> ExecuteCommand, string spName, List<object> parameters = null)
		{
			object result = null;

			var connection = _connectionPool.GetConnectionAsync();

			using (var cmd = new NpgsqlCommand(spName, connection))
			{
				cmd.CommandType = CommandType.StoredProcedure;

				if (parameters != null)
				{
					cmd.Parameters.AddRange(GetNpgsqlParameters(parameters));
				}

				result = ExecuteCommand(cmd);
			}

			_connectionPool.ReturnConnection(connection);

			return result;
		}
		private static async Task<object> SpAsync(Func<NpgsqlCommand, object> ExecuteCommand, string spName, List<object> parameters = null)
		{
			object result = null;

			//var connection = _connectionPool.GetConnectionAsync();
			var connection = await _connectionPool.GetConnectionAsync();

			using (var cmd = new NpgsqlCommand(spName, connection))
			{
				cmd.CommandType = CommandType.StoredProcedure;

				if (parameters != null)
				{
					cmd.Parameters.AddRange(GetNpgsqlParameters(parameters));
				}

				result = ExecuteCommand(cmd);
			}

			_connectionPool.ReturnConnectionAsync(connection);

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
		private static async Task<object> ExecuteReaderAsync(NpgsqlCommand cmd)
		{
			List<TEntity> result = new List<TEntity>();

			//using (var reader = cmd.ExecuteReader())
			using (var reader = await cmd.ExecuteReaderAsync())
			{
				//while (reader.Read())
				while (await reader.ReadAsync())
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
		private static async Task<object> ExecuteScalarAsync(NpgsqlCommand cmd)
		{
			//return cmd.ExecuteScalar();
			return await cmd.ExecuteScalarAsync();
		}
		#endregion

		#region helper methods
		// encapsulations for sp()
		protected static List<TEntity> SpExecuteReader(string spName, List<object> parameters = null)
		{
			var spResult = SpAsync(ExecuteReader, spName, parameters) as List<TEntity>;

			return spResult;
		}
		protected static async Task<List<TEntity>> SpExecuteReaderAsync(string spName, List<object> parameters = null)
		{
			//var spResult = Sp(ExecuteReader, spName, parameters) as List<TEntity>;
			var spResult = await SpAsync(ExecuteReaderAsync, spName, parameters) as List<TEntity>;

			return spResult;
		}

		protected static TEntity SpExecuteReaderReturningSingleRecord(string spName, List<object> parameters = null)
		{
			var spResult = SpExecuteReader(spName, parameters);

			return spResult.FirstOrDefault();
		}
		protected static async Task<TEntity> SpExecuteReaderReturningSingleRecordAsync(string spName, List<object> parameters = null)
		{
			//var spResult = SpExecuteReader(spName, parameters);
			var spResult = await SpExecuteReaderAsync(spName, parameters);

			return spResult.FirstOrDefault();
		}

		//protected static object SpExecuteScalar(string spName, List<object> parameters = null)
		protected static async Task<object> SpExecuteScalarAsync(string spName, List<object> parameters = null)
		{
			//return Sp(ExecuteScalar, spName, parameters);
			return await SpAsync(ExecuteScalarAsync, spName, parameters);
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
