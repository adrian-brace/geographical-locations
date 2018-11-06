namespace GeographicalLocationService.ExternalServices.Weather
{
	public class CurrentWeather
	{
		public string Summary { get; set; }

		public string Description { get; set; }

		public decimal Temperature { get; set; }

		public int Pressure { get; set; }

		public int Humidity { get; set; }

		public decimal WindSpeed { get; set; }
	}
}