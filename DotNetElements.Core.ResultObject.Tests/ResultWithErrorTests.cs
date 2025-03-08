namespace DotNetElements.Core.ResultObject.Tests;

[TestClass]
public class ResultWithErrorTests
{
    [TestMethod]
    public void ResultOk_ShouldBeOk()
    {
        ResultWithError result = ResultWithError.Ok();

        Assert.IsTrue(result.IsOk);
        Assert.IsFalse(result.IsFail);
    }

    [TestMethod]
    public void ResultFail_ShouldBeFail()
    {
        ResultWithError result = ResultWithError.Fail("TestErrorMessage");

        Assert.IsFalse(result.IsOk);
        Assert.IsTrue(result.IsFail);
    }

    [TestMethod]
    public void ResultWithError_ShouldContainError()
    {
        string error = "TestErrorMessage";
        ResultWithError result = ResultWithError.Fail(error);

        Assert.IsTrue(result.IsFail);
        Assert.AreEqual(result.GetErrorUnsafe(), error);
    }

    [TestMethod]
    [ExpectedException(typeof(ResultOkException))]
    public void ResultFail_GetErrorUnsafe_ShouldThrowException()
    {
        ResultWithError result = ResultWithError.Ok();

        result.GetErrorUnsafe();
    }

    [TestMethod]
    public void ResultWithError_HasError_ShouldReturnTrueAndError()
    {
        string error = "TestErrorMessage";
        ResultWithError result = ResultWithError.Fail(error);

        bool hasError = result.HasError(out string? resultError);

        Assert.IsTrue(hasError);
        Assert.AreEqual(error, resultError);
    }
}
