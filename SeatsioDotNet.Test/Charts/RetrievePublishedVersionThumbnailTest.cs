using Xunit;
using static System.Text.Encoding;

namespace SeatsioDotNet.Test.Charts
{
    public class RetrievePublishedVersionThumbnailTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chart = Client.Charts.Create();

            byte[] thumbnail = Client.Charts.RetrievePublishedVersionThumbnail(chart.Key);
            Assert.NotEmpty(thumbnail);
        }
    }
}