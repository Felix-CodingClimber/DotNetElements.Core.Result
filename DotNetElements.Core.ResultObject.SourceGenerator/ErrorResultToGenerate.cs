namespace DotNetElements.Core.ResultObject.SourceGenerator;

internal readonly record struct ErrorResultToGenerate
{
    public readonly string? NameSpace;
    public readonly string SimpleName;
	public readonly string Accessibility;
	public readonly string TValue;
    public readonly string TError;

    public readonly string FullName => $"{SimpleName}<{TValue}>";
    public string SimpleNameHelper => $"{SimpleName}Helper.{SimpleName}";

    public ErrorResultToGenerate(string? nameSpace, string nameWithoutTypeParameters, string accessibility, string tValue, string tError)
    {
        NameSpace = nameSpace;
        SimpleName = nameWithoutTypeParameters;
		Accessibility = accessibility;
		TValue = tValue;
        TError = tError;
    }
}
