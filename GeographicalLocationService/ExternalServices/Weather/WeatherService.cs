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
			var response = this._httpClientHelper.GetResponse<CurrentWeather>(new Uri(this._weatherServiceBaseUri));
			var currentWeathers = new Dictionary<string, CurrentWeather>();
			currentWeathers.Add(criteriaPairing, response);
			return currentWeathers;
		}
	}
}