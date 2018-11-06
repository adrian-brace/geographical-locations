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
		public static MODELS.SearchCityResponse Map(City city, Country country, CurrentWeather currentWeather)
		{
			var searchCityResponse = new MODELS.SearchCityResponse()
			{
				EstablishedOn = city.EstablishedOn,
				EstimatedPopulation = city.EstimatedPopulation,
				Id = city.Id,
				Name = city.Name,
				SubRegion = city.SubRegion,
				TouristRating = city.TouristRating,
				Country = new MODELS.Country()
				{
					Alpha2CountryCode = country.Alpha2Code,
					Alpha3CountryCode = country.Alpha3Code,
					Currencies = country.Currencies.Select(c => c.Code).ToList()
				},
				CurrentWeather = new MODELS.CurrentWeather()
				{
					Description = currentWeather.Weather.First().Description,
					Summary = currentWeather.Weather.First().Main,
					Humidity = currentWeather.Main.Humidity,
					Pressure = currentWeather.Main.Pressure,
					Temperature = currentWeather.Main.Temp,
					WindSpeed = currentWeather.Wind.Speed,
				}
			};

			return searchCityResponse;
		}
	}
}