using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class CopyChartToWorkspaceTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chart = await Client.Charts.CreateAsync("my chart", "BOOTHS");
        var workspace = await Client.Workspaces.CreateAsync("my ws");

        var copiedChart = await Client.Charts.CopyToWorkspaceAsync(chart.Key, workspace.Key);

        Assert.Equal("my chart", copiedChart.Name);
        var workspaceClient = CreateSeatsioClient(workspace.SecretKey);
        var retrievedChart = await workspaceClient.Charts.RetrieveAsync(copiedChart.Key);
        Assert.Equal("my chart", retrievedChart.Name);
        var drawing = await workspaceClient.Charts.RetrievePublishedVersionAsync(copiedChart.Key);
        Assert.Equal("BOOTHS", drawing.VenueType);
    }
}