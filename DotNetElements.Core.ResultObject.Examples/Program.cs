using DotNetElements.Core.ResultObject;
using DotNetElements.Core.ResultObject.Examples;

using static DotNetElements.Core.ResultObject.Examples.ExampleErrorResultHelper;
using static DotNetElements.Core.ResultObject.ResultHelper;

ExampleErrorResult<string> result = GetFailedExampleResult();

if (result.TryGetValue(out string? value, out ExampleError? error))
{
    Console.WriteLine($"Hello, successful Result with value {value}!");
}
else
{
    Console.WriteLine($"Hello, failed Result with error {error}!");
}

static ExampleErrorResult<string> GetFailedExampleResult()
{
    return Fail(ExampleError.BadError);
}

static ExampleErrorResult<string> GetSuccessExampleResult()
{
    return Ok("Hello, World!");
}

static ExampleErrorResult GetFailedExampleResultWithoutValue()
{
    return Fail(ExampleError.BadError);
}

static ExampleErrorResult GetSuccessExampleResultWithoutValue()
{
    return Ok();
}

static Result<string> GetFailedResult()
{
    return Fail();
}

static Result<string> GetSuccessResult()
{
    return Ok("Hello, World!");
}