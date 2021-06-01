using KiteFlightsBLL.Auth;
using KiteFlightsBLL.CustomExceptions;
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
	public class LoggedInAirlineFacadeTests : IClassFixture<LoggedInAirlineFacadeFixture>
	{
		private DalFixture _dalFixture;
		private LoggedInAirlineFacadeFixture _facadeFixture;

		private IAirlineDao AirlineDao { get => _dalFixture.AirlineDao; }
		private IFlightDao FlightDao { get => _dalFixture.FlightDao; }

		private ILoggedInAirlineFacade Facade { get => _facadeFixture.Facade; }

		public LoggedInAirlineFacadeTests(
			DalFixture dalFixture,
			LoggedInAirlineFacadeFixture facadeFixture)
		{
			_dalFixture = dalFixture;
			_facadeFixture = facadeFixture;

			_dalFixture.ClearAndRepopulateDb();
		}

		#region helper methods
		public static IEnumerable<object[]> GetInitialDataAirlinesTokens(int skipNum, int takeNum)
		{
			var result = InitialData.Airlines()
				.Skip(skipNum)
				.Take(takeNum)
				.Select(airline => new object[] { new LoginToken<Airline>(airline) });

			return result;
		}

		public static IEnumerable<object[]> GetInitialDataAirlinesTokens_1()
		{
			return GetInitialDataAirlinesTokens(0, 1);
		}

		public static IEnumerable<object[]> GetInitialDataAirlinesTokens_2_3()
		{
			return GetInitialDataAirlinesTokens(1, 2);
		}

		public static IEnumerable<object[]> GetInitialDataAirlinesTokens_1_2_3()
		{
			return GetInitialDataAirlinesTokens(0,3);
		}
		#endregion

		[Theory]
		[MemberData(nameof(GetInitialDataAirlinesTokens_1_2_3))]
		public void GetAllMyTickets_Success(LoginToken<Airline> token)
		{
			// arrange
			var expected = InitialData.Tickets().Where(t => t.Flight.Airline.Id == token.User.Id);

			// act
			var actual = Facade.GetAllMyTickets(token);

			// assert
			Assert.Equal(expected, actual);
		}

		[Theory]
		[MemberData(nameof(GetInitialDataAirlinesTokens_1_2_3))]
		public void GetAllMyFlights_Success(LoginToken<Airline> token)
		{
			// arrange
			var expected = InitialData.Flights().Where(f => f.Airline.Id == token.User.Id);

			// act
			var actual = Facade.GetAllMyFlights(token);

			// assert
			Assert.Equal(expected, actual);
		}

		[Theory]
		[MemberData(nameof(GetInitialDataAirlinesTokens_1))]
		public void CreateFlight_FlightBelongsToAirline_Success(LoginToken<Airline> token)
		{
			// arrange
			var flight = NewPocoGenerator.Flight();

			// act
			var id = Facade.CreateFlight(token, flight);

			// assert
			flight.Id = 4;
			var expected = flight;
			var actual = FlightDao.GetById(id);

			Assert.Equal(expected, actual);
		}

		[Theory]
		[MemberData(nameof(GetInitialDataAirlinesTokens_2_3))]
		public void CreateFlight_FlightDoesNotBelongToAirline_Failure(LoginToken<Airline> token)
		{
			// arrange
			var flight = NewPocoGenerator.Flight();

			// assert
			Assert.Throws<FlightNotOfAirlineException>(() => Facade.CreateFlight(token, flight));
		}

		[Theory]
		[MemberData(nameof(GetInitialDataAirlinesTokens_1))]
		public void UpdateFlightDetails_FlightBelongsToAirline_Success(LoginToken<Airline> token)
		{
			// arrange
			var flight = InitialData.Flights().First();

			flight.RemainingTicketsNo = 5;

			// act
			Facade.UpdateFlightDetails(token, flight);

			// assert
			var expected = flight;
			var actual = FlightDao.GetById((int)flight.Id);

			Assert.Equal(expected, actual);
		}

		[Theory]
		[MemberData(nameof(GetInitialDataAirlinesTokens_2_3))]
		public void UpdateFlightDetails_FlightDoesNotBelongToAirline_Failure(LoginToken<Airline> token)
		{
			// arrange
			var flight = InitialData.Flights().First();

			flight.RemainingTicketsNo = 5;

			// assert
			Assert.Throws<FlightNotOfAirlineException>(() => Facade.UpdateFlightDetails(token, flight));
		}

		[Theory]
		[MemberData(nameof(GetInitialDataAirlinesTokens_1))]
		public void CancelFlight_FlightBelongsToAirline_Success(LoginToken<Airline> token)
		{
			// arrange
			var flight = InitialData.Flights().First();

			// act
			Facade.CancelFlight(token, flight);

			// assert
			var expected = FlightDao.GetById((int)flight.Id);
			var actual = default(Flight);

			Assert.Equal(expected, actual);
		}

		[Theory]
		[MemberData(nameof(GetInitialDataAirlinesTokens_2_3))]
		public void CancelFlight_FlightDoesNotBelongToAirline_Failure(LoginToken<Airline> token)
		{
			// arrange
			var flight = InitialData.Flights().First();

			// assert
			Assert.Throws<FlightNotOfAirlineException>(() => Facade.CancelFlight(token, flight));
		}

		[Theory]
		[MemberData(nameof(GetInitialDataAirlinesTokens_1))]
		public void ChangeMyPassword_Success(LoginToken<Airline> token)
		{
			// arrange
			var airline = InitialData.Airlines().First();
			airline.User.Password = "0000";

			// act
			var changeSucceeded = Facade.ChangeMyPassword(token, token.User.User.Password, "0000");

			// assert
			var expected = airline;
			var actual = AirlineDao.GetById((int)airline.Id);

			Assert.True(changeSucceeded);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[MemberData(nameof(GetInitialDataAirlinesTokens_1))]
		public void UpdateMyDetails_AirlineMatchesTokenUser_Success(LoginToken<Airline> token)
		{
			// arrange
			var airline = InitialData.Airlines().First();

			airline.Name = "Wizz Air";

			// act
			Facade.UpdateMyDetails(token, airline);

			// assert
			var expected = airline;
			var actual = AirlineDao.GetById((int)airline.Id);

			Assert.Equal(expected, actual);
		}

		[Theory]
		[MemberData(nameof(GetInitialDataAirlinesTokens_2_3))]
		public void UpdateMyDetails_AirlineDoesNotMatchTokenUser_Failure(LoginToken<Airline> token)
		{
			// arrange
			var airline = InitialData.Airlines().First();

			airline.Name = "Wizz Air";

			// assert
			Assert.Throws<Exception>(() => Facade.UpdateMyDetails(token, airline));
		}

	}
}
