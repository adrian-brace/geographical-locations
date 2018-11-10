namespace GeographicalLocationService.ExternalServices.Weather
{
	using System;
	using System.Collections.Generic;
	using System.Web.Configuration;
	using GeographicalLocationService.Caching;
	using GeographicalLocationService.HttpClientUtilities;
	using GeographicalLocationService.Logging;

	public class WeatherService : IWeatherService
	{
		private readonly int _currentWeatherCacheTimeMinutes = int.Parse(WebConfigurationManager.AppSettings[ApplicationSettingKeyNames.CurrentWeatherCacheTimeMinutes]);

		private readonly string _weatherServiceBaseUri = WebConfigurationManager.AppSettings[ApplicationSettingKeyNames.WeatherServiceBaseUri];

		private readonly string _openWeatherMapApiKey = WebConfigurationManager.AppSettings[ApplicationSettingKeyNames.OpenWeatherMapApiKey];		

		private IHttpClientHelper _httpClientHelper;

		public WeatherService(IHttpClientHelper httpClientHelper)
		{
			_httpClientHelper = httpClientHelper;
		}

		public CurrentWeather Get(string cityName, string countryCode)
		{
			var criteriaPairing = $"{cityName},{countryCode}";

			var cachedCurrentWeather = CacheUtilities.GetObjectFromCache($"WeatherCache{criteriaPairing}", _currentWeatherCacheTimeMinutes, GetWeather, criteriaPairing);

			if (cachedCurrentWeather == null || !cachedCurrentWeather.ContainsKey(criteriaPairing))
			{
				return null;
			}

			return cachedCurrentWeather[criteriaPairing];
		}

		private Dictionary<string, CurrentWeather> GetWeather(string criteriaPairing)
		{
			try
			{
				var uri = new Uri($"{_weatherServiceBaseUri}?q={criteriaPairing}&appid={_openWeatherMapApiKey}");
				var response = _httpClientHelper.GetResponse<CurrentWeather>(uri);
				var currentWeathers = new Dictionary<string, CurrentWeather>();
				currentWeathers.Add(criteriaPairing, response);
				return currentWeathers;
			}
			catch (Exception ex)
			{
				Logger.RecordMessage("An unexpected error occured retrieving Current Weather from the external service.");
				Logger.RecordException(ex);
				return null;
			}
}
	}
}