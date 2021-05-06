using KiteFlightsCommon.POCOs.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KiteFlightsCommon.POCOs
{
	[Table("users")]
	public class User : IPoco, IEquatable<User>
	{
		#region Properties
		public long Id { get; set; }
		public string Username { get; set; } = "";
		public string Password { get; set; } = "";
		public string Email { get; set; } = "";
		public int UserRole { get; set; }
		#endregion

		#region Ctors
		public User()
		{
		}

		public User(long id, string username, string password, string email, int userRole)
		{
			Id = id;
			Username = username;
			Password = password;
			Email = email;
			UserRole = userRole;
		}
		#endregion

		#region Equals(), GetHashCode(), ==, !=
		public override bool Equals(object obj)
		{
			return Equals(obj as User);
		}

		public bool Equals(User other)
		{
			var result = other != null &&
				   Id == other.Id &&
				   Username == other.Username &&
				   Password == other.Password &&
				   Email == other.Email &&
				   UserRole == other.UserRole;

			return result;
		}

		public override int GetHashCode()
		{
			return (int)Id;
		}

		public static bool operator ==(User left, User right)
		{
			return EqualityComparer<User>.Default.Equals(left, right);
		}

		public static bool operator !=(User left, User right)
		{
			return !(left == right);
		}
		#endregion
	}
}
