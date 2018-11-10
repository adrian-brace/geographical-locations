namespace GeographicalLocationService.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Net.Http;
	using System.Threading.Tasks;
	using System.Web.Http;
	using AutoMapper;
	using GeographicalLocationService.Database;
	using GeographicalLocationService.ExternalServices.Countries;
	using GeographicalLocationService.ExternalServices.Weather;
	using GeographicalLocationService.Logging;
	using GeographicalLocationService.Mappers;
	using MODELS = Models;

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
			_geographicalLocationsDatabase = geographicalLocationsDatabase;
			_countriesService = countriesService;
			_weatherService = weatherService;
		}

		/// <summary>
		/// Add a new City
		/// </summary>
		/// <param name="addCityRequest">Add city request</param>
		/// <returns>City ID</returns>
		[HttpPost]
		public MODELS.AddCityResponse Add([FromBody]MODELS.AddCityRequest addCityRequest)
		{
			City cityToAdd = Mapper.Map<MODELS.AddCityRequest, City>(addCityRequest);

			// Validate the country code passed in against all country codes on the external service
			if (_countriesService.Get(cityToAdd.CountryCode) == null)
			{
				var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
				{
					Content = new StringContent($"Country Code {cityToAdd.CountryCode} was not recognised as a valid country code or there is a problem with the external Countries Service."),
					ReasonPhrase = "Country Code Invalid"
				};
				throw new HttpResponseException(resp);
			}

			var addedCity = _geographicalLocationsDatabase.Cities.Add(cityToAdd);
			_geographicalLocationsDatabase.SaveChanges();

			return new MODELS.AddCityResponse()
			{
				Id = addedCity.Id,
				SearchUri = $"~/api/{typeof(CitiesController).Name.Replace("Controller", string.Empty)}/{addedCity.Name}",
			};
		}

		/// <summary>
		/// Update an existing City
		/// </summary>
		/// <param name="id">City ID</param>
		/// <param name="updateCityRequest">Update city request</param>
		/// <returns>Number of rows affected</returns>
		[HttpPut]
		public int Update(int id, [FromBody]MODELS.UpdateCityRequest updateCityRequest)
		{
			var cityToUpdate = GetCity(id);

			cityToUpdate.EstablishedOn = !string.IsNullOrEmpty(updateCityRequest.EstablishedOn) ? DateTime.Parse(updateCityRequest.EstablishedOn) : (DateTime?)null;
			cityToUpdate.EstimatedPopulation = updateCityRequest.EstimatedPopulation;
			cityToUpdate.TouristRating = updateCityRequest.TouristRating;

			return _geographicalLocationsDatabase.SaveChanges();
		}

		/// <summary>
		/// Delete an existing City
		/// </summary>
		/// <param name="id">City ID</param>
		/// <returns>Number of rows affected</returns>
		[HttpDelete]
		public int Delete(int id)
		{
			var cityToDelete = GetCity(id);
			_geographicalLocationsDatabase.Cities.Remove(cityToDelete);
			return _geographicalLocationsDatabase.SaveChanges();
		}

		/// <summary>
		/// Search for Cities by name
		/// </summary>
		/// <param name="name">City name</param>
		/// <returns>List of search city responses</returns>
		[HttpGet]
		public List<MODELS.SearchCityResponse> Search(string name)
		{
			var matchingCities = GetCities(name);
			var distinctCountryCodes = matchingCities.Select(c => c.CountryCode).Distinct().ToList();

			var countriesDictionary = new Dictionary<string, Country>();
			var weathersDictionary = new Dictionary<string, CurrentWeather>();

			var tasks = CallExternalServices(name, distinctCountryCodes, countriesDictionary, weathersDictionary);

			TaskUtilities.WaitForTasks(tasks);

			var searchCityResponses = new List<MODELS.SearchCityResponse>();

			matchingCities.ForEach(matchedCity =>
			{
				var searchCityResponse = SearchCityResponseMapper.Map(
					matchedCity,
					countriesDictionary,
					weathersDictionary);

				searchCityResponses.Add(searchCityResponse);
			});

			return searchCityResponses;
		}

		internal City GetCity(int id)
		{
			var city = _geographicalLocationsDatabase.Cities.Where(c => c.Id == id).FirstOrDefault();

			if (city == null)
			{
				var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
				{
					Content = new StringContent($"No city with ID {id}"),
					ReasonPhrase = "City ID Not Found"
				};
				throw new HttpResponseException(resp);
			}

			return city;
		}

		internal List<City> GetCities(string name)
		{
			var matchingCities = _geographicalLocationsDatabase.Cities.Where(c => c.Name == name).ToList();

			if (matchingCities == null || matchingCities.Count == 0)
			{
				var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
				{
					Content = new StringContent($"There are no cities with the name {name}"),
					ReasonPhrase = "City Name Returned No Matches"
				};
				throw new HttpResponseException(resp);
			}

			return matchingCities;
		}		

		private List<Task> CallExternalServices(
			string name,
			List<string> distinctCountryCodes,
			Dictionary<string, Country> countriesDictionary,
			Dictionary<string, CurrentWeather> weathersDictionary)
		{
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

			return tasks;
		}
	}
}