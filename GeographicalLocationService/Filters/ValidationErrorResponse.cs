namespace GeographicalLocationService.Filters
{
	using System.Web.Http.ModelBinding;

	public class ValidationErrorResponse
	{
		public ModelStateDictionary ValidationErrors { get; set; }
	}
}