namespace GeographicalLocationService.Models
{
	public class SearchCityResponse : City
	{
		public Country Country { get; set; }

		public CurrentWeather CurrentWeather { get; set; }
	}
}