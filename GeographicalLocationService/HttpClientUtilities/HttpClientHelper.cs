namespace GeographicalLocationService.HttpClientUtilities
{
	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Net.Http;
	using System.Threading;
	using System.Threading.Tasks;
	using Newtonsoft.Json;

	public class HttpClientHelper : IHttpClientHelper
	{
		private const int DefaultRequestTimeoutSeconds = 30;

		[SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope", Justification = "The recommended usage of HttpClient is not to dispose of it after every request.")]
		private static readonly HttpClient HttpClient = new HttpClient()
		{			
			Timeout = TimeSpan.FromSeconds(120)
		};

		public RS GetResponse<RS>(Uri baseRequestUri)
		{
			var result = Task.Run(() => GetAsync(baseRequestUri, DefaultRequestTimeoutSeconds)).Result;
			result.EnsureSuccessStatusCode();
			return JsonConvert.DeserializeObject<RS>(result.Content.ReadAsStringAsync().Result);
		}

		public RS GetResponse<RS>(Uri baseRequestUri, int timeoutSeconds)
		{
			var result = Task.Run(() => GetAsync(baseRequestUri, timeoutSeconds)).Result;
			result.EnsureSuccessStatusCode();
			return JsonConvert.DeserializeObject<RS>(result.Content.ReadAsStringAsync().Result);
		}

		private async Task<HttpResponseMessage> GetAsync(Uri baseRequestUri, int timeoutSeconds)
		{
			using (var cancellationTokenSource = new CancellationTokenSource())
			{
				cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(timeoutSeconds));
				return await HttpClient.GetAsync(baseRequestUri, cancellationTokenSource.Token);
			}
		}
	}
}