using Tests.Lionbridge.Base;
using Apps.Lionbridge.DataSourceHandlers;

namespace Tests.Lionbridge;

[TestClass]
public class DataHandlerTests : TestBase
{
    [TestMethod]
    public async Task JobDataSourceHandler_ReturnsJobs()
    {
        // Arrange
        var handler = new JobDataSourceHandler(InvocationContext);

        // Act
        var result = await handler.GetDataAsync(new(), default);

        // Assert
        foreach (var job in result)
            Console.WriteLine($"{job.Value} - {job.DisplayName}");
        Assert.IsNotNull(result);
    }
}
