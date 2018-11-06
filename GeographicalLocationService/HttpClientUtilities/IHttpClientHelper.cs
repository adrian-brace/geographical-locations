namespace GeographicalLocationService.HttpClientUtilities
{
	using System;

	public interface IHttpClientHelper
	{
		string GetResult(Uri baseRequestUri);

		string GetResult(Uri baseRequestUri, int timeoutSeconds);
	}
}
