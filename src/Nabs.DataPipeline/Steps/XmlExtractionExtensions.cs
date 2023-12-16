namespace Nabs.DataPipeline.Steps;

public static class XmlExtractionExtensions
{
	public static string ExtractStringValue(this XElement? element)
	{
		var value = element?.Value;
		return value ?? string.Empty;
	}

	public static string? ExtractStringValueOrNull(this XElement? element)
	{
		var value = element?.Value;

		return value;
	}

	public static Guid ExtractGuidValue(this XElement? element)
	{
		var value = element?.Value is not null 
			? Guid.Parse(element.Value) 
			: Guid.Empty;
		return value;
	}

	public static DateOnly ExtractDateOnlyValue(this XElement? element)
	{
		var value = element?.Value is not null 
			? DateOnly.Parse(element.Value) 
			: default;

		return value;
	}

	public static Guid ExtractGuidValue(this XAttribute? attribute)
	{
		var value = attribute?.Value is not null 
			? Guid.Parse(attribute.Value) 
			: Guid.Empty;
		return value;
	}
}
