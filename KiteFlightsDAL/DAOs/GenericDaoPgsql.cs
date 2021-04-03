using KiteFlightsDAL.HelperClasses.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsDAL.DAOs
{
	public class GenericDaoPgsql<TEntity> : BaseDaoPgsql<TEntity> where TEntity : new()
	{
		public GenericDaoPgsql(string connectionString) : base(connectionString)
		{

		}

		// getting
		public TEntity GetById(int id)
		{
			TEntity entity = default;

			try
			{
				string tableName;

				if (typeof(TEntity).TryGetAttributeValue((TableAttribute tableAttribute) => tableAttribute.Name, out string tableAttributeName))
				{
					tableName = tableAttributeName;
				}
				else
				{
					throw new Exception("No table name was found.");
				}




				dynamic parameters = new ExpandoObject();
				var paramsDict =  (IDictionary<string, object>)parameters;

				paramsDict.Add("_id", id);

				var spResult = SpExecuteReader($"sp_{tableName}_get_by_id", parameters) as List<TEntity>;

				




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
			throw new NotImplementedException();
		}

		// adding
		public int Add(TEntity entity)
		{
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
