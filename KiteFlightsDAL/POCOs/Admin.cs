using KiteFlightsDAL.HelperClasses.CustomAttributes;
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
		[Column("id")]
		[SpParameter("_id")]
		public int Id { get; set; }
		[Column("first_name")]
		[SpParameter("_first_name")]
		public string FirstName { get; set; }
		[Column("last_name")]
		[SpParameter("_last_name")]
		public string LastName { get; set; }
		[Column("level")]
		[SpParameter("_level")]
		public int Level { get; set; }
		[Column("user")]
		[SpParameter("_user")]
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
			return other != null &&
				   Id == other.Id &&
				   FirstName == other.FirstName &&
				   LastName == other.LastName &&
				   Level == other.Level &&
				   EqualityComparer<User>.Default.Equals(User, other.User);
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
