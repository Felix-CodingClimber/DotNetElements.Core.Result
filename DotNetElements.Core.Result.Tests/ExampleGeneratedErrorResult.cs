using System.Diagnostics.CodeAnalysis;

namespace DotNetElements.Core.Result.Tests;

file partial class ExampleErrorResult<TValue> : ErrorResult<string>
{
    protected readonly TValue Value;

    protected ExampleErrorResult(bool isOk, TValue value, string error) : base(isOk, error)
    {
        Value = value;
    }

    public static ExampleErrorResult<TValue> Fail(string error) => new(false, default!, error);

    // Optional helper functions 
    public bool TryGetValue([NotNullWhen(true)] out TValue? value, [NotNullWhen(false)] out string? error)
    {
        value = Value;
        error = Error;

        return IsOk;
    }

    public TValue GetValueUnsafe() => Value ?? throw new ResultFailException();

    // Implicit conversions
    public static implicit operator ExampleErrorResult<TValue>(TValue value) => new(true, value, default!);
    public static implicit operator ExampleErrorResult<TValue>(ExampleResultHelper.ExampleErrorResult result) => result.IsFail ? new(false, default!, result.GetErrorUnsafe()) : throw new ResultException("Can not convert a successful result to a result with a value");

    // Optional conversions 
    public ExampleResultHelper.ExampleErrorResult AsCrudResult() => IsOk ? ExampleResultHelper.ExampleErrorResult.Ok() : ExampleResultHelper.ExampleErrorResult.Fail(Error);
    public Result AsResult() => IsOk ? Result.Ok() : Result.Fail();
    public Result<TValue> AsResultWithValue() => IsOk ? Result<TValue>.Ok(Value) : Result<TValue>.Fail();
}

file static partial class ExampleResultHelper
{
    public class ExampleErrorResult : ErrorResult<string>
    {
        private ExampleErrorResult(bool isOk, string error) : base(isOk, error) { }

        public static ExampleErrorResult Ok() => new(true, default!);
        public static ExampleErrorResult Fail(string error) => new(false, error);

        // Optional conversions 
        public Result AsResult => IsOk ? Result.Ok() : Result.Fail();
    }

    public static ExampleErrorResult Fail(string error) => ExampleErrorResult.Fail(error);
    public static ExampleErrorResult Fail(string error, Action logAction)
    {
        logAction.Invoke();

        return ExampleErrorResult.Fail(error);
    }
}
