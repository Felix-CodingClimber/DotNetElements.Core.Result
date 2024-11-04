﻿using System.Text;

namespace DotNetElements.Core.Result.SourceGenerator;

internal static class ErrorResultOutputWriter
{
    public static string WriteErrorResultClass(ErrorResultToGenerate result)
    {
        StringBuilder sb = new();

        sb.Append(@"
//-----------------------------------------------------
// This code was generated by a tool.
//
// Changes to this file may cause incorrect behavior
// and will be lost when the code is regenerated.
// <auto-generated />
//-----------------------------------------------------
#nullable enable

using System.Diagnostics.CodeAnalysis;
");
		if (result.NameSpace is not null)
		{
            sb.Append(@"
namespace ").Append(result.NameSpace).Append(@"
{
    ");
		}
        sb.Append(result.Accessibility).Append(" partial class ").Append(result.FullName).Append(" : ErrorResult<").Append(result.TError).Append(@">
    {
        protected readonly TValue Value;
    
        protected ").Append(result.SimpleName).Append("(bool isOk, TValue value, ").Append(result.TError).Append(@" error) : base(isOk, error)
        {
            Value = value;
        }
    
        public static ").Append(result.FullName).Append(" Fail(").Append(result.TError).Append(@" error) => new(false, default!, error);
    
        // Optional helper functions 
        public bool TryGetValue([NotNullWhen(true)] out TValue? value, [NotNullWhen(false)] out ").Append(result.TError).Append(@"? error)
        {
            value = Value;
            error = Error;
    
            return IsOk;
        }
    
        public TValue GetValueUnsafe() => Value ?? throw new ResultFailException();
    
        // Implicit conversions
        public static implicit operator ").Append(result.FullName).Append(@"(TValue value) => new(true, value, default!);
        public static implicit operator ").Append(result.FullName).Append("(").Append(result.SimpleNameHelper).Append(@" result) => result.IsFail ? new(false, default!, result.GetErrorUnsafe()) : throw new ResultException(""Can not convert a successful result to a result with a value"");
    
        // Optional conversions
        public ").Append(result.SimpleNameHelper).Append(" AsCrudResult() => IsOk ? ").Append(result.SimpleNameHelper).Append(".Ok() : ").Append(result.SimpleNameHelper).Append(@".Fail(Error);
        public Result AsResult() => IsOk ? Result.Ok() : Result.Fail();
        public Result<TValue> AsResultWithValue() => IsOk ? Result<TValue>.Ok(Value) : Result<TValue>.Fail();
    }
        ");

        sb.Append(@"
    ").Append(result.Accessibility).Append(" static partial class ").Append(result.SimpleName).Append(@"Helper
    {
        public class ").Append(result.SimpleName).Append(" : ErrorResult<").Append(result.TError).Append(@">
        {
            private ").Append(result.SimpleName).Append("(bool isOk, ").Append(result.TError).Append(@" error) : base(isOk, error) { }

            public static ").Append(result.SimpleName).Append(@" Ok() => new(true, default!);
            public static ").Append(result.SimpleName).Append(" Fail(").Append(result.TError).Append(@" error) => new(false, error);

            // Optional conversions 
            public Result AsResult => IsOk ? Result.Ok() : Result.Fail();
        }

        public static ").Append(result.SimpleName).Append(" Fail(").Append(result.TError).Append(" error) => ").Append(result.SimpleName).Append(@".Fail(error);
        public static ").Append(result.SimpleName).Append(" Fail(").Append(result.TError).Append(@" error, Action logAction)
        {
            logAction.Invoke();

            return ").Append(result.SimpleName).Append(@".Fail(error);
        }
    }");

        if(result.NameSpace is not null)
        {
		    sb.Append(@"
}");
        }

        return sb.ToString();
    }
}
