using System.Collections.Generic;
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

            var drawing = Client.Charts.RetrievePublishedVersion(chart.Key);
            Assert.Equal("MIXED", drawing["venueType"]);
            Assert.Empty(categories(drawing));
        }

        [Fact]
        public void Name()
        {
            var chart = Client.Charts.Create("aChart");

            Chart retrievedChart = Client.Charts.Retrieve(chart.Key);

            Assert.Equal("aChart", retrievedChart.Name);
            var drawing = Client.Charts.RetrievePublishedVersion(chart.Key);
            Assert.Equal("aChart", drawing["name"]);
            Assert.Empty(categories(drawing));
        }

        [Fact]
        public void VenueType()
        {
            var chart = Client.Charts.Create(null, "BOOTHS");

            Chart retrievedChart = Client.Charts.Retrieve(chart.Key);

            Assert.Equal("Untitled chart", retrievedChart.Name);
            var drawing = Client.Charts.RetrievePublishedVersion(chart.Key);
            Assert.Equal("BOOTHS", drawing["venueType"]);
            Assert.Empty(categories(drawing));
        }

        [Fact]
        public void Categories()
        {
            var chart = Client.Charts.Create(null, null, new[]
            {
                new Category(1, "Category 1", "#aaaaaa"),
                new Category(2, "Category 2", "#bbbbbb")
            });

            Chart retrievedChart = Client.Charts.Retrieve(chart.Key);

            Assert.Equal("Untitled chart", retrievedChart.Name);
            var drawing = Client.Charts.RetrievePublishedVersion(chart.Key);
            var actualCategories = categories(drawing);
            Assert.Equal(2, actualCategories.Count);
            Assert.Equal(1, actualCategories[0]["key"]);
            Assert.Equal("Category 1", actualCategories[0]["label"]);
            Assert.Equal("#aaaaaa", actualCategories[0]["color"]);   
            Assert.Equal(2, actualCategories[1]["key"]);
            Assert.Equal("Category 2", actualCategories[1]["label"]);
            Assert.Equal("#bbbbbb", actualCategories[1]["color"]);
        }

        private List<object> categories(dynamic drawing)
        {
            return drawing["categories"]["list"];
        }
    }
}