namespace GeographicalLocationService.Logging
{
	using System;

	public static class Logger
	{
		public static void RecordException(Exception ex)
		{
			System.Diagnostics.Debug.WriteLine(ex.ToString());
		}
	}
}