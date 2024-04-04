using System.Threading.Tasks;
using SeatsioDotNet.EventReports;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class RetrieveEventObjectInfoTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);

        var objectInfo = await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-1");

        Assert.Equal(EventObjectInfo.Free, objectInfo.Status);
        Assert.True(objectInfo.ForSale);
    }
}