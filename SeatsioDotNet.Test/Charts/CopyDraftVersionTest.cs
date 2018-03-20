using Xunit;

namespace SeatsioDotNet.Test.Charts
{
    public class CopyDraftVersionTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chart = Client.Charts().Create();
            Client.Events().Create(chart.Key);
            Client.Charts().Update(chart.Key, "newname");
            
            var copiedChart = Client.Charts().CopyDraftVersion(chart.Key);
            
            Assert.Equal("newname (copy)", copiedChart.Name);
        }
    }
}