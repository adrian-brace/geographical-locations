using System.Reflection;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using GeographicalLocationService.App_Start;
using GeographicalLocationService.Database;
using GeographicalLocationService.ExternalServices.Countries;
using GeographicalLocationService.ExternalServices.Weather;
using GeographicalLocationService.HttpClientUtilities;

[assembly: PreApplicationStartMethod(typeof(AutofacConfig), "Register")]

namespace GeographicalLocationService.App_Start
{
	public static class AutofacConfig
	{
		public static void Register()
		{
			var config = GlobalConfiguration.Configuration;
			var builder = new ContainerBuilder();
			builder.RegisterType<GeographicalLocationsEntities>().As<IGeographicalLocationsDatabase>().InstancePerLifetimeScope();
			builder.RegisterType<HttpClientHelper>().As<IHttpClientHelper>().InstancePerLifetimeScope();
			builder.RegisterType<CountriesService>().As<ICountriesService>().InstancePerLifetimeScope();
			builder.RegisterType<WeatherService>().As<IWeatherService>().InstancePerLifetimeScope();
			builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
			var container = builder.Build();
			config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
		}
	}
}
