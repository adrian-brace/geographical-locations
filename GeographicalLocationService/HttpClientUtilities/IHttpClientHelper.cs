namespace GeographicalLocationService.HttpClientUtilities
{
	using System;

	public interface IHttpClientHelper
	{
		RS GetResponse<RS>(Uri baseRequestUri);

		RS GetResponse<RS>(Uri baseRequestUri, int timeoutSeconds);
	}
}
