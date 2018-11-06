namespace GeographicalLocationService.ExternalServices.Weather
{
	using Newtonsoft.Json;

	public class Weather
	{
		[JsonProperty("main")]
		public string Main { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }
	}
}