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
	[Table("tickets")]
	public class Ticket : IPoco, IEquatable<Ticket>
	{
		#region Properties
		[Column("id")]
		[SpParameter("_id")]
		public long Id { get; set; }
		[Column("flight")]
		[SpParameter("_flight")]
		public Flight Flight { get; set; }
		[Column("customer")]
		[SpParameter("_customer")]
		public Customer Customer { get; set; }
		#endregion

		#region Ctors
		public Ticket()
		{
		}

		public Ticket(int id, Flight flight, Customer customer)
		{
			Id = id;
			Flight = flight;
			Customer = customer;
		}
		#endregion

		#region Equals(), GetHashCode(), ==, !=
		public override bool Equals(object obj)
		{
			return Equals(obj as Ticket);
		}

		public bool Equals(Ticket other)
		{
			return other != null &&
				   Id == other.Id &&
				   EqualityComparer<Flight>.Default.Equals(Flight, other.Flight) &&
				   EqualityComparer<Customer>.Default.Equals(Customer, other.Customer);
		}

		public override int GetHashCode()
		{
			return (int)Id;
		}

		public static bool operator ==(Ticket left, Ticket right)
		{
			return EqualityComparer<Ticket>.Default.Equals(left, right);
		}

		public static bool operator !=(Ticket left, Ticket right)
		{
			return !(left == right);
		}
		#endregion
	}
}
