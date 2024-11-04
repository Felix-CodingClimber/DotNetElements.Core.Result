namespace DotNetElements.Core.Result.SourceGenerator;

internal readonly record struct ErrorResultToGenerate
{
    public readonly string FullName;
    public readonly string SimpleName;
    public readonly string TValue;
    public readonly string TError;

    public string SimpleNameHelper => $"{SimpleName}Helper.{SimpleName}";

    public ErrorResultToGenerate(string fullName, string nameWithoutTypeParameters, string tValue = "TValue", string tError = "string") // todo remove default
    {
        FullName = fullName;
        SimpleName = nameWithoutTypeParameters;
        TValue = tValue;
        TError = tError;
    }
}
