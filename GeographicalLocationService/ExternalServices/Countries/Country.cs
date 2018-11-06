namespace GeographicalLocationService.ExternalServices.Countries
{
	using Newtonsoft.Json;

	public class Country
	{
		[JsonProperty("alpha2Code")]
		public string Alpha2Code { get; set; }

		[JsonProperty("alpha3Code")]
		public string Alpha3Code { get; set; }

		[JsonProperty("currencies")]
		public Currency[] Currencies { get; set; }
	}
}