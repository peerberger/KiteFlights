using KiteFlightsDAL.HelperClasses.CustomAttributes;
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

		private static Dictionary<string, object> GetSpParameters(TEntity entity)
		{
			var parameters = new Dictionary<string, object>();

			foreach (var prop in entity.GetType().GetProperties())
			{
				if (prop.TryGetAttributeValue((SpParameterAttribute spParameterAttribute) => spParameterAttribute.Name, out string spParameterAttributeName))
				{
					parameters.Add(spParameterAttributeName, prop.GetValue(entity));
				}
				else
				{
					throw new Exception("No SpParameterAttribute was found.");
				}
			}

			return parameters;
		}

		// getting
		public TEntity GetById(int id)
		{
			TEntity entity = default;

			try
			{
				var parameters = new Dictionary<string, object>();

				parameters.Add("_id", id);

				var spResult = SpExecuteReader($"sp_{TableName}_get_by_id", parameters);

				// check if any records were found
				if (spResult.Count > 0)
				{
					entity = spResult.First();
				}
				else
				{
					throw new ArgumentException("No record that matched the Id was found.");
				}
			}
			catch (Exception ex)
			{
				// todo: add logging
			}

			return entity;
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

			// removes Id because sp_{TableName}_add() doesn't have an _id parameter
			parameters.Remove("_id");

			var spResult = SpExecuteScalar($"sp_{TableName}_add", parameters);

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
		public bool Remove(TEntity entity)
		{
			bool removed = false;

			try
			{
				var parameters = new Dictionary<string, object>();

				parameters.Add("_id", entity.GetType().GetProperty("Id").GetValue(entity));

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
