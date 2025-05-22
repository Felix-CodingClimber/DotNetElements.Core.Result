## About

This project provides simple `Result` and `Result<TValue>` types to be used as return types of functions as an alternative to exceptions.

The default types have a .IsOk and .IsFail property to check wether the function returned success or failure. In case of a Result<TValue> the result contains the return value of the function.

To include error state in the Result, one has to implement a custom `ErrorResult<TValue, TError>`.
A source generator automatically provides the class implementation.

There are helper functions like `Fail()`, `Ok()` or `TryGetValue(out string? resultValue)` to make working with Results as simple as possible.

## Recommended setup

1. Install nuget package `> dotnet add package DotNetElements.Core.Result --version <insert-latest-version-here>`

2. Add the following to a GlobalUsing.cs
```cs
global using static DotNetElements.Core.ResultObject.ResultHelper;
```

## Usage examples for basic Result and Result<TValue> types

### Example function using a Result as return type
```cs
static Result<string> GetResultFunction(bool expectedResult)
{
    // With logging
    if (!expectedResult)
        return Fail(() => Console.WriteLine("Fail from method"));

    // Without logging
    if (!expectedResult)
        return Fail();

    return "Success from method";
}
```

### Usage example using the .TryGetValue semantic
```cs
if (GetResultFunction(true).TryGetValue(out string? resultValue))
{
    Console.WriteLine($"Success from caller with value <{resultValue}>");
}
else
{
    Console.WriteLine("Fail from caller";
}
```

### Usage example using manual check and GetValueUnsafe()
```cs
Result<string> result = GetResultFunction(false);

if (result.IsOk)
{
    Console.WriteLine($"Success from caller with value <{result.GetValueUnsafe()}>");
}
else
{
    Console.WriteLine("Fail from caller");
}
```

### Usage example using explicit conversions to prevent data loss for Result with value
```cs
Result resultWithoutValue = result.AsResult();
```

## Usage examples for source generated custom ErrorResult<TError> and ErrorResult<TValue, TError> types

### Define a custom ErrorResult
```cs
namespace MyNamespace;

public enum CrudError
{
  InternalError,
  NotFound,
  DuplicateEntry
}

[ErrorResult<CrudError>]
public partial class CrudResult<TValue>;
```

### Example function using a custom CrudResult as return type
```cs
// This is needed to enable the usage of Ok() and Fail methods
// Can be in each file or in a GlobalUsing.cs
using static MyNamespace.CrudResultHelper;

static CrudResult<string> GetCrudResultFunction(bool expectedResult)
{
    // With logging
    if (!expectedResult)
        return Fail(CrudError.InternalError, () => Console.WriteLine("Fail from method"));

    // Without logging
    if (!expectedResult)
        return Fail(CrudError.InternalError);

    return "Success from method";
}
```

### Usage example using the .TryGetValue semantic
```cs
if (GetCrudResultFunction(true).TryGetValue(out string? crudValue, out CrudError? error))
{
    Console.WriteLine($"Success from caller with value <{crudValue}>");
}
else
{
    Console.WriteLine($"Fail from caller with error <{error}>");
}
```

### Usage example using manual check and GetValueUnsafe()
```cs
CrudResult<string> crudResult = GetCrudResultFunction(false);

if (crudResult.IsOk)
{
    Console.WriteLine($"Success from caller with value <{crudResult.GetValueUnsafe()}>");
}
else
{
    Console.WriteLine($"Fail from caller with error <{crudResult.GetErrorUnsafe()}>");
}
```
 
### Usage example using explicit conversions to prevent data loss
```cs
CrudResult crudResultWithoutValue = crudResult.AsCrudResult();
Result<string> simpleResultWithValue = crudResult.AsResultWithValue();
Result simpleResultWithoutValue = crudResult.AsResult();
```
