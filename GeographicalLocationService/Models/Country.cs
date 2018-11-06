namespace GeographicalLocationService.Models
{
	using System.Collections.Generic;

	public class Country
	{
		public string Alpha2CountryCode { get; set; }

		public string Alpha3CountryCode { get; set; }

		public List<string> Currencies { get; set; }
	}
}