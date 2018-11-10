namespace GeographicalLocationService.Mappers
{
	using System.Collections.Generic;
	using System.Linq;
	using GeographicalLocationService.Database;
	using GeographicalLocationService.ExternalServices.Countries;
	using GeographicalLocationService.ExternalServices.Weather;
	using MODELS = GeographicalLocationService.Models;

	public static class SearchCityResponseMapper
	{
		public static MODELS.SearchCityResponse Map(
			City city,
			Dictionary<string, Country> countries,
			Dictionary<string, CurrentWeather> currentWeathers)
		{
			var searchCityResponse = new MODELS.SearchCityResponse()
			{
				EstablishedOn = city.EstablishedOn,
				EstimatedPopulation = city.EstimatedPopulation,
				Id = city.Id,
				Name = city.Name,
				SubRegion = city.SubRegion,
				TouristRating = city.TouristRating				
			};

			if (countries != null && countries.ContainsKey(city.CountryCode))
			{
				var country = countries[city.CountryCode];

				if (country != null)
				{
					searchCityResponse.Country = new MODELS.Country()
					{
						Alpha2CountryCode = country.Alpha2Code,
						Alpha3CountryCode = country.Alpha3Code,
						Currencies = country.Currencies.Select(c => c.Code).ToList()
					};
				}
			}

			if (currentWeathers != null && currentWeathers.ContainsKey(city.CountryCode))
			{
				var currentWeather = currentWeathers[city.CountryCode];

				if (currentWeather != null)
				{
					searchCityResponse.CurrentWeather = new MODELS.CurrentWeather()
					{
						Description = currentWeather.Weather.First().Description,
						Summary = currentWeather.Weather.First().Main,
						Humidity = currentWeather.Main.Humidity,
						Pressure = currentWeather.Main.Pressure,
						Temperature = currentWeather.Main.Temp,
						WindSpeed = currentWeather.Wind.Speed,
					};
				}
			}

			return searchCityResponse;
		}
	}
}