using KiteFlightsDAL.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsDAL.DAOs.UserDao
{
	public class UserDaoPgsql : BaseDaoPgsql<User>, IUserDao
	{
		public UserDaoPgsql(string connectionString) : base(connectionString)
		{

		}

		// getting
		public User GetById(int id)
		{
			User user = null;

			try
			{
				var spResult = SpExecuteReader("sp_users_get_by_id", new
				{
					_id = id
				});

				// check if any records were found
				if (spResult.Count > 0)
				{
					user = spResult.First();
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

			return user;
		}

		public IList<User> GetAll()
		{
			return SpExecuteReader("sp_users_get_all");
		}

		// adding
		public int Add(User entity)
		{
			var newId = -1;

			var spResult = SpExecuteScalar("sp_users_add", new
			{
				_username = entity.Username,
				_password = entity.Password,
				_email = entity.Email,
				_user_role = entity.UserRole
			});

			//newId = spResult != null ? (int)(long)spResult : newId;
			newId = spResult != null ? Convert.ToInt32(spResult) : newId;

			return newId;
		}

		// updating
		public bool Update(User entity)
		{
			bool updated = false;

			try
			{
				var spResult = SpExecuteScalar("sp_users_update", new
				{
					_id = entity.Id,
					_username = entity.Username,
					_password = entity.Password,
					_email = entity.Email,
					_user_role = entity.UserRole
				});

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
		public bool Remove(User entity)
		{
			bool removed = false;

			try
			{
				var spResult = SpExecuteScalar("sp_users_remove", new { _id = entity.Id });

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
