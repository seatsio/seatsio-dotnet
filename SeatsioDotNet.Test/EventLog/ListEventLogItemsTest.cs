using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace SeatsioDotNet.Test.EventLog;

public class ListEventLogItemsTest : SeatsioClientTest
{
    [Fact]
    public void Test()
    {
        var chart = Client.Charts.Create();
        Client.Charts.Update(chart.Key, "a chart");

        Thread.Sleep(2000);

        var eventLogItems = Client.EventLog.ListAll();

        Assert.Equal(new[] {"chart.created", "chart.published"}, eventLogItems.Select(e => e.Type));
    }

    [Fact]
    public void Properties()
    {
        var chart = Client.Charts.Create();

        Thread.Sleep(2000);

        var eventLogItem = Client.EventLog.ListAll().ToList()[0];

        Assert.True(eventLogItem.Id > 0);
        Assert.Equal("chart.created", eventLogItem.Type);
        Assert.Equal(Workspace.Key, eventLogItem.WorkspaceKey);
        Assert.Equal(new Dictionary<string, object> {{"key", chart.Key}}, eventLogItem.Data);
    }
}