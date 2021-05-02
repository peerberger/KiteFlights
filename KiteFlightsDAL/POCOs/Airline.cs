using KiteFlightsDAL.POCOs.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KiteFlightsDAL.POCOs
{
	[Table("airlines")]
	public class Airline : IPoco, IUser, IEquatable<Airline>
	{
		#region Properties
		public long Id { get; set; }
		public string Name { get; set; } = "";
		public Country Country { get; set; } = new Country();
		public User User { get; set; } = new User();
		#endregion

		#region Ctors
		public Airline()
		{
		}

		public Airline(long id, string name, Country country, User user)
		{
			Id = id;
			Name = name;
			Country = country;
			User = user;
		}
		#endregion

		#region Equals(), GetHashCode(), ==, !=
		public override bool Equals(object obj)
		{
			return Equals(obj as Airline);
		}

		public bool Equals(Airline other)
		{
			var result = other != null &&
				   Id == other.Id &&
				   Name == other.Name &&
				   EqualityComparer<Country>.Default.Equals(Country, other.Country) &&
				   EqualityComparer<User>.Default.Equals(User, other.User);

			return result;
		}

		public override int GetHashCode()
		{
			return (int)Id;
		}

		public static bool operator ==(Airline left, Airline right)
		{
			return EqualityComparer<Airline>.Default.Equals(left, right);
		}

		public static bool operator !=(Airline left, Airline right)
		{
			return !(left == right);
		}
		#endregion
	}
}
