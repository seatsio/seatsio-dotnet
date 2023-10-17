using SeatsioDotNet.EventReports;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class RetrieveEventObjectInfoTest : SeatsioClientTest
{
    [Fact]
    public void Test()
    {
        var chartKey = CreateTestChart();
        var evnt = Client.Events.Create(chartKey);

        var objectInfo = Client.Events.RetrieveObjectInfo(evnt.Key, "A-1");

        Assert.Equal(EventObjectInfo.Free, objectInfo.Status);
        Assert.True(objectInfo.ForSale);
    }
}