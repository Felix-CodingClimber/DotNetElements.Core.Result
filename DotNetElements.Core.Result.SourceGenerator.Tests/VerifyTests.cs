namespace DotNetElements.Core.Result.SourceGenerator.Tests;

[TestClass]
[UsesVerify]
public partial class VerifyTests
{
    [TestMethod]
    public Task Run() => VerifyChecks.Run();
}
