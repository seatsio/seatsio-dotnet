using Xunit;

namespace SeatsioDotNet.Test.Charts
{
    public class RetrievePublishedVersionTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chart = Client.Charts.Create("aChart");

            var publishedVersion = Client.Charts.RetrievePublishedVersion(chart.Key);
            Assert.Equal("aChart", publishedVersion["name"]);
        }

    }
}