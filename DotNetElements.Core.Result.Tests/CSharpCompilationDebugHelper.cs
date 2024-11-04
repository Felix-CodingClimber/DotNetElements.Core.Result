using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace DotNetElements.Core.Result.Tests;

internal static class CSharpCompilationDebugHelper
{
    /// <summary>
    /// Emit the compilation result to a DLL file.
    /// </summary>
    /// <param name="compilation"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public static void EmitCompilationResult(CSharpCompilation compilation)
    {
        string outputPath = Path.Combine(Path.GetTempPath(), "GeneratedAssembly.dll");

        Microsoft.CodeAnalysis.Emit.EmitResult emitResult = compilation.Emit(outputPath);

        if (!emitResult.Success)
        {
            // Handle the case where the compilation failed
            foreach (Diagnostic diagnostic in emitResult.Diagnostics)
            {
                System.Diagnostics.Debug.WriteLine(diagnostic.ToString());
            }

            throw new InvalidOperationException("Compilation failed.");
        }

        System.Diagnostics.Debug.WriteLine($"Compilation successful. DLL written to: {outputPath}");
    }
}
