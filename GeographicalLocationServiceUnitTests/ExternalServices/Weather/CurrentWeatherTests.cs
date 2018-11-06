namespace GeographicalLocationServiceUnitTests.ExternalServices.Weather
{
	using GeographicalLocationService.ExternalServices.Weather;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Newtonsoft.Json;

	[TestClass]
	public class CurrentWeatherTests
	{
		[TestMethod]
		public void DeserializeCurrentWeather()
		{
			string jsonFromResource = EmbeddedResourceUtilities.ReadFromEmbeddedResource("GeographicalLocationServiceUnitTests.ExternalServices.Weather.CurrentWeather.json");
			var currentWeather = JsonConvert.DeserializeObject<CurrentWeather>(jsonFromResource);
			Assert.IsNotNull(currentWeather);
			Assert.IsNotNull(currentWeather.Main);
			Assert.IsNotNull(currentWeather.Weather);
			Assert.IsNotNull(currentWeather.Wind);
		}
	}
}
