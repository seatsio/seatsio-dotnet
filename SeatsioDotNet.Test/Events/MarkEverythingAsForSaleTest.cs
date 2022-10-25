using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class MarkEverythingAsForSaleTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.MarkAsNotForSale(evnt.Key, new[] {"o1", "o2"}, null, new[] {"cat1", "cat2"});

            Client.Events.MarkEverythingAsForSale(evnt.Key);
            
            Assert.Null(Client.Events.Retrieve(evnt.Key).ForSaleConfig);
        }
    }
}