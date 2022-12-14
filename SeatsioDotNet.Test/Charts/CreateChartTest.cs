using SeatsioDotNet.Charts;
using Xunit;

namespace SeatsioDotNet.Test.Charts
{
    public class CreateChartTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chart = Client.Charts.Create();

            Chart retrievedChart = Client.Charts.Retrieve(chart.Key);

            Assert.NotEqual(0, retrievedChart.Id);
            Assert.NotNull(retrievedChart.Key);
            Assert.Equal("NOT_USED", retrievedChart.Status);
            Assert.Equal("Untitled chart", retrievedChart.Name);
            Assert.NotNull(retrievedChart.PublishedVersionThumbnailUrl);
            Assert.Null(retrievedChart.DraftVersionThumbnailUrl);
            Assert.Null(retrievedChart.Events);
            Assert.Empty(retrievedChart.Tags);
            Assert.False(retrievedChart.Archived);
        }

        [Fact]
        public void Name()
        {
            var chart = Client.Charts.Create("aChart");

            Chart retrievedChart = Client.Charts.Retrieve(chart.Key);

            Assert.Equal("aChart", retrievedChart.Name);
        }

        [Fact]
        public void VenueType()
        {
            var chart = Client.Charts.Create(null, "BOOTHS");

            Chart retrievedChart = Client.Charts.Retrieve(chart.Key);

            Assert.Equal("Untitled chart", retrievedChart.Name);
        }

        [Fact]
        public void Categories()
        {
            var chart = Client.Charts.Create(null, null, new[]
            {
                new Category(1, "Category 1", "#aaaaaa"),
                new Category(2, "Category 2", "#bbbbbb", true),
                new Category("cat-3", "Category 3", "#cccccc")
            });

            Chart retrievedChart = Client.Charts.Retrieve(chart.Key);

            Assert.Equal("Untitled chart", retrievedChart.Name);
        }
    }
}