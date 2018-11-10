namespace GeographicalLocationService.Filters
{
	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Web.Configuration;
	using System.Web.Http.Controllers;
	using System.Web.Http.Filters;
	using GeographicalLocationService.Logging;

	[AttributeUsage(AttributeTargets.Class)]
	public sealed class RequestLoggingFilterAttribute : ActionFilterAttribute
	{
		[SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope", Justification = "Response instance will be disposed later by the garbage collector.")]
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			if (actionContext == null)
			{
				throw new ArgumentNullException("actionContext");
			}

			if (bool.Parse(WebConfigurationManager.AppSettings[ApplicationSettingKeyNames.RequestLoggingIsEnabled]))
			{
				Logger.RecordHttpRequest(actionContext.Request);
			}
		}
	}
}