namespace GeographicalLocationService
{
	using System.Collections.Generic;
	using System.Net.Http.Formatting;
	using System.Web.Http;
	using System.Web.Http.Filters;
	using GeographicalLocationService.Filters;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;
	using Newtonsoft.Json.Serialization;
	using Swashbuckle.Application;

	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			config.Formatters.Clear();
			var json = new JsonMediaTypeFormatter
			{
				SerializerSettings =
				{
					NullValueHandling = NullValueHandling.Ignore,
					ContractResolver = new CamelCasePropertyNamesContractResolver(),
					Formatting = Formatting.Indented
				}
			};
			config.Formatters.Add(json);

			// Web API configuration and services
			config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd" });

			// Web API routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
			   name: "swagger_root",
			   routeTemplate: string.Empty,
			   defaults: null,
			   constraints: null,
			   handler: new RedirectHandler(message => message.RequestUri.ToString(), "swagger"));

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional });

			AddFilters(config);
		}

		private static void AddFilters(HttpConfiguration config)
		{
			config.Filters.AddRange(
				new List<IFilter>
				{
					new RequestLoggingFilterAttribute(),
					new ModelValidationFilterAttribute(),
					new WebApiExceptionFilterAttribute()
				});
		}
	}
}
