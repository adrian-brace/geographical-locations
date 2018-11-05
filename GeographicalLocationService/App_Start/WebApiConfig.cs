﻿namespace GeographicalLocationService
{
	using System.Web.Http;
	using Swashbuckle.Application;

	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services

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
		}
	}
}
