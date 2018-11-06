namespace GeographicalLocationService.ExternalServices.Countries
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public interface ICountriesService
	{
		Country Get(string countryCode);
	}
}
