namespace GeographicalLocationService.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Net.Http;
	using System.Web.Http;
	using GeographicalLocationService.Database;
	using GeographicalLocationService.ExternalServices.Countries;
	using GeographicalLocationService.ExternalServices.Weather;
	using GeographicalLocationService.Models;

	public class CitiesController : ApiController
	{
		private readonly IGeographicalLocationsDatabase _geographicalLocationsDatabase;

		private readonly ICountriesService _countriesService;

		private readonly IWeatherService _weatherService;

		public CitiesController(
			IGeographicalLocationsDatabase geographicalLocationsDatabase,
			ICountriesService countriesService,
			IWeatherService weatherService)
		{
			this._geographicalLocationsDatabase = geographicalLocationsDatabase;
			this._countriesService = countriesService;
			this._weatherService = weatherService;
		}

		// GET api/<controller>/{name}
		[HttpGet]
		public SearchCityResponse Get(string name)
		{
			throw new NotImplementedException();
		}

		// POST api/<controller>
		[HttpPost]
		public int Post([FromBody]AddCityRequest addCityRequest)
		{
			throw new NotImplementedException();
		}

		// PUT api/<controller>/{id} (update)
		[HttpPut]
		public bool Put(int id, [FromBody]UpdateCityRequest updateCityRequest)
		{
			throw new NotImplementedException();
		}

		// DELETE api/<controller>/{id}
		[HttpDelete]
		public bool Delete(int id)
		{
			throw new NotImplementedException();
		}
	}
}