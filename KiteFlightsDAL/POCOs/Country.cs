using KiteFlightsDAL.POCOs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsDAL.POCOs
{
	public class Country : IPoco, IEquatable<Country>
	{
		#region Properties
		public int Id { get; set; }
		public string Name { get; set; }
		#endregion

		#region Ctors
		public Country()
		{
		}

		public Country(int id, string name)
		{
			Id = id;
			Name = name;
		}
		#endregion

		#region Equals(), GetHashCode(), ==, !=
		public override bool Equals(object obj)
		{
			return Equals(obj as Country);
		}

		public bool Equals(Country other)
		{
			return other != null &&
				   Id == other.Id &&
				   Name == other.Name;
		}

		public override int GetHashCode()
		{
			return Id;
		}

		public static bool operator ==(Country left, Country right)
		{
			return EqualityComparer<Country>.Default.Equals(left, right);
		}

		public static bool operator !=(Country left, Country right)
		{
			return !(left == right);
		}
		#endregion
	}
}
