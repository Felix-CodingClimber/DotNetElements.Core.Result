using System.Runtime.InteropServices;
using DotNetElements.Core.Result.SourceGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace DotNetElements.Core.Result.Tests;

[TestClass]
[UsesVerify]
public partial class ErrorResultGeneratorSnapshotTest
{
    [TestMethod]
    public Task GeneratesErrorResultCorrectly()
    {
        string source =
		@"
            using DotNetElements.Core.Result;

            namespace DotNetElements.Core.Result.Examples;

            [ErrorResult<int>]
            public partial class ExampleResult<TValue>;
        ";

        return VerifySourceGenerator(source);
    }

    public static Task VerifySourceGenerator(string source)
    {
        // Parse the provided string into a C# syntax tree
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source);

        // Create references for assemblies we require
        IEnumerable<PortableExecutableReference> references =
        [
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(ErrorResultAttribute<>).Assembly.Location),
            MetadataReference.CreateFromFile(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "netstandard.dll")),
            MetadataReference.CreateFromFile(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "System.Runtime.dll")),
            MetadataReference.CreateFromFile(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "System.dll"))
        ];

        // Create a Roslyn compilation for the syntax tree.
        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName: "Tests",
            syntaxTrees: [syntaxTree],
            references: references);

        // Create an instance of our ErrorResult incremental source generator
        ErrorResultGenerator generator = new();

        // The GeneratorDriver is used to run our generator against a compilation
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        // Run the source generator!
        driver = driver.RunGenerators(compilation);

        return Verify(driver);
    }
}
