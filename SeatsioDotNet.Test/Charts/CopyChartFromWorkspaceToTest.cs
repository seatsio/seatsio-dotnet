using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class CopyChartFromWorkspaceToTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chart = await Client.Charts.CreateAsync("my chart", "SIMPLE");
        var toWorkspace = await Client.Workspaces.CreateAsync("my ws");

        var copiedChart = await Client.Charts.CopyToWorkspaceAsync(chart.Key, Workspace.Key, toWorkspace.Key);

        Assert.Equal("my chart", copiedChart.Name);
        var workspaceClient = CreateSeatsioClient(toWorkspace.SecretKey);
        var retrievedChart = await workspaceClient.Charts.RetrieveAsync(copiedChart.Key);
        Assert.Equal("my chart", retrievedChart.Name);
        var drawing = await workspaceClient.Charts.RetrievePublishedVersionAsync(copiedChart.Key);
        Assert.Equal("SIMPLE", drawing.VenueType);
    }
}