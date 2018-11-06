namespace GeographicalLocationService.Models
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;

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