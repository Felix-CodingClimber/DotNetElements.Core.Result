using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace DotNetElements.Core.Result.SourceGenerator;

[Generator]
public class ErrorResultGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<ErrorResultToGenerate?> classesToGenerate = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                typeof(ErrorResultAttribute).FullName,
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
            string result = ErrorResultGenerationHelper.GenerateErrorResultClass(value);
            context.AddSource($"ErrorResult.{value.SimpleName}.g.cs", SourceText.From(result, Encoding.UTF8));
        }
    }

    private static ErrorResultToGenerate? GetErrorResultsToGenerate(SemanticModel semanticModel, SyntaxNode classDeclarationSyntax)
    {
        // Get the semantic representation of the class syntax
        if (semanticModel.GetDeclaredSymbol(classDeclarationSyntax) is not INamedTypeSymbol classSymbol)
            return null;

        // Get the full type name of the class
        string classNameWithoutTypeParameters = classSymbol.Name;
        string classNameFull = classSymbol.ToString();
        string typeArgument = classSymbol.TypeArguments[0].ToString();

        return new ErrorResultToGenerate(classNameFull, classNameWithoutTypeParameters, typeArgument); // todo include attribute type parameter for TError
    }
}
