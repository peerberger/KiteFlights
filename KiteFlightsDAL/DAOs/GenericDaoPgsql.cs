using KiteFlightsDAL.HelperClasses.ExtensionMethods;
using KiteFlightsDAL.POCOs.Interfaces;
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

		public GenericDaoPgsql(string connectionString) : base(connectionString)
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

			if (spResult is long)
			{
				spResult = (int)(long)spResult;
			}

			newId = spResult != null ? (int)spResult : newId;

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
		// todo: maybe change it so the parameter is the id to avoid the below reflection (to get the id value)
		public bool Remove(TEntity entity)
		{
			bool removed = false;

			try
			{
				var parameters = new List<object>();

				parameters.Add(entity.GetType().GetProperty("Id").GetValue(entity));

				var spResult = SpExecuteScalar($"sp_{TableName}_remove", parameters);

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
