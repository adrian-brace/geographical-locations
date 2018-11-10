namespace GeographicalLocationService.Validation
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.Text.RegularExpressions;

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public sealed class ISODateTimeAttribute : ValidationAttribute
	{
		private const string ISODateExpression = @"^\d{4}-\d{2}-\d{2}$";

		public override bool IsValid(object value)
		{
			return Regex.IsMatch((string)value, ISODateExpression) && DateTime.TryParse((string)value, out var date);
		}

		public override string FormatErrorMessage(string name)
		{
			return $"Invalid input: {name}. Date input must conform to this ISO Date format and be a valid date. [yyyy-MM-dd]";
		}
	}
}