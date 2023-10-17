using System.Collections.Generic;
using SeatsioDotNet.EventReports;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class RetrieveEventObjectInfosTest : SeatsioClientTest
{
    [Fact]
    public void Test()
    {
        var chartKey = CreateTestChart();
        var evnt = Client.Events.Create(chartKey);

        var objectInfos = Client.Events.RetrieveObjectInfos(evnt.Key, new string[] {"A-1", "A-2"});

        Assert.Equal(EventObjectInfo.Free, objectInfos["A-1"].Status);
        Assert.Equal(EventObjectInfo.Free, objectInfos["A-2"].Status);
        Assert.Equal(2, objectInfos.Count);
    }

    [Fact]
    public void Holds()
    {
        var chartKey = CreateTestChart();
        var evnt = Client.Events.Create(chartKey);
        var holdToken = Client.HoldTokens.Create();
        Client.Events.Hold(evnt.Key, new string[] {"GA1"}, holdToken.Token);

        var objectInfos = Client.Events.RetrieveObjectInfos(evnt.Key, new string[] {"GA1"});

        Dictionary<string, Dictionary<string, int>> expectedHolds = new Dictionary<string, Dictionary<string, int>>();
        expectedHolds.Add(holdToken.Token, new() {{"NO_TICKET_TYPE", 1}});
        Assert.Equivalent(expectedHolds, objectInfos["GA1"].Holds);
    }
}