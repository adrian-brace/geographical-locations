namespace GeographicalLocationService.ExternalServices.Weather
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public interface IWeatherService
	{
		List<CurrentWeather> Get(string cityName, string countryCode);
	}
}
