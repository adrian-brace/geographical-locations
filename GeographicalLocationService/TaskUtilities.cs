namespace GeographicalLocationService
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using GeographicalLocationService.Logging;

	public class TaskUtilities
	{
		public static void WaitForTasks(List<Task> tasks)
		{
			try
			{
				Task.WaitAll(tasks.ToArray());
			}
			catch (AggregateException aex)
			{
				foreach (var ex in aex.InnerExceptions)
				{
					Logger.RecordException(ex);
				}

				throw;
			}
		}
	}
}