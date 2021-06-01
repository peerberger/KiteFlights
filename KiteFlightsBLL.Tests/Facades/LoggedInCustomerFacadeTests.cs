using KiteFlightsBLL.Auth;
using KiteFlightsBLL.Tests.CommonFixtures;
using KiteFlightsBLL.Tests.Facades.FacadesFixtures;
using KiteFlightsBLL.Tests.Utilities;
using KiteFlightsCommon.DaoInterfaces;
using KiteFlightsCommon.FacadesInterfaces;
using KiteFlightsCommon.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KiteFlightsBLL.Tests.Facades
{
	[Collection("DalFixtureCollection")]
	public class LoggedInCustomerFacadeTests : IClassFixture<LoggedInCustomerFacadeFixture>
	{
		private DalFixture _dalFixture;
		private LoggedInCustomerFacadeFixture _facadeFixture;

		private ITicketDao TicketDao { get => _dalFixture.TicketDao; }
		//private IFlightDao FlightDao { get => _dalFixture.FlightDao; }

		private ILoggedInCustomerFacade Facade { get => _facadeFixture.Facade; }

		public LoggedInCustomerFacadeTests(
			DalFixture dalFixture,
			LoggedInCustomerFacadeFixture facadeFixture)
		{
			_dalFixture = dalFixture;
			_facadeFixture = facadeFixture;

			_dalFixture.ClearAndRepopulateDb();
		}

		#region helper methods
		public static IEnumerable<object[]> GetInitialDataCustomersTokens(int skipNum, int takeNum)
		{
			var result = InitialData.Customers()
				.Skip(skipNum)
				.Take(takeNum)
				.Select(customer => new object[] { new LoginToken<Customer>(customer) });

			return result;
		}

		public static IEnumerable<object[]> GetInitialDataCustomersTokens_1()
		{
			return GetInitialDataCustomersTokens(0, 1);
		}

		public static IEnumerable<object[]> GetInitialDataCustomersTokens_2()
		{
			return GetInitialDataCustomersTokens(1, 1);
		}

		public static IEnumerable<object[]> GetInitialDataCustomersTokens_2_3()
		{
			return GetInitialDataCustomersTokens(1, 2);
		}

		public static IEnumerable<object[]> GetInitialDataCustomersTokens_1_2_3()
		{
			return GetInitialDataCustomersTokens(0, 3);
		}
		#endregion

		[Theory]
		[MemberData(nameof(GetInitialDataCustomersTokens_1_2_3))]
		public void GetAllMyFlights_Success(LoginToken<Customer> token)
		{
			// arrange
			var myTicketsFlightsIds = InitialData.Tickets()
				.Where(t => t.Customer.Id == token.User.Id)
				.Select(t => t.Flight.Id);
			var expected = InitialData.Flights()
				.Where(f => myTicketsFlightsIds.Contains(f.Id));

			// act
			var actual = Facade.GetAllMyFlights(token);

			// assert
			Assert.Equal(expected, actual);
		}

		[Theory]
		[MemberData(nameof(GetInitialDataCustomersTokens_2))]
		public void PurchaseTicket_Success(LoginToken<Customer> token)
		{
			// arrange
			var ticket = NewPocoGenerator.Ticket();

			// act
			var actual = Facade.PurchaseTicket(token, ticket.Flight);

			// assert
			ticket.Id = 4;
			var expected = ticket;

			Assert.Equal(expected, actual);
		}

		[Theory]
		[MemberData(nameof(GetInitialDataCustomersTokens_1))]
		public void CancelTicket_TicketBelongsToCustomer_Success(LoginToken<Customer> token)
		{
			// arrange
			var ticket = InitialData.Tickets().First();

			// act
			Facade.CancelTicket(token, ticket);

			// assert
			var expected = TicketDao.GetById((int)ticket.Id);
			var actual = default(Ticket);

			Assert.Equal(expected, actual);
		}

		[Theory]
		[MemberData(nameof(GetInitialDataCustomersTokens_2_3))]
		public void CancelTicket_TicketDoesNotBelongToCustomer_Failure(LoginToken<Customer> token)
		{
			// arrange
			var ticket = InitialData.Tickets().First();

			// assert
			Assert.Throws<Exception>(() => Facade.CancelTicket(token, ticket));
		}
	}
}
