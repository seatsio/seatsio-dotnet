using System.Collections.Generic;
using SeatsioDotNet.Events;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class ChangeBestAvailableObjectStatusTest : SeatsioClientTest
    {
        [Fact]
        public void Number()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            var bestAvailableResult = Client.Events.ChangeObjectStatus(evnt.Key, new BestAvailable(3), "foo");

            Assert.True(bestAvailableResult.NextToEachOther);
            Assert.Equal(new[] {"B-4", "B-5", "B-6"}, bestAvailableResult.Objects);
        }  
        
        [Fact]
        public void Labels()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            var bestAvailableResult = Client.Events.ChangeObjectStatus(evnt.Key, new BestAvailable(2), "foo");
            
            Assert.Equal(new Dictionary<string, Labels>
            {
                {"B-4", new Labels {Own = "4", Row = "B"}},
                {"B-5", new Labels {Own = "5", Row = "B"}}
            }, bestAvailableResult.Labels);
        }

        [Fact]
        public void Categories()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            var bestAvailableResult = Client.Events.ChangeObjectStatus(evnt.Key, new BestAvailable(3, new[] {"cat2"}), "foo");

            Assert.True(bestAvailableResult.NextToEachOther);
            Assert.Equal(new[] {"C-4", "C-5", "C-6"}, bestAvailableResult.Objects);
        }

        [Fact]
        public void ExtraData()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            var extraData = new []
            {
                new Dictionary<string, object> {{"foo", "bar"}},   
                new Dictionary<string, object> {{"foo", "baz"}}   
            };

            var bestAvailableResult = Client.Events.ChangeObjectStatus(evnt.Key, new BestAvailable(2, null, extraData), "foo");

            Assert.Equal(new[] {"B-4", "B-5"}, bestAvailableResult.Objects);
            Assert.Equal(new Dictionary<string, object> {{"foo", "bar"}}, Client.Events.RetrieveObjectStatus(evnt.Key, "B-4").ExtraData);
            Assert.Equal(new Dictionary<string, object> {{"foo", "baz"}}, Client.Events.RetrieveObjectStatus(evnt.Key, "B-5").ExtraData);
        }
    }
}