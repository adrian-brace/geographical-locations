namespace GeographicalLocationService.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Net.Http;
	using System.Web.Http;
	using GeographicalLocationService.Database;
	using GeographicalLocationService.Models;

	public class CitiesController : ApiController
	{
		private readonly IGeographicalLocationsDatabase _geographicalLocationsDatabase;

		public CitiesController(IGeographicalLocationsDatabase geographicalLocationsDatabase)
		{
			this._geographicalLocationsDatabase = geographicalLocationsDatabase;
		}

		// GET api/<controller>/5
		[HttpGet]
		public string Get(int id)
		{
			throw new NotImplementedException();
		}

		// POST api/<controller>
		[HttpPost]
		public void Post([FromBody]AddCityRequest addCityRequest)
		{
			throw new NotImplementedException();
		}

		// PUT api/<controller>/5 (update)
		[HttpPut]
		public void Put(int id, [FromBody]UpdateCityRequest updateCityRequest)
		{
			throw new NotImplementedException();
		}

		// DELETE api/<controller>/5
		[HttpDelete]
		public void Delete(int id)
		{
			throw new NotImplementedException();
		}
	}
}