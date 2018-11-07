namespace GeographicalLocationService.Filters
{
	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Net;
	using System.Net.Http;
	using System.Web.Http;
	using System.Web.Http.Controllers;
	using System.Web.Http.Filters;

	[AttributeUsage(AttributeTargets.Class)]
	public sealed class ModelValidationFilterAttribute : ActionFilterAttribute
	{
		[SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope", Justification = "Response instance will be disposed later by the garbage collector.")]
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			if (actionContext == null)
			{
				throw new ArgumentNullException("actionContext");
			}

			if (!actionContext.ModelState.IsValid)
			{
				var response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, new ValidationErrorResponse() { ValidationErrors = actionContext.ModelState }, actionContext.ControllerContext.Configuration.Formatters.JsonFormatter);
				throw new HttpResponseException(response);
			}
		}
	}
}