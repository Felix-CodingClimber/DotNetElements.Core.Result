namespace DotNetElements.Core.Result.SourceGenerator;

internal readonly record struct ErrorResultToGenerate
{
    public readonly string SimpleName;
	public readonly string Accessibility;
	public readonly string TValue;
    public readonly string TError;

    public readonly string FullName => $"{SimpleName}<{TValue}>";
    public string SimpleNameHelper => $"{SimpleName}Helper.{SimpleName}";

    public ErrorResultToGenerate(string nameWithoutTypeParameters, string accessibility, string tValue, string tError)
    {
        SimpleName = nameWithoutTypeParameters;
		Accessibility = accessibility;
		TValue = tValue;
        TError = tError;
    }
}
