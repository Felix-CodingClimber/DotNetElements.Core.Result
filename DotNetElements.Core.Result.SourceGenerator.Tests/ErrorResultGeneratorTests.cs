//using System.Reflection;
//using System.Runtime.InteropServices;
//using DotNetElements.Core.Result.SourceGenerator;
//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp;
//using Microsoft.CodeAnalysis.Emit;

//namespace DotNetElements.Core.Result.Tests;

//[TestClass]
//public partial class ErrorResultGeneratorTests
//{
//	[TestMethod]
//	public void GeneratesErrorResultCorrectly()
//	{
//		string source =
//		@"
//            using DotNetElements.Core.Result;

//            namespace DotNetElements.Core.Result.Examples;

//            [ErrorResult<int>]
//            public partial class ExampleResult<TValue>;
//        ";

//		BuildErrorResultAssembly(source);
//	}

//	public static Assembly BuildErrorResultAssembly(string source)
//	{
//		// Parse the provided string into a C# syntax tree
//		SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source);

//		// Create references for assemblies we require
//		IEnumerable<PortableExecutableReference> references =
//		[
//			MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
//			MetadataReference.CreateFromFile(typeof(ErrorResultAttribute<>).Assembly.Location),
//			MetadataReference.CreateFromFile(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "netstandard.dll")),
//			MetadataReference.CreateFromFile(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "System.Runtime.dll")),
//			MetadataReference.CreateFromFile(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "System.dll"))
//		];

//		// Create a Roslyn compilation for the syntax tree.
//		CSharpCompilation compilation = CSharpCompilation.Create(
//			assemblyName: "Tests",
//			syntaxTrees: [syntaxTree],
//			references: references);

//		// Create an instance of our ErrorResult incremental source generator
//		ErrorResultGenerator generator = new();

//		// The GeneratorDriver is used to run our generator against a compilation
//		GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

//		// Run the source generator!
//		driver = driver.RunGenerators(compilation);

//		using var ms = new MemoryStream();

//		EmitResult result = compilation.Emit(ms);

//		if (!result.Success)
//		{
//			IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
//				diagnostic.IsWarningAsError ||
//				diagnostic.Severity == DiagnosticSeverity.Error);

//			foreach (Diagnostic diagnostic in failures)
//			{
//				Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
//			}
//		}
//		else
//		{
//			ms.Seek(0, SeekOrigin.Begin);
//			Assembly assembly = Assembly.Load(ms.ToArray());

//			return assembly;
//		}
//	}
//}
