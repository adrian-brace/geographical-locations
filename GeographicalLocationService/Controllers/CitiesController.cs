namespace GeographicalLocationService.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Net.Http;
	using System.Threading.Tasks;
	using System.Web.Http;
	using GeographicalLocationService.Database;
	using GeographicalLocationService.ExternalServices.Countries;
	using GeographicalLocationService.ExternalServices.Weather;
	using GeographicalLocationService.Mappers;
	using MODELS = GeographicalLocationService.Models;

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

		[HttpGet]
		public List<MODELS.SearchCityResponse> Search(string name)
		{
			var searchCityResponses = new List<MODELS.SearchCityResponse>();

			var matchingCities = this._geographicalLocationsDatabase.Cities.Where(c => c.Name == name).ToList();

			if (matchingCities == null || matchingCities.Count == 0)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound);
			}

			var distinctCountryCodes = matchingCities.Select(c => c.CountryCode).Distinct().ToList();

			var countriesDictionary = new Dictionary<string, Country>();
			var weathersDictionary = new Dictionary<string, CurrentWeather>();

			var tasks = new List<Task>();

			distinctCountryCodes.ForEach(countryCode =>
			{
				tasks.Add(Task.Factory.StartNew(() =>
				{
					countriesDictionary.Add(countryCode, _countriesService.Get(countryCode));
				}));

				tasks.Add(Task.Factory.StartNew(() =>
				{
					weathersDictionary.Add(countryCode, _weatherService.Get(name, countryCode));
				}));
			});

			TaskUtilities.WaitForTasks(tasks);

			matchingCities.ForEach(matchedCity =>
			{
				var searchCityResponse = SearchCityResponseMapper.Map(
					matchedCity,
					countriesDictionary[matchedCity.CountryCode],
					weathersDictionary[matchedCity.CountryCode]);

				searchCityResponses.Add(searchCityResponse);
			});

			return searchCityResponses;
		}

		[HttpPost]
		public int Add([FromBody]MODELS.AddCityRequest addCityRequest)
		{
			this._geographicalLocationsDatabase.Cities.Add(new City()
			{
				CountryCode = addCityRequest.Country,
				EstablishedOn = addCityRequest.EstablishedOn,
				EstimatedPopulation = addCityRequest.EstimatedPopulation,
				Name = addCityRequest.Name,
				SubRegion = addCityRequest.SubRegion,
				TouristRating = addCityRequest.TouristRating
			});

			return this._geographicalLocationsDatabase.SaveChanges();
		}

		[HttpPut]
		public bool Update(int id, [FromBody]MODELS.UpdateCityRequest updateCityRequest)
		{
			var cityToUpdate = this.GetCity(id);

			cityToUpdate.EstablishedOn = updateCityRequest.EstablishedOn;
			cityToUpdate.EstimatedPopulation = updateCityRequest.EstimatedPopulation;
			cityToUpdate.TouristRating = updateCityRequest.TouristRating;

			return this._geographicalLocationsDatabase.SaveChanges() > 0;
		}

		[HttpDelete]
		public bool Delete(int id)
		{
			var cityToDelete = this.GetCity(id);

			this._geographicalLocationsDatabase.Cities.Remove(cityToDelete);
			return this._geographicalLocationsDatabase.SaveChanges() > 0;
		}

		private City GetCity(int id)
		{
			var cityToDelete = this._geographicalLocationsDatabase.Cities.Where(city => city.Id == id).FirstOrDefault();

			if (cityToDelete == null)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound);
			}

			return cityToDelete;
		}
	}
}