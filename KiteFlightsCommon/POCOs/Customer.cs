using KiteFlightsCommon.POCOs.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KiteFlightsCommon.POCOs
{
	[Table("customers")]
	public class Customer : IPoco, IUser, IEquatable<Customer>
	{
		#region Properties
		public long Id { get; set; }
		public string FirstName { get; set; } = "";
		public string LastName { get; set; } = "";
		public string Address { get; set; } = "";
		public string PhoneNo { get; set; } = "";
		public string CreditCardNo { get; set; } = "";
		public User User { get; set; } = new User();
		#endregion

		#region Ctors
		public Customer()
		{
		}

		public Customer(int id, string firstName, string lastName, string address, string phoneNo, string creditCardNo, User user)
		{
			Id = id;
			FirstName = firstName;
			LastName = lastName;
			Address = address;
			PhoneNo = phoneNo;
			CreditCardNo = creditCardNo;
			User = user;
		}
		#endregion

		#region Equals(), GetHashCode(), ==, !=
		public override bool Equals(object obj)
		{
			return Equals(obj as Customer);
		}

		public bool Equals(Customer other)
		{
			var result = other != null &&
				   Id == other.Id &&
				   FirstName == other.FirstName &&
				   LastName == other.LastName &&
				   Address == other.Address &&
				   PhoneNo == other.PhoneNo &&
				   CreditCardNo == other.CreditCardNo &&
				   EqualityComparer<User>.Default.Equals(User, other.User);

			return result;
		}

		public override int GetHashCode()
		{
			return (int)Id;
		}

		public static bool operator ==(Customer left, Customer right)
		{
			return EqualityComparer<Customer>.Default.Equals(left, right);
		}

		public static bool operator !=(Customer left, Customer right)
		{
			return !(left == right);
		}
		#endregion
	}
}