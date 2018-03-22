using Xunit;

namespace SeatsioDotNet.Test.Charts
{
    public class CopyChartToSubaccountTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chart = Client.Charts.Create("my chart", "BOOTHS");
            var subaccount = Client.Subaccounts.Create();
            
            var copiedChart = Client.Charts.CopyToSubaccount(chart.Key, subaccount.Id);
            
            Assert.Equal("my chart", copiedChart.Name);
            var subaccountClient = CreateSeatsioClient(subaccount.SecretKey);
            var retrievedChart = subaccountClient.Charts.Retrieve(copiedChart.Key);
            Assert.Equal("my chart", retrievedChart.Name);
            var drawing = subaccountClient.Charts.RetrievePublishedVersion(copiedChart.Key);
            Assert.Equal("BOOTHS", drawing.VenueType);
        }
    }
}