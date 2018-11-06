namespace GeographicalLocationService.HttpClientUtilities
{
	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Net.Http;
	using System.Threading;
	using System.Threading.Tasks;

	public class HttpClientHelper : IHttpClientHelper
	{
		private const int DefaultRequestTimeoutSeconds = 30;

		[SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope", Justification = "The recommended usage of HttpClient is not to dispose of it after every request.")]
		private static readonly HttpClient HttpClient = new HttpClient()
		{
			// Default to 30 minutes and let each individual request override to DefaultRequestTimeoutSeconds where applicable using Cancellation Token
			Timeout = TimeSpan.FromSeconds(120)
		};

		/// <summary>
		/// Returns the response as json
		/// </summary>
		/// <param name="baseRequestUri">The base request URI.</param>
		/// <returns>
		/// Response string
		/// </returns>
		public string GetResult(Uri baseRequestUri)
		{
			var result = Task.Run(() => this.GetAsync(baseRequestUri, DefaultRequestTimeoutSeconds)).Result;
			result.EnsureSuccessStatusCode();
			return result.Content.ReadAsStringAsync().Result;
		}

		/// <summary>
		/// Returns the response as json
		/// </summary>
		/// <param name="baseRequestUri">The base request URI.</param>
		/// <param name="timeoutSeconds">The timeout seconds.</param>
		/// <returns>
		/// Response string
		/// </returns>
		public string GetResult(Uri baseRequestUri, int timeoutSeconds)
		{
			var result = Task.Run(() => this.GetAsync(baseRequestUri, timeoutSeconds)).Result;
			result.EnsureSuccessStatusCode();
			return result.Content.ReadAsStringAsync().Result;
		}

		/// <summary>
		/// Gets the asynchronous task.
		/// </summary>
		/// <param name="baseRequestUri">The base request URI.</param>
		/// <param name="timeoutSeconds">The timeout seconds.</param>
		/// <returns>
		/// Asynchronous task
		/// </returns>
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