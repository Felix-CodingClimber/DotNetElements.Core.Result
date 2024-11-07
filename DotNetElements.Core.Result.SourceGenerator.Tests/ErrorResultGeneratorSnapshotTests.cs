using System.Runtime.InteropServices;
using DotNetElements.Core.Result.SourceGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace DotNetElements.Core.Result.Tests;

[TestClass]
[UsesVerify]
public partial class ErrorResultGeneratorSnapshotTests
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
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source);

        IEnumerable<PortableExecutableReference> references =
        [
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(ErrorResultAttribute<>).Assembly.Location),
            MetadataReference.CreateFromFile(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "netstandard.dll")),
            MetadataReference.CreateFromFile(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "System.Runtime.dll")),
            MetadataReference.CreateFromFile(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "System.dll"))
        ];

        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName: "Tests",
            syntaxTrees: [syntaxTree],
            references: references);

        ErrorResultGenerator generator = new();

        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        driver = driver.RunGenerators(compilation);

        return Verify(driver).UseDirectory("SnapshotResults");
    }
}
