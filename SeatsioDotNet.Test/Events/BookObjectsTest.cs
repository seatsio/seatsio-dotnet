using System.Collections.Generic;
using SeatsioDotNet.Events;
using SeatsioDotNet.HoldTokens;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class BookObjectsTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            var result = Client.Events.Book(evnt.Key, new[] {"A-1", "A-2"});

            Assert.Equal(ObjectStatus.Booked, Client.Events.RetrieveObjectStatus(evnt.Key, "A-1").Status);
            Assert.Equal(ObjectStatus.Booked, Client.Events.RetrieveObjectStatus(evnt.Key, "A-2").Status);
            Assert.Equal(ObjectStatus.Free, Client.Events.RetrieveObjectStatus(evnt.Key, "A-3").Status);
            
            Assert.Equal(new Dictionary<string, Labels>
            {
                {"A-1", new Labels {Own = "1", Row = "A"}},
                {"A-2", new Labels {Own = "2", Row = "A"}}
            }, result.Labels);
        }

        [Fact]
        public void HoldToken()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            HoldToken holdToken = Client.HoldTokens.Create();
            Client.Events.Hold(evnt.Key, new[] {"A-1", "A-2"}, holdToken.Token);

            Client.Events.Book(evnt.Key, new[] {"A-1", "A-2"}, holdToken.Token);

            var status1 = Client.Events.RetrieveObjectStatus(evnt.Key, "A-1");
            Assert.Equal(ObjectStatus.Booked, status1.Status);
            Assert.Null(status1.HoldToken);

            var status2 = Client.Events.RetrieveObjectStatus(evnt.Key, "A-2");
            Assert.Equal(ObjectStatus.Booked, status2.Status);
            Assert.Null(status2.HoldToken);
        }

        [Fact]
        public void OrderId()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            Client.Events.Book(evnt.Key, new[] {"A-1", "A-2"}, null, "order1");

            Assert.Equal("order1", Client.Events.RetrieveObjectStatus(evnt.Key, "A-1").OrderId);
            Assert.Equal("order1", Client.Events.RetrieveObjectStatus(evnt.Key, "A-2").OrderId);
        }
    }
}