using Microsoft.CodeAnalysis;

namespace DotNetElements.Core.Result.SourceGenerator.Utils;

internal static class AccessibilityExtensions
{
	public static string ToStringFast(this Accessibility accessibility)
	{
		return accessibility switch
		{
			Accessibility.Public => "public",
			Accessibility.Internal => "internal",
			Accessibility.Private => "private",
			Accessibility.Protected => "protected",
			Accessibility.ProtectedAndInternal => "protected internal",
			Accessibility.ProtectedOrInternal => "protected internal",
			_ => throw new NotImplementedException(nameof(accessibility))
		};
	}
}
