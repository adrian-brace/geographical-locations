namespace GeographicalLocationService.ExternalServices.Countries
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using System.Web.Configuration;
	using GeographicalLocationService.Caching;
	using GeographicalLocationService.HttpClientUtilities;
	using Newtonsoft.Json.Linq;

	public class CountriesService : ICountriesService
	{
		private readonly int _countryCacheTimeMinutes = int.Parse(WebConfigurationManager.AppSettings[ApplicationSettingKeyNames.CountryCacheTimeMinutes]);

		private readonly string _countriesServiceBaseUri = WebConfigurationManager.AppSettings[ApplicationSettingKeyNames.CountriesServiceBaseUri];

		private IHttpClientHelper _httpClientHelper;

		public CountriesService(IHttpClientHelper httpClientHelper)
		{
			this._httpClientHelper = httpClientHelper;
		}

		public Country Get(string countryCode)
		{
			var cachedCountries = CacheUtilities.GetObjectFromCache($"CountriesCache", this._countryCacheTimeMinutes, this.GetAllCountries);

			if (!cachedCountries.ContainsKey(countryCode))
			{
				return null;
			}

			return cachedCountries[countryCode];
		}

		internal static Country BuildCountry(dynamic countryJson)
		{
			string alpha2Code = countryJson["alpha2Code"];
			string alpha3Code = countryJson["alpha3Code"];

			var country = new Country()
			{
				Alpha2CountryCode = alpha2Code,
				Alpha3CountryCode = alpha3Code
			};

			dynamic currencies = JArray.Parse(countryJson["currencies"]);

			foreach (var currency in currencies)
			{
				country.Currencies.Add(currency.ToString());
			}

			return country;
		}

		private Dictionary<string, Country> GetAllCountries()
		{
			var response = this._httpClientHelper.GetResult(new Uri(this._countriesServiceBaseUri));
			dynamic allCountriesResponse = JArray.Parse(response);

			var allCountries = new Dictionary<string, Country>();
			
			foreach (var countryJson in allCountriesResponse)
			{
				Country country = BuildCountry(countryJson);
				allCountries.Add(country.Alpha2CountryCode, country);
			}

			return allCountries;
		}
	}
}