namespace GeographicalLocationServiceUnitTests.ExternalServices.Countries
{
	using System.Collections.Generic;
	using System.Linq;
	using GeographicalLocationService.ExternalServices.Countries;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Newtonsoft.Json;

	[TestClass]
	public class CountriesTests
	{
		[TestMethod]
		public void DeserializeAllCountries()
		{
			string jsonFromResource = EmbeddedResourceUtilities.ReadFromEmbeddedResource("GeographicalLocationServiceUnitTests.ExternalServices.Countries.AllCountries.json");
			var countries = JsonConvert.DeserializeObject<List<Country>>(jsonFromResource);
			Assert.IsNotNull(countries);
			Assert.IsTrue(countries.Count > 0);
			Assert.IsTrue(countries.Any(c => c.Alpha2Code == "GB"));
		}		
	}
}
