namespace DotNetElements.Core.ResultObject.SourceGenerator.Tests;

[TestClass]
[UsesVerify]
public partial class VerifyTests
{
    [TestMethod]
    public Task Run() => VerifyChecks.Run();
}
