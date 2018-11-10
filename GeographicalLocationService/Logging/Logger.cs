namespace GeographicalLocationService.Logging
{
	using System;
	using System.Diagnostics;
	using System.Net.Http;

	public static class Logger
	{
		public static void RecordException(Exception ex)
		{
			Debug.WriteLine(ex.ToString());
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
	}
}