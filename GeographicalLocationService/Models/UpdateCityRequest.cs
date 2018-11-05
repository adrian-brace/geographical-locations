namespace GeographicalLocationService.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using System.Web;

	public class UpdateCityRequest
	{
		[Range(1, 5)]
		public int TouristRating { get; set; }

		public DateTime DateEstablished { get; set; }

		public int ExtimatedPopulation { get; set; }
	}
}
