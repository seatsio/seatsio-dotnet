using System.Linq;
using SeatsioDotNet.Charts;
using Xunit;

namespace SeatsioDotNet.Test.Charts
{
    public class ListChartsTest : SeatsioClientTest
    {
        [Fact]
        public void All()
        {
            var chart1 = Client.Charts().Create();
            var chart2 = Client.Charts().Create();
            var chart3 = Client.Charts().Create();

            var charts = Client.Charts().List().All();

            Assert.Equal(new[] {chart3.Key, chart2.Key, chart1.Key}, charts.Select(c => c.Key));
        }
        
        [Fact]
        public void MultiplePages()
        {
            var charts = Enumerable.Repeat("", 30).Select(x => Client.Charts().Create());

            var retrievedCharts = Client.Charts().List().All();

            Assert.Equal(charts.Reverse().Select(c => c.Key), retrievedCharts.Select(c => c.Key));
        }

        [Fact]
        public void Filter()
        {
            var chart1 = Client.Charts().Create("foo");
            var chart2 = Client.Charts().Create("bar");
            var chart3 = Client.Charts().Create("foofoo");

            var charts = Client.Charts().List().All(new ChartListParams().SetFilter("foo"));

            Assert.Equal(new[] {chart3.Key, chart1.Key}, charts.Select(c => c.Key));
        }

        [Fact]
        public void Tag()
        {
            var chart1 = Client.Charts().Create();
            Client.Charts().AddTag(chart1.Key, "foo");

            var chart2 = Client.Charts().Create();

            var chart3 = Client.Charts().Create();
            Client.Charts().AddTag(chart3.Key, "foo");

            var charts = Client.Charts().List().All(new ChartListParams().SetTag("foo"));

            Assert.Equal(new[] {chart3.Key, chart1.Key}, charts.Select(c => c.Key));
        }

        [Fact]
        public void Exand()
        {
            var chart = Client.Charts().Create();
            var event1 = Client.Events().Create(chart.Key);
            var event2 = Client.Events().Create(chart.Key);

            var charts = Client.Charts().List().All(new ChartListParams().SetExpandEvents());

            Assert.Equal(new[] {event2.Id, event1.Id}, charts.First().Events.Select(c => c.Id));
        }
    }
}