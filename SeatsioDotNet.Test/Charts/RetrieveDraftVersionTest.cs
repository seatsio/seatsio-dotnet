using Xunit;

namespace SeatsioDotNet.Test.Charts
{
    public class RetrieveDraftVersionTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chart = Client.Charts().Create();
            Client.Events().Create(chart.Key);
            Client.Charts().Update(chart.Key, "aChart");

            var draftVersion = Client.Charts().RetrieveDraftVersion(chart.Key);
            Assert.Equal("aChart", draftVersion["name"]);
        }

    }
}