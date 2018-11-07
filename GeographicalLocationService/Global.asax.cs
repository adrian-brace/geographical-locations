namespace GeographicalLocationService
{
	using System.Web;
	using System.Web.Http;
	using System.Web.Mvc;
	using System.Web.Routing;
	using GeographicalLocationService.App_Start;

	public class WebApiApplication : HttpApplication
	{
		protected void Application_Start()
		{
			AutoMapperConfig.Initialize();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
		}
	}
}
