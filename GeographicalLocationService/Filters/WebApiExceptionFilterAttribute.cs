﻿namespace GeographicalLocationService.Filters
{
	using System;
	using System.Net;
	using System.Net.Http;
	using System.Web.Http.Filters;
	using GeographicalLocationService.Logging;

	[AttributeUsage(AttributeTargets.Class)]
	public sealed class WebApiExceptionFilterAttribute : ExceptionFilterAttribute
	{
		public override void OnException(HttpActionExecutedContext actionExecutedContext)
		{
			if (actionExecutedContext == null)
			{
				throw new ArgumentNullException("actionExecutedContext");
			}

			Logger.RecordException(actionExecutedContext.Exception);

			if (actionExecutedContext.Exception is NotImplementedException)
			{
				actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
			}
		}
	}
}