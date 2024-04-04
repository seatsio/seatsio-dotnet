using System.Collections.Generic;
using System.Threading.Tasks;
using SeatsioDotNet.EventReports;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class RetrieveEventObjectInfosTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);

        var objectInfos = await Client.Events.RetrieveObjectInfosAsync(evnt.Key, new string[] {"A-1", "A-2"});

        Assert.Equal(EventObjectInfo.Free, objectInfos["A-1"].Status);
        Assert.Equal(EventObjectInfo.Free, objectInfos["A-2"].Status);
        Assert.Equal(2, objectInfos.Count);
    }

    [Fact]
    public async Task Holds()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        var holdToken = await Client.HoldTokens.CreateAsync();
        await Client.Events.HoldAsync(evnt.Key, new string[] {"GA1"}, holdToken.Token);

        var objectInfos = await Client.Events.RetrieveObjectInfosAsync(evnt.Key, new string[] {"GA1"});

        Dictionary<string, Dictionary<string, int>> expectedHolds = new Dictionary<string, Dictionary<string, int>>();
        expectedHolds.Add(holdToken.Token, new() {{"NO_TICKET_TYPE", 1}});
        Assert.Equivalent(expectedHolds, objectInfos["GA1"].Holds);
    }
}