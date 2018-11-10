namespace GeographicalLocationService.Logging
{
	using System;
	using System.Diagnostics;
	using System.Net.Http;
	using System.Web.Http;

	public static class Logger
	{
		public static void RecordException(Exception ex)
		{
			Debug.WriteLine(ex.ToString());

			var httpResponseException = ex as HttpResponseException;

			if (httpResponseException != null)
			{
				var httpResponseExceptionDetail = GetHttpResponseExceptionDetail(httpResponseException);

				if (!string.IsNullOrEmpty(httpResponseExceptionDetail))
				{
					Debug.WriteLine(httpResponseExceptionDetail);
				}
			}

			var aggregateException = ex as AggregateException;

			if (aggregateException != null)
			{
				foreach (var innerException in aggregateException.InnerExceptions)
				{
					RecordException(innerException);
				}
			}
		}

		public static void RecordMessage(string message)
		{
			Debug.WriteLine(message);
		}

		public static void RecordHttpRequest(HttpRequestMessage httpRequestMessage)
		{
			if (httpRequestMessage?.RequestUri != null)
			{
				Debug.WriteLine($"Request Absolute URI: {httpRequestMessage.RequestUri.AbsoluteUri}");
			}
		}

		public static string GetHttpResponseExceptionDetail(HttpResponseException httpResponseException)
		{
			var httpResponseExceptionDetail = string.Empty;

			if (httpResponseException != null && httpResponseException.Response != null)
			{
				httpResponseExceptionDetail += $"Http Status Code: {httpResponseException.Response.StatusCode.ToString()}";

				if (httpResponseException.Response.RequestMessage != null)
				{
					httpResponseExceptionDetail += $", Request Uri: {httpResponseException.Response.RequestMessage.RequestUri.AbsoluteUri}";
					httpResponseExceptionDetail += $", Response Content: {httpResponseException.Response.Content.ReadAsStringAsync().Result}";
				}
			}

			return httpResponseExceptionDetail;
		}
	}
}