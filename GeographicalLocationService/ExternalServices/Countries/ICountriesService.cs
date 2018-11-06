namespace GeographicalLocationService.ExternalServices.Countries
{
	public interface ICountriesService
	{
		Country Get(string countryCode);
	}
}
