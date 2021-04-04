using KiteFlightsDAL.HelperClasses.ExtensionMethods;
using KiteFlightsDAL.POCOs;
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

		// todo: make all possible members static??
		public GenericDaoPgsql(string connectionString) : base(connectionString)
		{
			TableName = GetTableName();
		}

		private static string GetTableName()
		{
			if (typeof(TEntity).TryGetAttributeValue((TableAttribute tableAttribute) => tableAttribute.Name, out string tableAttributeName))
			{
				return tableAttributeName;
			}
			else
			{
				throw new Exception("No table name was found.");
			}
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




			dynamic parameters = new ExpandoObject();
			var paramsDict = (IDictionary<string, object>)parameters;

			
			//paramsDict.Add("_id", id);
			//paramsDict.Add("_id", id);




			var spResult = SpExecuteScalar($"sp_{TableName}_add", parameters);

			newId = spResult != null ? (int)spResult : newId;

			return newId;

			throw new NotImplementedException();
		}

		// updating
		public bool Update(TEntity entity)
		{
			throw new NotImplementedException();
		}

		// removing
		public bool Remove(TEntity entity)
		{
			throw new NotImplementedException();
		}

	}
}
