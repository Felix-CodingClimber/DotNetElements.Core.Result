namespace DotNetElements.Core.Result.Tests;

[TestClass]
public class ResultTests
{
    [TestMethod]
    public void ResultOk_ShouldBeOk()
    {
		Result result = Result.Ok();

        Assert.IsTrue(result.IsOk);
        Assert.IsFalse(result.IsFail);
    }

    [TestMethod]
    public void ResultFail_ShouldBeFail()
    {
		Result result = Result.Fail();

        Assert.IsFalse(result.IsOk);
        Assert.IsTrue(result.IsFail);
    }

    [TestMethod]
    public void ResultWithValue_ShouldContainValue()
    {
        string value = "test";
		Result<string> result = Result<string>.Ok(value);

        Assert.IsTrue(result.IsOk);
        Assert.AreEqual(value, result.GetValueUnsafe());
    }

    [TestMethod]
    [ExpectedException(typeof(ResultFailException))]
    public void ResultFail_GetValueUnsafe_ShouldThrowException()
    {
		Result<string> result = Result<string>.Fail();

        result.GetValueUnsafe();
    }

    [TestMethod]
    public void ResultWithValue_TryGetValue_ShouldReturnTrueAndValue()
    {
        string value = "test";
		Result<string> result = Result<string>.Ok(value);

        bool success = result.TryGetValue(out string? resultValue);

        Assert.IsTrue(success);
        Assert.AreEqual(value, resultValue);
    }

    [TestMethod]
    public void ResultFail_TryGetValue_ShouldReturnFalseAndNull()
    {
		Result<string> result = Result<string>.Fail();

        bool success = result.TryGetValue(out string? resultValue);

        Assert.IsFalse(success);
        Assert.IsNull(resultValue);
    }

    [TestMethod]
    public void Result_ImplicitConversion_ShouldCreateOkResult()
    {
        Result<string> result = "test";

        Assert.IsTrue(result.IsOk);
        Assert.AreEqual("test", result.GetValueUnsafe());
    }

    [TestMethod]
    public void ResultHelper_FailImplicitConversion_ShouldCreateFailResult()
    {
        Result<string> result = ResultHelper.Fail();

        Assert.IsFalse(result.IsOk);
        Assert.IsTrue(result.IsFail);
    }

	[TestMethod]
	public void ResultHelper_OkImplicitConversion_ShouldCreateOkResult()
	{
		Result result = ResultHelper.Ok();

		Assert.IsTrue(result.IsOk);
		Assert.IsFalse(result.IsFail);
	}

	[TestMethod]
	public void Result_ExplicitConversion_ShouldCreateOkResult()
	{
		Result<string> result = "test";

        Result resultBase = result.AsResult();

		Assert.IsTrue(resultBase.IsOk);
		Assert.IsFalse(resultBase.IsFail);
	}

	[TestMethod]
	public void Result_ExplicitConversion_ShouldCreateFailResult()
	{
        Result<string> result = Result<string>.Fail();

		Result resultBase = result.AsResult();

		Assert.IsTrue(resultBase.IsFail);
		Assert.IsFalse(resultBase.IsOk);
	}
}
