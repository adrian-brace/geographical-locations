namespace GeographicalLocationServiceUnitTests.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using GeographicalLocationService.App_Start;
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
		[AssemblyInitialize]
		public static void AssemblyInit(TestContext context)
		{
			AutoMapperConfig.Initialize();
		}

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
			citiesMocked.As<IQueryable<City>>().Setup(m => m.ElementType).Returns(cities.AsQueryable().ElementType);
			citiesMocked.As<IQueryable<City>>().Setup(m => m.GetEnumerator()).Returns(cities.AsQueryable().GetEnumerator);
			citiesMocked.Setup(db => db.Remove(cityToDelete)).Callback<City>((city) => cities.Remove(city));

			var geographicalLocationsDatabaseMocked = new Mock<IGeographicalLocationsDatabase>();
			geographicalLocationsDatabaseMocked.Setup(db => db.Cities).Returns(citiesMocked.Object).Verifiable();
			geographicalLocationsDatabaseMocked.Setup(db => db.SaveChanges()).Returns(1).Verifiable();

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
		public void Search()
		{
			CitiesController controller = null;
			List<MODELS.SearchCityResponse> actualResponse = null;

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

			countriesServiceMocked.Setup(c => c.Get(cardiffCity.CountryCode)).Returns(testCountry);

			var weatherServiceMocked = new Mock<IWeatherService>();
			var testCurrentWeather = new CurrentWeather()
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

			weatherServiceMocked.Setup(w => w.Get(cardiffCity.Name, cardiffCity.CountryCode)).Returns(testCurrentWeather);

			// Act
			try
			{
				controller = new CitiesController(
					geographicalLocationsDatabaseMocked.Object,
					countriesServiceMocked.Object,
					weatherServiceMocked.Object);

				actualResponse = controller.Search(cardiffCity.Name);
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

			var firstSearchCityResponse = actualResponse.First();

			Assert.IsNotNull(firstSearchCityResponse.Country);
			Assert.IsNotNull(firstSearchCityResponse.CurrentWeather);

			Assert.AreEqual(cardiffCity.Id, firstSearchCityResponse.Id);
			Assert.AreEqual(cardiffCity.Name, firstSearchCityResponse.Name);
			Assert.AreEqual(cardiffCity.SubRegion, firstSearchCityResponse.SubRegion);
			Assert.AreEqual(cardiffCity.TouristRating, firstSearchCityResponse.TouristRating);
			Assert.AreEqual(cardiffCity.EstablishedOn, firstSearchCityResponse.EstablishedOn);
			Assert.AreEqual(cardiffCity.EstimatedPopulation, firstSearchCityResponse.EstimatedPopulation);

			Assert.AreEqual(testCountry.Alpha2Code, firstSearchCityResponse.Country.Alpha2CountryCode);
			Assert.AreEqual(testCountry.Alpha3Code, firstSearchCityResponse.Country.Alpha3CountryCode);
			Assert.AreEqual(testCountry.Currencies.First().Code, firstSearchCityResponse.Country.Currencies.First());

			Assert.AreEqual(testCurrentWeather.Weather.First().Description, firstSearchCityResponse.CurrentWeather.Description);
			Assert.AreEqual(testCurrentWeather.Weather.First().Main, firstSearchCityResponse.CurrentWeather.Summary);
			Assert.AreEqual(testCurrentWeather.Main.Humidity, firstSearchCityResponse.CurrentWeather.Humidity);
			Assert.AreEqual(testCurrentWeather.Main.Pressure, firstSearchCityResponse.CurrentWeather.Pressure);			
			Assert.AreEqual(testCurrentWeather.Main.Temp, firstSearchCityResponse.CurrentWeather.Temperature);
			Assert.AreEqual(testCurrentWeather.Wind.Speed, firstSearchCityResponse.CurrentWeather.WindSpeed);
		}

		[TestMethod]
		public void Add()
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

			var newCity = new City()
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
			citiesMocked.Setup(db => db.Add(It.IsAny<City>())).Returns(newCity).Callback<City>((city) => cities.Add(newCity));

			var geographicalLocationsDatabaseMocked = new Mock<IGeographicalLocationsDatabase>();
			geographicalLocationsDatabaseMocked.Setup(db => db.Cities).Returns(citiesMocked.Object).Verifiable();
			geographicalLocationsDatabaseMocked.Setup(db => db.SaveChanges()).Returns(1).Verifiable();

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

				actualResult = controller.Add(testInputAddCityRequest);
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
			Assert.AreEqual(newCity.Id, actualResult, "Expected a non zero City ID to be returned");
			Assert.IsTrue(cities.Any(c => c.Id == newCity.Id));
		}

		[TestMethod]
		public void Update()
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
			geographicalLocationsDatabaseMocked.Setup(db => db.Cities).Returns(citiesMocked.Object).Verifiable();
			geographicalLocationsDatabaseMocked.Setup(db => db.SaveChanges()).Returns(1).Verifiable();

			var countriesServiceMocked = new Mock<ICountriesService>();
			var weatherServiceMocked = new Mock<IWeatherService>();

			var actualResult = 0;
			CitiesController controller = null;

			// Act
			try
			{
				controller = new CitiesController(
					geographicalLocationsDatabaseMocked.Object,
					countriesServiceMocked.Object,
					weatherServiceMocked.Object);

				actualResult = controller.Update(testCityId, testInputUpdateCityRequest);
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
			Assert.AreEqual(1, actualResult, "Expected API to report that one City row was successfully updated.");
			Assert.IsTrue(cities.Any(c => c.Id == updatedCity.Id));			
		}
	}
}
