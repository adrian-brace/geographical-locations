namespace GeographicalLocationService.Models
{
	using System;

	public class City
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string SubRegion { get; set; }

		public byte? TouristRating { get; set; }

		public DateTime? EstablishedOn { get; set; }

		public int? EstimatedPopulation { get; set; }
	}
}