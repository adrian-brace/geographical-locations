namespace GeographicalLocationService.Models
{
	public class CurrentWeather
	{
		public string Summary { get; set; }

		public string Description { get; set; }

		public double Temperature { get; set; }

		public int Pressure { get; set; }

		public int Humidity { get; set; }

		public double WindSpeed { get; set; }
	}
}