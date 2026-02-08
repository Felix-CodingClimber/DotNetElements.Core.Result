using System.Diagnostics.CodeAnalysis;

namespace DotNetElements.Core.ResultObject;

public readonly struct Result<TValue>
{
    public bool IsOk { get; private init; }

    public bool IsFail => !IsOk;

    private readonly TValue value;

    private Result(bool isOk, TValue value)
    {
        IsOk = isOk;
        this.value = value;
    }

    public static Result<TValue> Ok(TValue value) => new(true, value);
    public static Result<TValue> Fail() => new Result<TValue>(false, default!);

    public static implicit operator Result<TValue>(TValue value) => new(true, value);
    public static implicit operator Result<TValue>(ResultFail _) => new(false, default!);

    // Optional conversions 
    public Result AsResult() => IsOk ? Result.Ok() : Result.Fail();

    // Optional helper functions 
    public bool TryGetValue([NotNullWhen(true)] out TValue? value)
    {
        value = this.value;

        return IsOk;
    }

    public TValue GetValueUnsafe() => value ?? throw new ResultFailException();
}

public readonly struct Result
{
    public bool IsOk { get; private init; }

    public bool IsFail => !IsOk;

    private Result(bool isOk)
    {
        IsOk = isOk;
    }

    public static Result Ok() => new(true);
    public static Result Fail() => new(false);

    public static implicit operator Result(ResultOk _) => new Result(true);
    public static implicit operator Result(ResultFail _) => new Result(false);
}

public readonly struct ResultWithError
{
    public bool IsOk { get; private init; }

    public bool IsFail => !IsOk;

    private readonly string? error;

    private ResultWithError(bool isOk, string? error = null)
    {
        IsOk = isOk;
        this.error = error;
    }

    public static ResultWithError Ok() => new(true);
    public static ResultWithError Fail(string error) => new(false, error);

    public bool HasError([NotNullWhen(true)] out string? error)
    {
        error = this.error;

        return IsFail;
    }

    public string GetErrorUnsafe() => error ?? throw new ResultOkException();

    // Optional conversions
    public Result AsResult => IsOk ? Result.Ok() : Result.Fail();

    // Helper methods
    public static ResultWithError OkIf(bool isSuccess, string error)
    {
        return isSuccess ? Ok() : Fail(error);
    }

    public static ResultWithError OkIf(Func<bool> predicate, string error)
    {
        return predicate.Invoke() ? Ok() : Fail(error);
    }

    public static ResultWithError OkIfNotNull<TValue>(TValue? value, string error)
    {
        return value is not null ? Ok() : Fail(error);
    }
}

public abstract class ErrorResult<TError>
{
    public bool IsOk { get; private init; }

    public bool IsFail => !IsOk;

    protected readonly TError Error;

    protected ErrorResult(bool isOk, TError error)
    {
        IsOk = isOk;
        Error = error;
    }

    public bool HasError([NotNullWhen(true)] out TError? error)
    {
        error = Error;

        return IsFail;
    }

    public TError GetErrorUnsafe() => Error ?? throw new ResultOkException();
}

// todo check if this is needed
public readonly struct ResultOk;
// todo check if this is needed
public readonly struct ResultFail;

public static partial class ResultHelper
{
    public static ResultOk Ok() => new ResultOk();
    public static Result<TValue> Ok<TValue>(TValue value) => Result<TValue>.Ok(value);

    public static ResultFail Fail() => new ResultFail();
    public static ResultFail Fail(Action logAction)
    {
        logAction.Invoke();

        return new ResultFail();
    }
}

public class ResultException(string message) : Exception(message);
public class ResultOkException(string message = "Can not get a error for a successful result") : Exception(message);
public class ResultFailException(string message = "Can not get a value for a failed result") : Exception(message);