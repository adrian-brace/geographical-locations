namespace GeographicalLocationService.Models
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using GeographicalLocationService.ExternalServices.Countries;
	using GeographicalLocationService.ExternalServices.Weather;

	public class SearchCityResponse : City
	{
		public Country Country { get; set; }

		public List<CurrentWeather> CurrentWeather { get; set; }
	}
}