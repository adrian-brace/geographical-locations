namespace GeographicalLocationService.ExternalServices.Weather
{
	using Newtonsoft.Json;

	public class Wind
	{
		[JsonProperty("speed")]
		public double Speed { get; set; }
	}
}