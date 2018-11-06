namespace GeographicalLocationService.ExternalServices.Weather
{
	using System;
	using System.Collections.Generic;
	using System.Web.Configuration;
	using GeographicalLocationService.Caching;
	using GeographicalLocationService.HttpClientUtilities;
	using Newtonsoft.Json.Linq;

	public class WeatherService : IWeatherService
	{
		private readonly int _currentWeatherCacheTimeMinutes = int.Parse(WebConfigurationManager.AppSettings[ApplicationSettingKeyNames.CurrentWeatherCacheTimeMinutes]);

		private readonly string _weatherServiceBaseUri = WebConfigurationManager.AppSettings[ApplicationSettingKeyNames.WeatherServiceBaseUri];

		private IHttpClientHelper _httpClientHelper;

		public WeatherService(IHttpClientHelper httpClientHelper)
		{
			this._httpClientHelper = httpClientHelper;
		}

		public CurrentWeather Get(string cityName, string countryCode)
		{
			var criteriaPairing = $"{cityName},{countryCode}";

			var cachedCurrentWeather = CacheUtilities.GetObjectFromCache($"WeatherCache{criteriaPairing}", this._currentWeatherCacheTimeMinutes, this.GetWeather, criteriaPairing);

			if (!cachedCurrentWeather.ContainsKey(criteriaPairing))
			{
				return null;
			}

			return cachedCurrentWeather[criteriaPairing];
		}

		private Dictionary<string, CurrentWeather> GetWeather(string criteriaPairing)
		{
			var response = this._httpClientHelper.GetResult(new Uri(this._weatherServiceBaseUri));
			dynamic currentWeatherResponse = JObject.Parse(response);

			var currentWeathers = new Dictionary<string, CurrentWeather>();

			if (currentWeatherResponse != null)
			{
				dynamic weather = currentWeatherResponse["weather"];
				dynamic main = currentWeatherResponse["main"];
				dynamic wind = currentWeatherResponse["wind"];
				var currentWeather = new CurrentWeather();

				if (weather != null)
				{
					currentWeather.Summary = weather["main"];
					currentWeather.Description = weather["description"];
				}

				if (main != null)
				{
					currentWeather.Temperature = main["temp"];
					currentWeather.Pressure = main["pressure"];
					currentWeather.Humidity = main["humidity"];				
				}

				if (wind != null)
				{
					currentWeather.WindSpeed = wind["speed"];
				}

				currentWeathers.Add(criteriaPairing, currentWeather);
			}

			return currentWeathers;
		}
	}
}