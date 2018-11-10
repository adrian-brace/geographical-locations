namespace GeographicalLocationService.Caching
{
	using System;
	using System.Runtime.Caching;

	public static class CacheUtilities
	{
		public static T GetObjectFromCache<T>(string cacheItemName, int cacheTimeInMinutes, Func<string, T> objectSettingFunction, string functionParameter)
		{
			if (objectSettingFunction == null)
			{
				throw new ArgumentNullException("objectSettingFunction");
			}

			ObjectCache cache = MemoryCache.Default;
			var cachedObject = (T)cache[cacheItemName];

			if (cachedObject == null)
			{
				CacheItemPolicy policy = new CacheItemPolicy();
				policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(cacheTimeInMinutes);
				cachedObject = objectSettingFunction(functionParameter);

				if (cachedObject != null)
				{
					cache.Set(cacheItemName, cachedObject, policy);
				}
			}

			return cachedObject;
		}

		public static T GetObjectFromCache<T>(string cacheItemName, int cacheTimeInMinutes, Func<T> objectSettingFunction)
		{
			if (objectSettingFunction == null)
			{
				throw new ArgumentNullException("objectSettingFunction");
			}

			ObjectCache cache = MemoryCache.Default;
			var cachedObject = (T)cache[cacheItemName];

			if (cachedObject == null)
			{
				CacheItemPolicy policy = new CacheItemPolicy();
				policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(cacheTimeInMinutes);
				cachedObject = objectSettingFunction();

				if (cachedObject != null)
				{
					cache.Set(cacheItemName, cachedObject, policy);
				}
			}

			return cachedObject;
		}
	}
}