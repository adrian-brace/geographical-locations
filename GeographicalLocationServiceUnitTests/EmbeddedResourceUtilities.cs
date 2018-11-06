namespace GeographicalLocationServiceUnitTests
{
	using System.IO;
	using System.Reflection;

	public static class EmbeddedResourceUtilities
	{
		public static string ReadFromEmbeddedResource(string resourceName)
		{
			string jsonFromFile = string.Empty;
			var assembly = Assembly.GetExecutingAssembly();

			Stream stream = null;

			try
			{
				stream = assembly.GetManifestResourceStream(resourceName);

				using (var reader = new StreamReader(stream))
				{
					stream = null;
					jsonFromFile = reader.ReadToEnd();
				}
			}
			finally
			{
				if (stream != null)
				{
					stream.Dispose();
				}
			}

			return jsonFromFile;
		}
	}
}
