﻿using DotNetElements.Core.ResultObject;
using DotNetElements.Core.ResultObject.Examples;

using static DotNetElements.Core.ResultObject.Examples.ExampleErrorResultHelper;

ExampleErrorResult<string> result = GetExampleResult();

if (result.TryGetValue(out string? value, out ExampleError? error))
{
	Console.WriteLine($"Hello, successful Result with value {value}!");
}
else
{
	Console.WriteLine($"Hello, failed Result with error {error}!");
}

static ExampleErrorResult<string> GetExampleResult()
{
	return Fail(ExampleError.BadError);
}
