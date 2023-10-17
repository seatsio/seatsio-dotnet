using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class DiscardDraftTest : SeatsioClientTest
{
    [Fact]
    public void Test()
    {
        var chart = Client.Charts.Create("oldname");
        Client.Events.Create(chart.Key);
        Client.Charts.Update(chart.Key, "newname");

        Client.Charts.DiscardDraftVersion(chart.Key);

        var retrievedChart = Client.Charts.Retrieve(chart.Key);
        Assert.Equal("oldname", retrievedChart.Name);
        Assert.Equal("PUBLISHED", retrievedChart.Status);
    }
}