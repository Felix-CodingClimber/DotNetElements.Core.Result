namespace DotNetElements.Core.Result.SourceGenerator;

internal readonly record struct ErrorResultToGenerate
{
    public readonly string FullName;
    public readonly string SimpleName;
	public readonly string Accessibility;
	public readonly string TValue;
    public readonly string TError;

    public string SimpleNameHelper => $"{SimpleName}Helper.{SimpleName}";

    public ErrorResultToGenerate(string fullName, string nameWithoutTypeParameters, string accessibility, string tValue, string tError)
    {
        FullName = fullName;
        SimpleName = nameWithoutTypeParameters;
		Accessibility = accessibility;
		TValue = tValue;
        TError = tError;
    }
}
