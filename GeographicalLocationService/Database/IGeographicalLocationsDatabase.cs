namespace GeographicalLocationService.Database
{
	using System.Data.Entity;

	public interface IGeographicalLocationsDatabase
	{
		DbSet<City> Cities { get; set; }
		
		int SaveChanges();
	}
}
