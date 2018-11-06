namespace GeographicalLocationService.ExternalServices.Weather
{
	public interface IWeatherService
	{
		CurrentWeather Get(string cityName, string countryCode);
	}
}
