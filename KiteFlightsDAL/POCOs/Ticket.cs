using KiteFlightsDAL.POCOs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsDAL.POCOs
{
	public class Ticket : IPoco, IEquatable<Ticket>
	{
		#region Properties
		public long Id { get; set; }
		public Flight Flight { get; set; }
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
