using KiteFlightsCommon.FacadesInterfaces;
using KiteFlightsCommon.POCOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
		//public async Task<ActionResult<IList<Airline>>> GetAllAirlinesAsync()
		public ActionResult<IList<Airline>> GetAllAirlines()
		{
			//return await _facade.GetAllAirlines();

			return _facade.GetAllAirlines().ToList();
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
