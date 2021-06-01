using KiteFlightsCommon.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsBLL.Tests.Utilities
{
	public static class NewPocoGenerator
	{
		public static Admin Admin()
		{
			return new Admin
			{
				FirstName = "Bill",
				LastName = "Gaits",
				Level = 1,
				User = new User
				{
					Username = "billgaits",
					Password = "admin4",
					Email = "billgaits@gmail.com",
					UserRole = 3
				}
			};
		}

		public static Customer Customer()
		{
			return new Customer
			{
				FirstName = "Bob",
				LastName = "Bobson",
				Address = "4th st. City-ville",
				PhoneNo = "444-444-4444",
				CreditCardNo = "4444-4444-4444-4444",
				User = new User
				{
					Username = "bobbobson",
					Password = "customer4",
					Email = "bobbobson@gmail.com",
					UserRole = 1
				}
			};
		}

		public static Airline Airline()
		{
			return new Airline
			{
				Name = "Wizz Air",
				Country = new Country
				{
					Id = 3,
					Name = "UK"
				},
				User = new User
				{
					Username = "wizzair",
					Password = "airline4",
					Email = "wizzair@gmail.com",
					UserRole = 2
				}
			};
		}

		public static Flight Flight()
		{
			return new Flight
			{
				Airline = new Airline
				{
					Id = 1,
					Name = "El Al",
					Country = new Country
					{
						Id = 1,
						Name = "Israel"
					},
					User = new User
					{
						Id = 7,
						Username = "elal",
						Password = "airline1",
						Email = "elal@gmail.com",
						UserRole = 2
					}
				},
				OriginCountry = new Country
				{
					Id = 1,
					Name = "Israel"
				},
				DestinationCountry = new Country
				{
					Id = 2,
					Name = "USA"
				},
				DepartureTime = new DateTime(2021, 1, 1),
				LandingTime = new DateTime(2021, 1, 1),
				RemainingTicketsNo = 40
			};
		}

		public static Ticket Ticket()
		{
			return new Ticket
			{
				Flight = new Flight
				{
					Id = 1,
					Airline = new Airline
					{
						Id = 1,
						Name = "El Al",
						Country = new Country
						{
							Id = 1,
							Name = "Israel"
						},
						User = new User
						{
							Id = 7,
							Username = "elal",
							Password = "airline1",
							Email = "elal@gmail.com",
							UserRole = 2
						}
					},
					OriginCountry = new Country
					{
						Id = 1,
						Name = "Israel"
					},
					DestinationCountry = new Country
					{
						Id = 2,
						Name = "USA"
					},
					DepartureTime = new DateTime(2021, 1, 1),
					LandingTime = new DateTime(2021, 1, 1, 1, 0, 0),
					RemainingTicketsNo = 10
				},
				Customer = new Customer
				{
					Id = 2,
					FirstName = "Kile",
					LastName = "Kilometer",
					Address = "2nd st. City-ville",
					PhoneNo = "222-222-2222",
					CreditCardNo = "2222-2222-2222-2222",
					User = new User
					{
						Id = 5,
						Username = "kilekilometer",
						Password = "customer2",
						Email = "kilekilometer@gmail.com",
						UserRole = 1
					}
				}
			};
		}
	}
}