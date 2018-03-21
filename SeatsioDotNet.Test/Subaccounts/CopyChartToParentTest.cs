using Xunit;

namespace SeatsioDotNet.Test.Subaccounts
{
    public class CopyChartToParentTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var subaccount = Client.Subaccounts.Create();
            var chart = CreateSeatsioClient(subaccount.SecretKey).Charts.Create("aChart");

            var copiedChart = Client.Subaccounts.CopyChartToParent(subaccount.Id, chart.Key);

            Assert.Equal("aChart", copiedChart.Name);
            var retrievedChart = Client.Charts.Retrieve(copiedChart.Key);
            Assert.Equal("aChart", retrievedChart.Name);
        }
    }
}