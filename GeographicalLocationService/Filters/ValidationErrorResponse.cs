namespace GeographicalLocationService.Filters
{
	using System.Web.Http.ModelBinding;

	/// <summary>
	/// Validation Error Response
	/// </summary>
	public class ValidationErrorResponse
	{
		/// <summary>
		/// Gets or sets the validation errors.
		/// </summary>
		/// <value>
		/// The validation errors.
		/// </value>
		public ModelStateDictionary ValidationErrors { get; set; }
	}
}