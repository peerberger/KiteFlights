using KiteFlightsDAL.HelperClasses.CustomAttributes;
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
		[Column("id")]
		[SpParameter("_id")]
		public long Id { get; set; }
		[Column("airline")]
		[SpParameter("_airline")]
		public Airline Airline { get; set; }
		[Column("origin_country")]
		[SpParameter("_origin_country")]
		public Country OriginCountry { get; set; }
		[Column("destination_country")]
		[SpParameter("_destination_country")]
		public Country DestinationCountry { get; set; }
		[Column("departure_time")]
		[SpParameter("_departure_time")]
		public DateTime DepartureTime { get; set; }
		[Column("landing_time")]
		[SpParameter("_landing_time")]
		public DateTime LandingTime { get; set; }
		[Column("remaining_tickets_no")]
		[SpParameter("_remaining_tickets_no")]
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
			return other != null &&
				   Id == other.Id &&
				   EqualityComparer<Airline>.Default.Equals(Airline, other.Airline) &&
				   EqualityComparer<Country>.Default.Equals(OriginCountry, other.OriginCountry) &&
				   EqualityComparer<Country>.Default.Equals(DestinationCountry, other.DestinationCountry) &&
				   DepartureTime == other.DepartureTime &&
				   LandingTime == other.LandingTime &&
				   RemainingTicketsNo == other.RemainingTicketsNo;
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