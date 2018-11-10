namespace GeographicalLocationService.Filters
{
	using System;
	using System.Net;
	using System.Net.Http;
	using System.Web.Configuration;
	using System.Web.Http;
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

			if (bool.Parse(WebConfigurationManager.AppSettings[ApplicationSettingKeyNames.ErrorLoggingIsEnabled]))
			{
				Logger.RecordException(actionExecutedContext.Exception);
			}

			if (actionExecutedContext.Exception is NotImplementedException)
			{
				actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
			}
			else if(!(actionExecutedContext.Exception is HttpRequestException)
				&& !(actionExecutedContext.Exception is HttpResponseException))
			{
				var errorContent = "An unexpected exception occured: ";

				if (bool.Parse(WebConfigurationManager.AppSettings[ApplicationSettingKeyNames.ReturnFullExceptionDetail]))
				{
					errorContent += actionExecutedContext.Exception.ToString();
				}
				else
				{
					errorContent += $"{actionExecutedContext.Exception.Message}";
				}

				actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
				{
					Content = new StringContent(errorContent)
				};
			}
		}
	}
}