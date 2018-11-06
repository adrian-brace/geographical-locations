namespace GeographicalLocationService.ExternalServices.Countries
{
	using Newtonsoft.Json;

	public class Currency
	{
		[JsonProperty("code")]
		public string Code { get; set; }
	}
}