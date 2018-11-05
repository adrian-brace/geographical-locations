namespace GeographicalLocationService.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using System.Web;

	public class AddCityRequest : UpdateCityRequest
	{
		[Required]
		[MaxLength(128)]
		public string Name { get; set; }

		[MaxLength(128)]
		public string SubRegion { get; set; }

		[Required]
		[MaxLength(64)]
		public string Country { get; set; }
	}
}
