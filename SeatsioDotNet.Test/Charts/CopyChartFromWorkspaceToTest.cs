using Xunit;

namespace SeatsioDotNet.Test.Charts
{
    public class CopyChartFromWorkspaceToTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chart = Client.Charts.Create("my chart", "BOOTHS");
            var toWorkspace = Client.Workspaces.Create("my ws");

            var copiedChart = Client.Charts.CopyToWorkspace(chart.Key, Workspace.Key, toWorkspace.Key);

            Assert.Equal("my chart", copiedChart.Name);
            var workspaceClient = CreateSeatsioClient(toWorkspace.SecretKey);
            var retrievedChart = workspaceClient.Charts.Retrieve(copiedChart.Key);
            Assert.Equal("my chart", retrievedChart.Name);
            var drawing = workspaceClient.Charts.RetrievePublishedVersion(copiedChart.Key);
            Assert.Equal("BOOTHS", drawing.VenueType);
        }
    }
}