using SeatsioDotNet.Charts;
using Xunit;

namespace SeatsioDotNet.Test.Charts
{
    public class UpdateChartTest : SeatsioClientTest
    {
        [Fact]
        public void Name()
        {
            var chart = Client.Charts.Create(null, "BOOTHS", new[] {new Category(1, "Category 1", "#aaaaaa")});

            Client.Charts.Update(chart.Key, "aChart");

            Chart retrievedChart = Client.Charts.Retrieve(chart.Key);
            Assert.Equal("aChart", retrievedChart.Name);
            var drawing = Client.Charts.RetrievePublishedVersion(chart.Key);
            Assert.Equal("BOOTHS", drawing.VenueType);
            Assert.Equal(1, drawing.Categories.Count);
        }

        [Fact]
        public void Categories()
        {
            var chart = Client.Charts.Create("aChart", "BOOTHS");

            Client.Charts.Update(chart.Key, categories: new[] {new Category(1, "Category 1", "#aaaaaa")});

            Chart retrievedChart = Client.Charts.Retrieve(chart.Key);
            Assert.Equal("aChart", retrievedChart.Name);
            var drawing = Client.Charts.RetrievePublishedVersion(chart.Key);
            Assert.Equal("BOOTHS", drawing.VenueType);
            Assert.Equal(1, drawing.Categories.Count);
        }
    }
}