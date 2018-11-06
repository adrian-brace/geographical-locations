namespace GeographicalLocationService.ExternalServices.Weather
{
	using Newtonsoft.Json;

	public class CurrentWeather
	{
		[JsonProperty("weather")]
		public Weather[] Weather { get; set; }

		[JsonProperty("main")]
		public Main Main { get; set; }

		[JsonProperty("wind")]
		public Wind Wind { get; set; }
	}
}