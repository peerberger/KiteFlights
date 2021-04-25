using KiteFlightsDAL.HelperClasses.CustomAttributes;
using KiteFlightsDAL.POCOs.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KiteFlightsDAL.POCOs
{
	[Table("customers")]
	public class Customer : IPoco, IUser, IEquatable<Customer>
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
		[Column("address")]
		[SpParameter("_address")]
		public string Address { get; set; }
		[Column("phone_no")]
		[SpParameter("_phone_no")]
		public string PhoneNo { get; set; }
		[Column("credit_card_no")]
		[SpParameter("_credit_card_no")]
		public string CreditCardNo { get; set; }
		[Column("user")]
		[SpParameter("_user")]
		public User User { get; set; }
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
			return other != null &&
				   Id == other.Id &&
				   FirstName == other.FirstName &&
				   LastName == other.LastName &&
				   Address == other.Address &&
				   PhoneNo == other.PhoneNo &&
				   CreditCardNo == other.CreditCardNo &&
				   EqualityComparer<User>.Default.Equals(User, other.User);
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