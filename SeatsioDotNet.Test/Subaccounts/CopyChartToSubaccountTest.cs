using Xunit;

namespace SeatsioDotNet.Test.Subaccounts
{
    public class CopyChartToSubaccountTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var fromSubaccount = Client.Subaccounts.Create();
            var chart = CreateSeatsioClient(fromSubaccount.SecretKey).Charts.Create("aChart");
            var toSubaccount = Client.Subaccounts.Create();

            var copiedChart = Client.Subaccounts.CopyChartToSubaccount(fromSubaccount.Id, toSubaccount.Id, chart.Key);

            Assert.Equal("aChart", copiedChart.Name);
            var retrievedChart = CreateSeatsioClient(toSubaccount.SecretKey).Charts.Retrieve(copiedChart.Key);
            Assert.Equal("aChart", retrievedChart.Name);
        }
    }
}