namespace GeographicalLocationService.Models
{
	using System.ComponentModel.DataAnnotations;

	public class AddCityRequest : UpdateCityRequest
	{
		[Required]
		[MaxLength(128)]
		public string Name { get; set; }

		[MaxLength(128)]
		public string SubRegion { get; set; }

		/// <summary>
		/// Gets or sets the country code
		/// </summary>
		/// <remarks>International Organization for Standardization, ISO Alpha-2</remarks>
		[Required]
		[MaxLength(2)]
		public string CountryCode { get; set; }
	}
}
