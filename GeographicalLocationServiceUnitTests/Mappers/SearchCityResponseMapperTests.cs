namespace GeographicalLocationServiceUnitTests.Mappers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
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
				CountryCode = "GB",
				EstablishedOn = new DateTime(1500, 1, 1),
				EstimatedPopulation = 260000,
				Id = 1234,
				Name = "Test Name",
				SubRegion = "Test Sub Region",
				TouristRating = 5
			};

			var testInputCountries = new Dictionary<string, Country>();

			testInputCountries.Add(
				testInputCity.CountryCode,
				new Country()
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
				});

			var testInputCurrentWeathers = new Dictionary<string, CurrentWeather>();
			testInputCurrentWeathers.Add(
				testInputCity.CountryCode,
				new CurrentWeather()
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
				});

			var searchCityResponse = SearchCityResponseMapper.Map(testInputCity, testInputCountries, testInputCurrentWeathers);

			Assert.IsNotNull(searchCityResponse);
			Assert.AreEqual(testInputCity.EstablishedOn, searchCityResponse.EstablishedOn);
			Assert.AreEqual(testInputCity.EstimatedPopulation, searchCityResponse.EstimatedPopulation);
			Assert.AreEqual(testInputCity.Id, searchCityResponse.Id);
			Assert.AreEqual(testInputCity.Name, searchCityResponse.Name);
			Assert.AreEqual(testInputCity.SubRegion, searchCityResponse.SubRegion);
			Assert.AreEqual(testInputCity.TouristRating, searchCityResponse.TouristRating);

			Assert.IsNotNull(searchCityResponse.Country);

			var country = testInputCountries[testInputCity.CountryCode];

			Assert.AreEqual(country.Alpha2Code, searchCityResponse.Country.Alpha2CountryCode);
			Assert.AreEqual(country.Alpha3Code, searchCityResponse.Country.Alpha3CountryCode);
			Assert.IsNotNull(searchCityResponse.Country.Currencies);
			Assert.AreEqual(country.Currencies.Length, searchCityResponse.Country.Currencies.Count);

			for (int currencyIndex = 0; currencyIndex < country.Currencies.Length; currencyIndex++)
			{
				Assert.IsTrue(searchCityResponse.Country.Currencies.Contains(country.Currencies[currencyIndex].Code));
			}

			Assert.IsNotNull(searchCityResponse.CurrentWeather);

			var testInputWeather = testInputCurrentWeathers[testInputCity.CountryCode];

			Assert.AreEqual(testInputWeather.Weather.First().Description, searchCityResponse.CurrentWeather.Description);
			Assert.AreEqual(testInputWeather.Weather.First().Main, searchCityResponse.CurrentWeather.Summary);
			Assert.AreEqual(testInputWeather.Main.Humidity, searchCityResponse.CurrentWeather.Humidity);
			Assert.AreEqual(testInputWeather.Main.Pressure, searchCityResponse.CurrentWeather.Pressure);
			Assert.AreEqual(testInputWeather.Main.Temp, searchCityResponse.CurrentWeather.Temperature);
			Assert.AreEqual(testInputWeather.Wind.Speed, searchCityResponse.CurrentWeather.WindSpeed);
		}
	}
}
