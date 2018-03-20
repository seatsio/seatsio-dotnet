using System.Linq;
using SeatsioDotNet.Charts;
using Xunit;

namespace SeatsioDotNet.Test.Charts
{
    public class RetrieveChartTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chart = Client.Charts().Create();
            Client.Charts().AddTag(chart.Key, "tag1");
            Client.Charts().AddTag(chart.Key, "tag2");

            Chart retrievedChart = Client.Charts().Retrieve(chart.Key);
            
            Assert.NotEqual(0, retrievedChart.Id);
            Assert.NotNull(retrievedChart.Key);
            Assert.Equal("NOT_USED", retrievedChart.Status);
            Assert.Equal("Untitled chart", retrievedChart.Name);
            Assert.NotNull(retrievedChart.PublishedVersionThumbnailUrl);
            Assert.Null(retrievedChart.DraftVersionThumbnailUrl);
            Assert.Null(retrievedChart.Events);
            CustomAssert.ContainsOnly(new[] {"tag1", "tag2"}, retrievedChart.Tags);
            Assert.False(retrievedChart.Archived);
        }

        [Fact]
        public void WithEvents()
        {
            var chart = Client.Charts().Create();
            var event1 = Client.Events().Create(chart.Key);
            var event2 = Client.Events().Create(chart.Key);

            Chart retrievedChart = Client.Charts().RetrieveWithEvents(chart.Key);

            Assert.Equal(new[] {event2.Id, event1.Id}, retrievedChart.Events.Select(e => e.Id));
        }
    }
}