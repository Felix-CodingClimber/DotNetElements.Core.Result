﻿//HintName: ErrorResult.ExampleResult.g.cs

#nullable enable
//-----------------------------------------------------
// This code was generated by a tool.
//
// Changes to this file may cause incorrect behavior
// and will be lost when the code is regenerated.
// <auto-generated />
//-----------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace DotNetElements.Core.Result.Examples
{
    public partial class ExampleResult<TValue> : ErrorResult<int>
    {
        protected readonly TValue Value;
    
        protected ExampleResult(bool isOk, TValue value, int error) : base(isOk, error)
        {
            Value = value;
        }
    
        public static ExampleResult<TValue> Fail(int error) => new(false, default!, error);
    
        // Optional helper functions 
        public bool TryGetValue([NotNullWhen(true)] out TValue? value, [NotNullWhen(false)] out int? error)
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
        
    public static partial class ExampleResultHelper
    {
        public class ExampleResult : ErrorResult<int>
        {
            private ExampleResult(bool isOk, int error) : base(isOk, error) { }

            public static ExampleResult Ok() => new(true, default!);
            public static ExampleResult Fail(int error) => new(false, error);

            // Optional conversions 
            public Result AsResult => IsOk ? Result.Ok() : Result.Fail();
        }

        public static ExampleResult Fail(int error) => ExampleResult.Fail(error);
        public static ExampleResult Fail(int error, Action logAction)
        {
            logAction.Invoke();

            return ExampleResult.Fail(error);
        }
    }
}