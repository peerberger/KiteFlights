using KiteFlightsCommon.DaoInterfaces;
using KiteFlightsCommon.POCOs.Interfaces;
using KiteFlightsDAL.HelperClasses.ExtensionMethods;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsDAL.DAOs
{
	public class GenericDaoPgsql<TEntity> : BaseDaoPgsql<TEntity>, ICrudDao<TEntity> where TEntity : IPoco, new()
	{
		protected string TableName { get; set; }

		public GenericDaoPgsql(NpgsqlConnection connection) : base(connection)
		{
			try
			{
				TableName = GetTableName();
			}
			catch (Exception ex)
			{
				// todo: add logging
			}
		}
	
		#region crud methods
		// getting
		public TEntity GetById(int id)
		{
			var parameters = new List<object> { id };

			return SpExecuteReaderReturningSingleRecord($"sp_{TableName}_get_by_id", parameters);
		}

		public IList<TEntity> GetAll()
		{
			return SpExecuteReader($"sp_{TableName}_get_all");
		}

		// adding
		public int Add(TEntity entity)
		{
			var newId = -1;

			var parameters = GetSpParameters(entity);

			var spResult = SpExecuteScalar($"sp_{TableName}_add", parameters);

			newId = CheckIfSpResultNullAndReturnValue(spResult, -1);

			return newId;
		}

		// updating
		public bool Update(TEntity entity)
		{
			bool updated = false;

			try
			{
				var parameters = GetSpParameters(entity);

				var spResult = SpExecuteScalar($"sp_{TableName}_update", parameters);

				updated = CheckIfSpResultNullAndReturnValue(spResult, false);

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
		public bool Remove(int id)
		{
			bool removed = false;

			try
			{
				var parameters = new List<object> { id };

				var spResult = SpExecuteScalar($"sp_{TableName}_remove", parameters);

				removed = CheckIfSpResultNullAndReturnValue(spResult, false);

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
		#endregion

		#region helper methods
		private static string GetTableName()
		{
			if (typeof(TEntity).TryGetAttributeValue((TableAttribute tableAttribute) => tableAttribute.Name, out string tableAttributeName))
			{
				return tableAttributeName;
			}
			else
			{
				throw new Exception("No TableAttribute was found.");
			}
		}

		#region GetSpParameters() failed versions
		//private static Dictionary<string, object> GetSpParameters(TEntity entity)
		//{
		//	var parameters = new Dictionary<string, object>();

		//	foreach (var prop in entity.GetType().GetProperties())
		//	{
		//		if (prop.TryGetAttributeValue((SpParameterAttribute spParameterAttribute) => spParameterAttribute.Name, out string spParameterAttributeName))
		//		{
		//			parameters.Add(spParameterAttributeName, prop.GetValue(entity));
		//		}
		//		else
		//		{
		//			throw new Exception("No SpParameterAttribute was found.");
		//		}
		//	}

		//	return parameters;
		//}

		//private static List<object> GetSpParameters<PocoEntity>(PocoEntity entity, bool includeId) where PocoEntity : IPoco, new()
		//{
		//	var parameters = new List<object>();

		//	foreach (var prop in entity.GetType().GetProperties())
		//	{
		//		Type type = prop.PropertyType;
		//		var name = prop.Name;
		//		object value;

		//		if (name == "Id" && includeId)
		//		{
		//			var value = prop.GetValue(entity);
		//			parameters.Add(value);
		//		}
		//		else if (typeof(IPoco).IsAssignableFrom(type))
		//		{
		//			parameters.AddRange(GetSpParameters((PocoEntity)prop.GetValue(entity), includeId));
		//		}
		//		else
		//		{
		//			var value = prop.GetValue(entity);
		//			parameters.Add(value);
		//		}

		//	}

		//	return parameters;
		//}

		//private static List<object> GetSpParameters(object entity, bool includeId)
		//{
		//	var parameters = new List<object>();

		//	if (!(entity is IPoco))
		//	{
		//		parameters.Add(entity);
		//		return parameters;
		//	}

		//	foreach (var prop in entity.GetType().GetProperties())
		//	{
		//		//if (!(prop.Name == "Id" && includeId))
		//		if (prop.Name == "Id" && !includeId)
		//		{
		//			continue;
		//		}

		//		//if (entity is Country)
		//		//{
		//		//	includeId = true;
		//		//}

		//		var innerParameters = GetSpParameters(prop.GetValue(entity), includeId);

		//		parameters.AddRange(innerParameters);
		//	}

		//	return parameters;
		//}

		//private static List<object> GetSpParameters(object entity)
		//{
		//	var parameters = new List<object>();

		//	if (!(entity is IPoco))
		//	{
		//		parameters.Add(entity);
		//		return parameters;
		//	}

		//	foreach (var prop in entity.GetType().GetProperties())
		//	{
		//		if (prop.Name == "Id" )
		//		{
		//			continue;
		//		}

		//		//if (entity is Country)
		//		//{
		//		//	includeId = true;
		//		//}

		//		var innerParameters = GetSpParameters(prop.GetValue(entity), includeId);

		//		parameters.AddRange(innerParameters);
		//	}

		//	return parameters;
		//}
		#endregion
		// todo: Try maybe making GetSpParameters() more efficient, so the adding SPs won't have to receive lots of unused parameters
		private static List<object> GetSpParameters(object entity)
		{
			var parameters = new List<object>();

			if (!(entity is IPoco))
			{
				parameters.Add(entity);
				return parameters;
			}

			foreach (var prop in entity.GetType().GetProperties())
			{
				var innerParameters = GetSpParameters(prop.GetValue(entity));

				parameters.AddRange(innerParameters);
			}

			return parameters;
		}

		private static T CheckIfSpResultNullAndReturnValue<T>(object spResult, T alternativeValue)
		{
			if (spResult is long)
			{
				spResult = (int)(long)spResult;
			}

			var result = spResult != null ? (T)spResult : alternativeValue;

			return result;
		}
		#endregion
	}
}
