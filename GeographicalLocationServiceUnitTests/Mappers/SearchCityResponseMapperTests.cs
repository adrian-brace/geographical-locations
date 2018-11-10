namespace GeographicalLocationServiceUnitTests.Mappers
{
	using System;
	using GeographicalLocationService.Database;
	using GeographicalLocationService.ExternalServices.Countries;
	using GeographicalLocationService.ExternalServices.Weather;
	using GeographicalLocationService.Mappers;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class SearchCityResponseMapperTests
	{
		[TestMethod]
		public void Map()
		{
			var testInputCity = new City()
			{
				EstablishedOn = new DateTime(1500,1,1),
				EstimatedPopulation = 260000,
				Id = 1234,
				Name = "Test Name",
				SubRegion = "Test Sub Region",
				TouristRating = 5
			};

			var testInputCountry = new Country()
			{
				Alpha2Code = "GB",
				Alpha3Code = "GBR",
				Currencies = new Currency[]
				{
					new Currency()
					{
						Code = "GBP"
					}
				}
			};

			var testInputCurrentWeather = new CurrentWeather()
			{
				Main = new Main()
				{
					Humidity = 81,
					Pressure = 1012,
					Temp = 280.32D
				},
				Weather = new Weather[]
				{
					new Weather()
					{
						Description = "light intensity drizzle",
						Main = "Drizzle"
					}
				},
				Wind = new Wind()
				{
					Speed = 4.1D
				}
			};

			var searchCityResponse = SearchCityResponseMapper.Map(testInputCity, testInputCountry, testInputCurrentWeather);

			Assert.IsNotNull(searchCityResponse);
			Assert.AreEqual(testInputCity.EstablishedOn, searchCityResponse.EstablishedOn);
			Assert.AreEqual(testInputCity.EstimatedPopulation, searchCityResponse.EstimatedPopulation);
			Assert.AreEqual(testInputCity.Id, searchCityResponse.Id);
			Assert.AreEqual(testInputCity.Name, searchCityResponse.Name);
			Assert.AreEqual(testInputCity.SubRegion, searchCityResponse.SubRegion);
			Assert.AreEqual(testInputCity.TouristRating, searchCityResponse.TouristRating);

			Assert.IsNotNull(searchCityResponse.Country);
			Assert.AreEqual(testInputCountry.Alpha2Code, searchCityResponse.Country.Alpha2CountryCode);
			Assert.AreEqual(testInputCountry.Alpha3Code, searchCityResponse.Country.Alpha3CountryCode);
			Assert.IsNotNull(searchCityResponse.Country.Currencies);
			Assert.AreEqual(testInputCountry.Currencies.Length, searchCityResponse.Country.Currencies.Count);

			for (int currencyIndex = 0; currencyIndex < testInputCountry.Currencies.Length; currencyIndex++)
			{
				Assert.IsTrue(searchCityResponse.Country.Currencies.Contains(testInputCountry.Currencies[currencyIndex].Code));
			}

			Assert.IsNotNull(searchCityResponse.CurrentWeather);

			var testInputWeather = testInputCurrentWeather.Weather[0];

			Assert.AreEqual(testInputWeather.Description, searchCityResponse.CurrentWeather.Description);
			Assert.AreEqual(testInputWeather.Main, searchCityResponse.CurrentWeather.Summary);

			Assert.AreEqual(testInputCurrentWeather.Main.Humidity, searchCityResponse.CurrentWeather.Humidity);
			Assert.AreEqual(testInputCurrentWeather.Main.Pressure, searchCityResponse.CurrentWeather.Pressure);
			Assert.AreEqual(testInputCurrentWeather.Main.Temp, searchCityResponse.CurrentWeather.Temperature);
			Assert.AreEqual(testInputCurrentWeather.Wind.Speed, searchCityResponse.CurrentWeather.WindSpeed);
		}
	}
}
