namespace GeographicalLocationService.App_Start
{
	using AutoMapper;
	using GeographicalLocationService.Database;
	using MODELS = GeographicalLocationService.Models;

	public class AutoMapperConfig
	{
		public static void Initialize()
		{
			Mapper.Initialize((config) =>
			{
				config.CreateMap<MODELS.AddCityRequest, City>().ReverseMap();
			});
		}
	}
}