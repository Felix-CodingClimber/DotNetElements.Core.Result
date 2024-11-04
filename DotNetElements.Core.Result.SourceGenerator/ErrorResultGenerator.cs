using System.Text;
using DotNetElements.Core.Result.SourceGenerator.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace DotNetElements.Core.Result.SourceGenerator;

[Generator]
public class ErrorResultGenerator : IIncrementalGenerator
{
    private const string ErrorResultAttributeNamespace = "DotNetElements.Core.Result";
    private const string ErrorResultAttributeShortName = "ErrorResult";
    private const string ErrorResultAttributeTypeName = $"{ErrorResultAttributeShortName}Attribute`1";
    private const string ErrorResultAttributeFullName = $"{ErrorResultAttributeNamespace}.{ErrorResultAttributeTypeName}";

	public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<ErrorResultToGenerate?> classesToGenerate = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                ErrorResultAttributeFullName,
                predicate: static (s, _) => true,
                transform: static (ctx, _) => GetErrorResultsToGenerate(ctx.SemanticModel, ctx.TargetNode))
            .Where(static m => m is not null);

        // Generate source code for each attribute found
        context.RegisterSourceOutput(classesToGenerate, static (spc, source) => Execute(spc, source));
    }

    private static void Execute(SourceProductionContext context, ErrorResultToGenerate? errorResultToGenerate)
    {
        if (errorResultToGenerate is { } value)
        {
            string result = ErrorResultOutputWriter.WriteErrorResultClass(value);
            context.AddSource($"ErrorResult.{value.SimpleName}.g.cs", SourceText.From(result, Encoding.UTF8));
        }
    }

    private static ErrorResultToGenerate? GetErrorResultsToGenerate(SemanticModel semanticModel, SyntaxNode classDeclarationSyntax)
    {
        // Get the semantic representation of the class syntax
        if (semanticModel.GetDeclaredSymbol(classDeclarationSyntax) is not INamedTypeSymbol classSymbol)
            return null;

        if (!classSymbol.IsGenericType || classSymbol.TypeArguments.Length != 1)
            return null;

        // Get the full type name of the class
        string classNameWithoutTypeParameters = classSymbol.Name;
        string classNameFull = classSymbol.ToString();
        string typeArgument = classSymbol.TypeArguments[0].ToString();
        string accessibility = classSymbol.DeclaredAccessibility.ToStringFast();
        string? attributeTypeArgument = null;

		// Get more information from the ErrorResultAttribute
		foreach (AttributeData attributeData in classSymbol.GetAttributes())
        {
            if (attributeData.AttributeClass is not { MetadataName: ErrorResultAttributeTypeName, IsGenericType: true })
                continue;

			attributeTypeArgument = attributeData.AttributeClass.TypeArguments[0].ToString();
		}

        if (attributeTypeArgument is null)
            return null;

		return new ErrorResultToGenerate(classNameFull, classNameWithoutTypeParameters, accessibility, typeArgument, attributeTypeArgument);
    }
}
