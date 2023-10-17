using System.Linq;
using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class ListChartsInArchiveTest : SeatsioClientTest
{
    [Fact]
    public void Test()
    {
        var chart1 = Client.Charts.Create();
        Client.Charts.MoveToArchive(chart1.Key);
            
        var chart2 = Client.Charts.Create();
            
        var chart3 = Client.Charts.Create();
        Client.Charts.MoveToArchive(chart3.Key);

        var charts = Client.Charts.Archive.All();
        Assert.Equal(new [] { chart3.Key, chart1.Key}, charts.Select(c => c.Key));
    }
}