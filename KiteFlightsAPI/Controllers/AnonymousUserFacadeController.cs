using KiteFlightsBLL.Utilities;
using KiteFlightsCommon.DTOs.AnonymousUserFacadeControllerDTOs;
using KiteFlightsCommon.DTOs.CommonDTOs;
using KiteFlightsCommon.FacadesInterfaces;
using KiteFlightsCommon.POCOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KiteFlightsAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AnonymousUserFacadeController : ControllerBase
	{
		private readonly IAnonymousUserFacade _facade;

		public AnonymousUserFacadeController(IAnonymousUserFacade facade)
		{
			_facade = facade;
		}

		// GET: api/<AnonymousUserFacadeController>/airlines/getall
		[HttpGet("airlines/getall")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<IEnumerable<AirlineDTO>> GetAllAirlines()
		{
			try
			{
				var airlines = _facade.GetAllAirlines();

				var airlineDTOs = airlines
					.Select(a => PocoDtoConverter.AirlinePocoToDto(a));

				return Ok(airlineDTOs);
			}
			catch (Exception ex)
			{
				// todo: add logging

				return NotFound();
			}
		}

		// GET api/<AnonymousUserFacadeController>/flights/getbyid/5
		[HttpGet("flights/getbyid/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<FlightDTO> GetFlightById(int id)
		{
			try
			{
				var flight = _facade.GetFlightById(id);

				var flightDTO = PocoDtoConverter.FlightPocoToDto(flight);

				return Ok(flightDTO);
			}
			catch (Exception ex)
			{
				// todo: add logging

				return NotFound();
			}
		}

		// GET: api/<AnonymousUserFacadeController>/flights/getall
		[HttpGet("flights/getall")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<IEnumerable<FlightDTO>> GetAllFlights()
		{
			try
			{
				var flights = _facade.GetAllFlights();

				var flightDTOs = flights
					.Select(f => PocoDtoConverter.FlightPocoToDto(f));

				return Ok(flightDTOs);
			}
			catch (Exception ex)
			{
				// todo: add logging

				return NotFound();
			}
		}

		// todo: haven't implemented a method that uses AnonymousUserFacade.GetAllFlightsWithVacancies(),
		// because i don't udnerstand why it's necessary if Flight already contains the number of vacancies

		// GET: api/<AnonymousUserFacadeController>/flights/getbyorigincountry/2
		[HttpGet("flights/getbyorigincountry/{countryId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<IEnumerable<FlightDTO>> GetFlightsByOriginCountry(int countryId)
		{
			try
			{
				var flights = _facade.GetFlightsByOriginCountry(countryId);

				var flightDTOs = flights
					.Select(f => PocoDtoConverter.FlightPocoToDto(f));

				return Ok(flightDTOs);
			}
			catch (Exception ex)
			{
				// todo: add logging

				return NotFound();
			}
		}

		// GET: api/<AnonymousUserFacadeController>/flights/getbydestinationcountry/2
		[HttpGet("flights/getbydestinationcountry/{countryId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<IEnumerable<FlightDTO>> GetFlightsByDestinationCountry(int countryId)
		{
			try
			{
				var flights = _facade.GetFlightsByDestinationCountry(countryId);

				var flightDTOs = flights
					.Select(f => PocoDtoConverter.FlightPocoToDto(f));

				return Ok(flightDTOs);
			}
			catch (Exception ex)
			{
				// todo: add logging

				return NotFound();
			}
		}

		// GET: api/<AnonymousUserFacadeController>/flights/getbydepatruretime/2021-01-01T01:00:00
		[HttpGet("flights/getbydepatruretime/{departureTime}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<IEnumerable<FlightDTO>> GetFlightsByDepatrureTime(DateTime departureTime)
		{
			try
			{
				var flights = _facade.GetFlightsByDepatrureTime(departureTime);

				var flightDTOs = flights
					.Select(f => PocoDtoConverter.FlightPocoToDto(f));

				return Ok(flightDTOs);
			}
			catch (Exception ex)
			{
				// todo: add logging

				return NotFound();
			}
		}

		// GET: api/<AnonymousUserFacadeController>/flights/getbylandingtime/2021-01-01T01:00:00
		[HttpGet("flights/getbylandingtime/{landingTime}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<IEnumerable<FlightDTO>> GetFlightsByLandingTime(DateTime landingTime)
		{
			try
			{
				var flights = _facade.GetFlightsByLandingTime(landingTime);

				var flightDTOs = flights
					.Select(f => PocoDtoConverter.FlightPocoToDto(f));

				return Ok(flightDTOs);
			}
			catch (Exception ex)
			{
				// todo: add logging

				return NotFound();
			}
		}
	}
}
