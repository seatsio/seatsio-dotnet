using Xunit;

namespace SeatsioDotNet.Test.Charts
{
    public class CopyChartToWorkspaceTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chart = Client.Charts.Create("my chart", "BOOTHS");
            var workspace = Client.Workspaces.Create("my ws");
            
            var copiedChart = Client.Charts.CopyToWorkspace(chart.Key, workspace.Key);
            
            Assert.Equal("my chart", copiedChart.Name);
            var workspaceClient = CreateSeatsioClient(workspace.SecretKey);
            var retrievedChart = workspaceClient.Charts.Retrieve(copiedChart.Key);
            Assert.Equal("my chart", retrievedChart.Name);
            var drawing = workspaceClient.Charts.RetrievePublishedVersion(copiedChart.Key);
            Assert.Equal("BOOTHS", drawing.VenueType);
        }
    }
}