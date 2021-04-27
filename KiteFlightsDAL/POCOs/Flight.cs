using KiteFlightsDAL.POCOs.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KiteFlightsDAL.POCOs
{
	[Table("flights")]
	public class Flight : IPoco, IEquatable<Flight>
	{
		#region Properties
		public long Id { get; set; }
		public Airline Airline { get; set; }
		public Country OriginCountry { get; set; }
		public Country DestinationCountry { get; set; }
		public DateTime DepartureTime { get; set; }
		public DateTime LandingTime { get; set; }
		public int RemainingTicketsNo { get; set; }
		#endregion

		#region Ctors
		public Flight()
		{
		}

		public Flight(int id, Airline airline, Country originCountry, Country destinationCountry, DateTime departureTime, DateTime landingTime, int remainingTicketsNo)
		{
			Id = id;
			Airline = airline;
			OriginCountry = originCountry;
			DestinationCountry = destinationCountry;
			DepartureTime = departureTime;
			LandingTime = landingTime;
			RemainingTicketsNo = remainingTicketsNo;
		}
		#endregion

		#region Equals(), GetHashCode(), ==, !=
		public override bool Equals(object obj)
		{
			return Equals(obj as Flight);
		}

		public bool Equals(Flight other)
		{
			var result = other != null &&
				   Id == other.Id &&
				   EqualityComparer<Airline>.Default.Equals(Airline, other.Airline) &&
				   EqualityComparer<Country>.Default.Equals(OriginCountry, other.OriginCountry) &&
				   EqualityComparer<Country>.Default.Equals(DestinationCountry, other.DestinationCountry) &&
				   DepartureTime == other.DepartureTime &&
				   LandingTime == other.LandingTime &&
				   RemainingTicketsNo == other.RemainingTicketsNo;

			return result;
		}

		public override int GetHashCode()
		{
			return (int)Id;
		}

		public static bool operator ==(Flight left, Flight right)
		{
			return EqualityComparer<Flight>.Default.Equals(left, right);
		}

		public static bool operator !=(Flight left, Flight right)
		{
			return !(left == right);
		}
		#endregion
	}
}