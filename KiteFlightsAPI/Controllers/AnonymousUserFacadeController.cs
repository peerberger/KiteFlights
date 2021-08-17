﻿using KiteFlightsBLL.Utilities;
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

		// GET: api/<AnonymousUserFacadeController>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<IList<AirlineDTO>> GetAllAirlines()
		{
			try
			{
				var airlines = _facade.GetAllAirlines().ToList();

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

		// GET api/<AnonymousUserFacadeController>/5
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<FlightDTO> GetFlightById(int id)
		{
			try
			{
				var flight = _facade.GetFlightById(id);

				var flightDTO = new FlightDTO
				{
					Id = flight.Id,
					Airline = PocoDtoConverter.AirlinePocoToDto(flight.Airline),
					OriginCountry = PocoDtoConverter.CountryPocoToDto(flight.OriginCountry),
					DestinationCountry = PocoDtoConverter.CountryPocoToDto(flight.DestinationCountry),
					DepartureTime = flight.DepartureTime,
					LandingTime = flight.LandingTime,
					RemainingTicketsNo = flight.RemainingTicketsNo
				};

				return Ok(flightDTO);
			}
			catch (Exception ex)
			{
				// todo: add logging

				return NotFound();
			}
		}









		//// GET api/<AnonymousUserFacadeController>/5
		//[HttpGet("{id}")]
		//public string Get(int id)
		//{
		//	return "value";
		//}

		//// POST api/<AnonymousUserFacadeController>
		//[HttpPost]
		//public void Post([FromBody] string value)
		//{
		//}

		//// PUT api/<AnonymousUserFacadeController>/5
		//[HttpPut("{id}")]
		//public void Put(int id, [FromBody] string value)
		//{
		//}

		//// DELETE api/<AnonymousUserFacadeController>/5
		//[HttpDelete("{id}")]
		//public void Delete(int id)
		//{
		//}
	}
}
