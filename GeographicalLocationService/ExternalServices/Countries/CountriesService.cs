namespace GeographicalLocationService.ExternalServices.Countries
{
	using System;
	using System.Collections.Generic;
	using System.Web.Configuration;
	using GeographicalLocationService.Caching;
	using GeographicalLocationService.HttpClientUtilities;
	using GeographicalLocationService.Logging;

	public class CountriesService : ICountriesService
	{
		private readonly int _countryCacheTimeMinutes = int.Parse(WebConfigurationManager.AppSettings[ApplicationSettingKeyNames.CountryCacheTimeMinutes]);

		private readonly string _countriesServiceBaseUri = WebConfigurationManager.AppSettings[ApplicationSettingKeyNames.CountriesServiceBaseUri];

		private IHttpClientHelper _httpClientHelper;

		public CountriesService(IHttpClientHelper httpClientHelper)
		{
			_httpClientHelper = httpClientHelper;
		}

		public Country Get(string countryCode)
		{
			var cachedCountries = CacheUtilities.GetObjectFromCache($"CountriesCache", _countryCacheTimeMinutes, GetAllCountries);

			if (cachedCountries == null || !cachedCountries.ContainsKey(countryCode))
			{
				return null;
			}

			return cachedCountries[countryCode];
		}
		
		private Dictionary<string, Country> GetAllCountries()
		{
			try
			{
				var response = _httpClientHelper.GetResponse<List<Country>>(new Uri(this._countriesServiceBaseUri));

				var allCountries = new Dictionary<string, Country>();

				foreach (var country in response)
				{
					allCountries.Add(country.Alpha2Code, country);
				}

				return allCountries;
			}
			catch (Exception ex)
			{
				Logger.RecordMessage("An unexpected error occured retrieving Countries from the external service.");
				Logger.RecordException(ex);
				return null;
			}
		}
	}
}