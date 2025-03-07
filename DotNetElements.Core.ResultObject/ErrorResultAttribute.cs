namespace DotNetElements.Core.ResultObject;

[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class ErrorResultAttribute<TError> : System.Attribute;