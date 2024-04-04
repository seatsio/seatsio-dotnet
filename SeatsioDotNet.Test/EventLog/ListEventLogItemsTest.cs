using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.EventLog;

public class ListEventLogItemsTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chart = await Client.Charts.CreateAsync();
        await Client.Charts.UpdateAsync(chart.Key, "a chart");

        Thread.Sleep(2000);

        var eventLogItems = Client.EventLog.ListAllAsync();

        Assert.Equal(new[] {"chart.created", "chart.published"}, eventLogItems.Select(e => e.Type));
    }

    [Fact]
    public async Task Properties()
    {
        var chart = await Client.Charts.CreateAsync();

        Thread.Sleep(2000);

        var eventLogItem = (await Client.EventLog.ListAllAsync().ToListAsync()).First();

        Assert.True(eventLogItem.Id > 0);
        Assert.Equal("chart.created", eventLogItem.Type);
        Assert.Equal(new Dictionary<string, object> {{"key", chart.Key}, {"workspaceKey", Workspace.Key}}, eventLogItem.Data);
    }
}