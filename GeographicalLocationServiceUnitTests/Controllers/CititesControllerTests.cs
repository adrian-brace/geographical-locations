namespace GeographicalLocationServiceUnitTests.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using GeographicalLocationService.Controllers;
	using GeographicalLocationService.Database;
	using GeographicalLocationService.ExternalServices.Countries;
	using GeographicalLocationService.ExternalServices.Weather;	
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;
	using MODELS = GeographicalLocationService.Models;

	[TestClass]
	public class CititesControllerTests
	{
		[TestMethod]
		public void Delete()
		{
			// Arrange
			var cityToDelete = new City()
			{
				Id = 1234,
				CountryCode = "GB",
				EstablishedOn = new DateTime(1536, 1, 1),
				EstimatedPopulation = 250000,
				Name = "Cardiff",
				SubRegion = "Glamorgan",
				TouristRating = 5
			};

			var cities = new List<City>()
			{
				cityToDelete
			};

			var citiesMocked = new Mock<DbSet<City>>();
			citiesMocked.As<IQueryable<City>>().Setup(m => m.Provider).Returns(cities.AsQueryable().Provider);
			citiesMocked.As<IQueryable<City>>().Setup(m => m.Expression).Returns(cities.AsQueryable().Expression);

			var geographicalLocationsDatabaseMocked = new Mock<IGeographicalLocationsDatabase>();
			geographicalLocationsDatabaseMocked.Setup(db => db.Cities.Remove(cityToDelete)).Callback<City>((city) => cities.Remove(city));

			var countriesServiceMocked = new Mock<ICountriesService>();
			var weatherServiceMocked = new Mock<IWeatherService>();

			bool actualResult = false;
			CitiesController controller = null;

			// Act
			try
			{
				controller = new CitiesController(
					geographicalLocationsDatabaseMocked.Object,
					countriesServiceMocked.Object,
					weatherServiceMocked.Object);

				actualResult = controller.Delete(cityToDelete.Id);
			}
			catch (Exception ex)
			{
				Assert.Fail($"Exception not expected: {ex.Message}");
			}
			finally
			{
				if (controller != null)
				{
					controller.Dispose();
					controller = null;
				}
			}

			// Assert
			Mock.VerifyAll(geographicalLocationsDatabaseMocked);
			Assert.IsTrue(actualResult, "Expected City to be successfully deleted");
			Assert.IsTrue(cities.Count == 0);
		}

		[TestMethod]
		public void Get()
		{
			CitiesController controller = null;
			MODELS.SearchCityResponse actualResponse = null;

			var cardiffCity = new City()
			{
				Id = 1234,
				CountryCode = "GB",
				EstablishedOn = new DateTime(1536, 1, 1),
				EstimatedPopulation = 250000,
				Name = "Cardiff",
				SubRegion = "Glamorgan",
				TouristRating = 5
			};

			var cities = new List<City>()
			{
				cardiffCity
			};

			var citiesMocked = new Mock<DbSet<City>>();
			citiesMocked.As<IQueryable<City>>().Setup(m => m.Provider).Returns(cities.AsQueryable().Provider);
			citiesMocked.As<IQueryable<City>>().Setup(m => m.Expression).Returns(cities.AsQueryable().Expression);
			citiesMocked.As<IQueryable<City>>().Setup(m => m.ElementType).Returns(cities.AsQueryable().ElementType);
			citiesMocked.As<IQueryable<City>>().Setup(m => m.GetEnumerator()).Returns(() => cities.AsQueryable().GetEnumerator());

			var geographicalLocationsDatabaseMocked = new Mock<IGeographicalLocationsDatabase>();
			geographicalLocationsDatabaseMocked.Setup(db => db.Cities).Returns(citiesMocked.Object);

			var countriesServiceMocked = new Mock<ICountriesService>();

			var testCountry = new Country()
			{
				Alpha2CountryCode = "GB",
				Alpha3CountryCode = "GBR",
				CurrencyCode = "GBP"
			};

			countriesServiceMocked.Setup(c => c.Get(cardiffCity.CountryCode)).Returns(testCountry);

			var weatherServiceMocked = new Mock<IWeatherService>();
			var testCurrentWeather = new CurrentWeather()
			{
				Description = "light intensity drizzle",
				Summary = "Drizzle",
				Humidity = 81,
				Pressure = 1012,
				Temperature = 280.32m,
				WindSpeed = 4.1m
			};

			weatherServiceMocked.Setup(w => w.Get(cardiffCity.Name, cardiffCity.CountryCode)).Returns(new List<CurrentWeather>()
			{
				testCurrentWeather
			});

			// Act
			try
			{
				controller = new CitiesController(
					geographicalLocationsDatabaseMocked.Object,
					countriesServiceMocked.Object,
					weatherServiceMocked.Object);

				actualResponse = controller.Get(cardiffCity.Name);
			}
			catch (Exception ex)
			{
				Assert.Fail($"Exception not expected: {ex.Message}");
			}
			finally
			{
				if (controller != null)
				{
					controller.Dispose();
					controller = null;
				}
			}

			Mock.VerifyAll(geographicalLocationsDatabaseMocked, countriesServiceMocked, weatherServiceMocked);

			Assert.IsNotNull(actualResponse);
			Assert.IsNotNull(actualResponse.Country);
			Assert.IsNotNull(actualResponse.CurrentWeather);

			Assert.AreEqual(cardiffCity.Id, actualResponse.Id);
			Assert.AreEqual(cardiffCity.Name, actualResponse.Name);
			Assert.AreEqual(cardiffCity.SubRegion, actualResponse.SubRegion);
			Assert.AreEqual(cardiffCity.TouristRating, actualResponse.TouristRating);
			Assert.AreEqual(cardiffCity.EstablishedOn, actualResponse.EstablishedOn);
			Assert.AreEqual(cardiffCity.EstimatedPopulation, actualResponse.EstimatedPopulation);

			Assert.AreEqual(testCountry.Alpha2CountryCode, actualResponse.Country.Alpha2CountryCode);
			Assert.AreEqual(testCountry.Alpha3CountryCode, actualResponse.Country.Alpha3CountryCode);
			Assert.AreEqual(testCountry.CurrencyCode, actualResponse.Country.CurrencyCode);

			Assert.AreEqual(1, actualResponse.CurrentWeather.Count);
			Assert.AreEqual(testCurrentWeather.Description, actualResponse.CurrentWeather.First().Description);
			Assert.AreEqual(testCurrentWeather.Humidity, actualResponse.CurrentWeather.First().Humidity);
			Assert.AreEqual(testCurrentWeather.Pressure, actualResponse.CurrentWeather.First().Pressure);
			Assert.AreEqual(testCurrentWeather.Summary, actualResponse.CurrentWeather.First().Summary);
			Assert.AreEqual(testCurrentWeather.Temperature, actualResponse.CurrentWeather.First().Temperature);
			Assert.AreEqual(testCurrentWeather.WindSpeed, actualResponse.CurrentWeather.First().WindSpeed);
		}

		[TestMethod]
		public void Post()
		{
			// Arrange
			var testInputAddCityRequest = new MODELS.AddCityRequest()
			{
				Country = "GB",
				EstablishedOn = new DateTime(1536, 1, 1),
				EstimatedPopulation = 250000,
				Name = "Cardiff",
				SubRegion = "Glamorgan",
				TouristRating = 5
			};

			var addedCity = new City()
			{
				Id = 1234,
				CountryCode = "GB",
				EstablishedOn = new DateTime(1536, 1, 1),
				EstimatedPopulation = 250000,
				Name = "Cardiff",
				SubRegion = "Glamorgan",
				TouristRating = 5
			};

			var cities = new List<City>();

			var citiesMocked = new Mock<DbSet<City>>();
			citiesMocked.As<IQueryable<City>>().Setup(m => m.Provider).Returns(cities.AsQueryable().Provider);
			citiesMocked.As<IQueryable<City>>().Setup(m => m.Expression).Returns(cities.AsQueryable().Expression);

			var geographicalLocationsDatabaseMocked = new Mock<IGeographicalLocationsDatabase>();
			geographicalLocationsDatabaseMocked.Setup(db => db.Cities.Add(addedCity)).Callback<City>((city) => cities.Add(city));

			var countriesServiceMocked = new Mock<ICountriesService>();
			var weatherServiceMocked = new Mock<IWeatherService>();

			int actualResult = 0;
			CitiesController controller = null;

			// Act
			try
			{
				controller = new CitiesController(
					geographicalLocationsDatabaseMocked.Object,
					countriesServiceMocked.Object,
					weatherServiceMocked.Object);

				actualResult = controller.Post(testInputAddCityRequest);
			}
			catch (Exception ex)
			{
				Assert.Fail($"Exception not expected: {ex.Message}");
			}
			finally
			{
				if (controller != null)
				{
					controller.Dispose();
					controller = null;
				}
			}

			// Assert
			Mock.VerifyAll(geographicalLocationsDatabaseMocked);
			Assert.AreEqual(addedCity.Id, actualResult, "Expected a non zero City ID to be returned");
			Assert.IsTrue(cities.Any(c => c.Id == addedCity.Id));
		}

		[TestMethod]
		public void Put()
		{
			// Arrange
			var testCityId = 1234;
			var testInputUpdateCityRequest = new MODELS.UpdateCityRequest()
			{
				EstablishedOn = new DateTime(1536, 1, 1),
				EstimatedPopulation = 250000,
				TouristRating = 5
			};

			var updatedCity = new City()
			{
				Id = testCityId,
				CountryCode = "GB",
				EstablishedOn = new DateTime(1536, 1, 1),
				EstimatedPopulation = 250000,
				Name = "Cardiff",
				SubRegion = "Glamorgan",
				TouristRating = 5
			};

			var cities = new List<City>()
			{
				updatedCity
			};

			var citiesMocked = new Mock<DbSet<City>>();
			citiesMocked.As<IQueryable<City>>().Setup(m => m.Provider).Returns(cities.AsQueryable().Provider);
			citiesMocked.As<IQueryable<City>>().Setup(m => m.Expression).Returns(cities.AsQueryable().Expression);
			citiesMocked.As<IQueryable<City>>().Setup(m => m.ElementType).Returns(cities.AsQueryable().ElementType);
			citiesMocked.As<IQueryable<City>>().Setup(m => m.GetEnumerator()).Returns(() => cities.AsQueryable().GetEnumerator());

			var geographicalLocationsDatabaseMocked = new Mock<IGeographicalLocationsDatabase>();
			geographicalLocationsDatabaseMocked.Setup(db => db.Cities).Returns(citiesMocked.Object);
			geographicalLocationsDatabaseMocked.Setup(db => db.SaveChanges()).Returns(1);

			var countriesServiceMocked = new Mock<ICountriesService>();
			var weatherServiceMocked = new Mock<IWeatherService>();

			bool actualResult = false;
			CitiesController controller = null;

			// Act
			try
			{
				controller = new CitiesController(
					geographicalLocationsDatabaseMocked.Object,
					countriesServiceMocked.Object,
					weatherServiceMocked.Object);

				actualResult = controller.Put(testCityId, testInputUpdateCityRequest);
			}
			catch (Exception ex)
			{
				Assert.Fail($"Exception not expected: {ex.Message}");
			}
			finally
			{
				if (controller != null)
				{
					controller.Dispose();
					controller = null;
				}
			}

			// Assert
			Mock.VerifyAll(geographicalLocationsDatabaseMocked);
			Assert.IsTrue(actualResult, "Expected API to report that the City was successfully updated.");
			Assert.IsTrue(cities.Any(c => c.Id == updatedCity.Id));
		}
	}
}
