//HintName: ErrorResult.ExampleResult.g.cs

using System.Diagnostics.CodeAnalysis;

namespace DotNetElements.Core.Result
{
    internal partial class ExampleResult<TValue> : ErrorResult<string>
    {
        protected readonly TValue Value;
    
        protected ExampleResult(bool isOk, TValue value, string error) : base(isOk, error)
        {
            Value = value;
        }
    
        public static ExampleResult<TValue> Fail(string error) => new(false, default!, error);
    
        // Optional helper functions 
        public bool TryGetValue([NotNullWhen(true)] out TValue? value, [NotNullWhen(false)] out string? error)
        {
            value = Value;
            error = Error;
    
            return IsOk;
        }
    
        public TValue GetValueUnsafe() => Value ?? throw new ResultFailException();
    
        // Implicit conversions
        public static implicit operator ExampleResult<TValue>(TValue value) => new(true, value, default!);
        public static implicit operator ExampleResult<TValue>(ExampleResultHelper.ExampleResult result) => result.IsFail ? new(false, default!, result.GetErrorUnsafe()) : throw new ResultException("Can not convert a successful result to a result with a value");
    
        // Optional conversions
        public ExampleResultHelper.ExampleResult AsCrudResult() => IsOk ? ExampleResultHelper.ExampleResult.Ok() : ExampleResultHelper.ExampleResult.Fail(Error);
        public Result AsResult() => IsOk ? Result.Ok() : Result.Fail();
        public Result<TValue> AsResultWithValue() => IsOk ? Result<TValue>.Ok(Value) : Result<TValue>.Fail();
    }
    internal static partial class ExampleResultHelper
    {
        public class ExampleResult : ErrorResult<string>
        {
            private ExampleResult(bool isOk, string error) : base(isOk, error) { }

            public static ExampleResult Ok() => new(true, default!);
            public static ExampleResult Fail(string error) => new(false, error);

            // Optional conversions 
            public Result AsResult => IsOk ? Result.Ok() : Result.Fail();
        }

        public static ExampleResult Fail(string error) => ExampleResult.Fail(error);
        public static ExampleResult Fail(string error, Action logAction)
        {
            logAction.Invoke();

            return ExampleResult.Fail(error);
        }
    }}