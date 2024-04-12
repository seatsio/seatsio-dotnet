using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class ListChartsInArchiveTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chart1 = await Client.Charts.CreateAsync();
        await Client.Charts.MoveToArchiveAsync(chart1.Key);

        var chart2 = await Client.Charts.CreateAsync();

        var chart3 = await Client.Charts.CreateAsync();
        await Client.Charts.MoveToArchiveAsync(chart3.Key);

        var charts = await Client.Charts.Archive.AllAsync().ToListAsync();
        Assert.Equal(new[] {chart3.Key, chart1.Key}, charts.Select(c => c.Key));
    }
}