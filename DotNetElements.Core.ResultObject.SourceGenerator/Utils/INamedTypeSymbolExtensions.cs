using Microsoft.CodeAnalysis;

namespace DotNetElements.Core.ResultObject.SourceGenerator.Utils;

internal static class INamedTypeSymbolExtensions
{
	private static readonly SymbolDisplayFormat getFullNamespaceSymbolDisplayFormat = 
		new(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);

	public static string? GetFullNamespace(this INamedTypeSymbol namedTypeSymbol)
	{
		string fullyQualifiedName = namedTypeSymbol.ToDisplayString(getFullNamespaceSymbolDisplayFormat);

		return fullyQualifiedName.Replace(namedTypeSymbol.Name, "").TrimEnd('.');
	}
}
