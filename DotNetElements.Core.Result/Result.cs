/*
using static ResultHelper;

///////////////////////////////////////////////////////////////////////////////////// 
// Usage example of Result: Call using the .Out semantic 
///////////////////////////////////////////////////////////////////////////////////// 
if (GetResultFunction(true).TryGetValue(out string? resultValue))
{
    Console.WriteLine($"Success from caller with value <{resultValue}>");
}
else
{
    Console.WriteLine("Fail from caller";
}

///////////////////////////////////////////////////////////////////////////////////// 
// Usage example of Result: Get value using manual check and GetValueUnsafe() 
///////////////////////////////////////////////////////////////////////////////////// 
Result<string> result = GetResultFunction(false);

if (result.IsOk)
{
    Console.WriteLine($"Success from caller with value <{result.GetValueUnsafe()}>");
}
else
{
    Console.WriteLine("Fail from caller");
}

///////////////////////////////////////////////////////////////////////////////////// 
// Usage example of Result: Explicit conversions to prevent data loss 
///////////////////////////////////////////////////////////////////////////////////// 
Result resultWithoutValue = result.AsResult();

///////////////////////////////////////////////////////////////////////////////////// 
// Usage example of Result: Method implementation 
///////////////////////////////////////////////////////////////////////////////////// 
static Result<string> GetResultFunction(bool expectedResult)
{
    ILogger logger = new Logger();

    // With logging 
    if (!expectedResult)
        return Fail(() => logger.LogError("Fail from method"));

    // Without logging 
    if (!expectedResult)
        return Fail();

    return "Success from method";
}

///////////////////////////////////////////////////////////////////////////////////// 
// Usage example of ErrorResult: Call using the .Out semantic 
///////////////////////////////////////////////////////////////////////////////////// 
if (GetCrudResultFunction(true).TryGetValue(out string? crudValue, out CrudError error))
{
    Console.WriteLine($"Success from caller with value <{crudValue}>");
}
else
{
    Console.WriteLine("Fail from caller");
}

///////////////////////////////////////////////////////////////////////////////////// 
// Usage example of ErrorResult: Call, get result as var and check result using .IsOk 
///////////////////////////////////////////////////////////////////////////////////// 
CrudResult<string> crudResult = GetCrudResultFunction(false);

if (crudResult.IsOk)
{
    Console.WriteLine($"Success from caller with value <{crudResult.GetValueUnsafe()}>");
}
else
{
    Console.WriteLine($"Fail from caller with error <{crudResult.Error}>");
}

///////////////////////////////////////////////////////////////////////////////////// 
// Usage example of ErrorResult: Explicit conversions to prevent data loss 
///////////////////////////////////////////////////////////////////////////////////// 
CrudResult crudResultWithoutValue = crudResult.AsCrudResult();
Result<string> simpleResultWithValue = crudResult.AsResultWithValue();
Result simpleResultWithoutValue = crudResult.AsResult();

///////////////////////////////////////////////////////////////////////////////////// 
// Usage example of ErrorResult: Method implementation 
///////////////////////////////////////////////////////////////////////////////////// 
static CrudResult<string> GetCrudResultFunction(bool expectedResult)
{
    ILogger logger = new Logger();

    // With logging 
    if (!expectedResult)
        return Fail(CrudError.InternalError, () => logger.LogError("Fail from method"));

    // Without logging 
    if (!expectedResult)
        return Fail(CrudError.InternalError);

    return "Success from method";
}

///////////////////////////////////////////////////////////////////////////////////// 
// Usage example of ErrorResult: Derived class implementation 
///////////////////////////////////////////////////////////////////////////////////// 

// This needs to be implemented by the user per specific error type
[ErrorResult<CrudError>]
partial class CrudResult<TValue>;

enum CrudError
{
    InternalError,
    NotFound,
    DuplicateEntry
}

// This gets implemented by a source generator 
internal partial class CrudResult<TValue> : ErrorResult<CrudError>
{
    protected readonly TValue Value;

    protected CrudResult(bool isOk, TValue value, CrudError error) : base(isOk, error)
    {
        Value = value;
    }

    public static CrudResult<TValue> Fail(CrudError error) => new(false, default!, error);

    // Optional helper functions 
    public bool TryGetValue([NotNullWhen(true)] out TValue? value, [NotNullWhen(false)] out CrudError? error)
    {
        value = Value;
        error = Error;

        return IsOk;
    }

    public TValue GetValueUnsafe() => Value ?? throw new ResultFailException();

    // Implicit conversions
    public static implicit operator CrudResult<TValue>(TValue value) => new(true, value, default!);
    public static implicit operator CrudResult<TValue>(CrudResult result) => result.IsFail ? new(false, default!, result.GetErrorUnsafe()) : throw new ResultException("Can not convert a successful result to a result with a value");

    // Optional conversions 
    public CrudResult AsCrudResult() => IsOk ? CrudResult.Ok() : CrudResult.Fail(Error);
    public Result AsResult() => IsOk ? Result.Ok() : Result.Fail();
    public Result<TValue> AsResultWithValue() => IsOk ? Result<TValue>.Ok(Value) : Result<TValue>.Fail();
}

internal static partial class ResultHelper
{
    public class CrudResult : ErrorResult<CrudError>
    {
        private CrudResult(bool isOk, CrudError error) : base(isOk, error) { }

        public static CrudResult Ok() => new(true, default!);
        public static CrudResult Fail(CrudError error) => new(false, error);

        // Optional conversions 
        public Result AsResult => IsOk ? Result.Ok() : Result.Fail();
    }

    public static CrudResult Fail(CrudError error) => CrudResult.Fail(error);
    public static CrudResult Fail(CrudError error, Action logAction)
    {
        logAction.Invoke();

        return CrudResult.Fail(error);
    }
}
*/

/////////////////////////////////////////////////////////////////////////////////////
// Core implementation 
/////////////////////////////////////////////////////////////////////////////////////

using System.Diagnostics.CodeAnalysis;

namespace DotNetElements.Core.Result;

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

public readonly struct ResultOk;
public readonly struct ResultFail;

public static partial class ResultHelper
{
    public static ResultOk Ok() => new ResultOk();

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