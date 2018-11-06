namespace GeographicalLocationService.ExternalServices.Weather
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;

	public class WeatherService : IWeatherService
	{
		public List<CurrentWeather> Get(string cityName, string countryCode)
		{
			throw new NotImplementedException();
		}
	}
}