using Xunit;
using static System.Text.Encoding;

namespace SeatsioDotNet.Test.Charts
{
    public class RetrieveDraftVersionThumbnailTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chart = Client.Charts.Create();
            Client.Events.Create(chart.Key);
            Client.Charts.Update(chart.Key, "aChart");

            byte[] draftVersion = Client.Charts.RetrieveDraftVersionThumbnail(chart.Key);
            Assert.Contains("<!DOCTYPE svg", UTF8.GetString(draftVersion));
        }
    }
}