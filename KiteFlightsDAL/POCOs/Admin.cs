using KiteFlightsDAL.POCOs.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsDAL.POCOs
{
	[Table("admins")]
	public class Admin : IPoco, IUser, IEquatable<Admin>
	{
		#region Properties
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public int Level { get; set; }
		public User User { get; set; }
		#endregion

		#region Ctors
		public Admin()
		{
		}

		public Admin(int id, string firstName, string lastName, int level, User user)
		{
			Id = id;
			FirstName = firstName;
			LastName = lastName;
			Level = level;
			User = user;
		}
		#endregion

		#region Equals(), GetHashCode(), ==, !=
		public override bool Equals(object obj)
		{
			return Equals(obj as Admin);
		}

		public bool Equals(Admin other)
		{
			var result = other != null &&
				   Id == other.Id &&
				   FirstName == other.FirstName &&
				   LastName == other.LastName &&
				   Level == other.Level &&
				   EqualityComparer<User>.Default.Equals(User, other.User);

			return result;
		}

		public override int GetHashCode()
		{
			return Id;
		}

		public static bool operator ==(Admin left, Admin right)
		{
			return EqualityComparer<Admin>.Default.Equals(left, right);
		}

		public static bool operator !=(Admin left, Admin right)
		{
			return !(left == right);
		}
		#endregion
	}
}
