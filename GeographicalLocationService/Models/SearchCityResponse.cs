namespace GeographicalLocationService.Models
{
	using System.Collections.Generic;

	public class SearchCityResponse : City
	{
		public Country Country { get; set; }

		public CurrentWeather CurrentWeather { get; set; }
	}
}