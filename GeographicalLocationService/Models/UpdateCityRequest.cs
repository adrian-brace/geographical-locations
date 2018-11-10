namespace GeographicalLocationService.Models
{
	using System.ComponentModel.DataAnnotations;
	using GeographicalLocationService.Validation;

	public class UpdateCityRequest
	{
		[Range(1, 5)]
		public byte? TouristRating { get; set; }

		[ISODateTime]
		public string EstablishedOn { get; set; }

		public int? EstimatedPopulation { get; set; }
	}
}
