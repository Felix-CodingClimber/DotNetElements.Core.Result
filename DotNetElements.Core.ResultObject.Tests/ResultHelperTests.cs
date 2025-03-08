using static DotNetElements.Core.ResultObject.ResultHelper;

namespace DotNetElements.Core.ResultObject.Tests;

[TestClass]
public class ResultHelperTests
{
    [TestMethod]
    public void ResultOk_ShouldBeOk()
    {
		Result result = Ok();

        Assert.IsTrue(result.IsOk);
        Assert.IsFalse(result.IsFail);
    }

    [TestMethod]
    public void ResultFail_ShouldBeFail()
    {
		Result result = Fail();

        Assert.IsFalse(result.IsOk);
        Assert.IsTrue(result.IsFail);
    }
}
