using System.Collections.Generic;
using FluentAssertions;
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
            Assert.Equal(new[] {"A-4", "A-5", "A-6"}, bestAvailableResult.Objects);
        }

        [Fact]
        public void ObjectDetails()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            var bestAvailableResult = Client.Events.ChangeObjectStatus(evnt.Key, new BestAvailable(2), "foo");

            var reportItem = bestAvailableResult.ObjectDetails["A-4"];
            Assert.Equal("A-4", reportItem.Label);
            reportItem.Labels.Should().BeEquivalentTo(new Labels("4", "seat", "A", "row"));
            reportItem.IDs.Should().BeEquivalentTo(new IDs("4", "A", null));
            Assert.Equal("foo", reportItem.Status);
            Assert.Equal("Cat1", reportItem.CategoryLabel);
            Assert.Equal("9", reportItem.CategoryKey);
            Assert.Equal("seat", reportItem.ObjectType);
            Assert.Null(reportItem.TicketType);
            Assert.Null(reportItem.OrderId);
            Assert.True(reportItem.ForSale);
            Assert.Null(reportItem.Section);
            Assert.Null(reportItem.Entrance);
            Assert.Null(reportItem.NumBooked);
            Assert.Null(reportItem.Capacity);
            Assert.Equal("A-3", reportItem.LeftNeighbour);
            Assert.Equal("A-5", reportItem.RightNeighbour);
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
            var extraData = new[]
            {
                new Dictionary<string, object> {{"foo", "bar"}},
                new Dictionary<string, object> {{"foo", "baz"}}
            };

            var bestAvailableResult = Client.Events.ChangeObjectStatus(evnt.Key, new BestAvailable(2, extraData: extraData), "foo");

            Assert.Equal(new[] {"A-4", "A-5"}, bestAvailableResult.Objects);
            Assert.Equal(new Dictionary<string, object> {{"foo", "bar"}}, Client.Events.RetrieveObjectInfo(evnt.Key, "A-4").ExtraData);
            Assert.Equal(new Dictionary<string, object> {{"foo", "baz"}}, Client.Events.RetrieveObjectInfo(evnt.Key, "A-5").ExtraData);
        }   
        
        [Fact]
        public void DoNotTryToPreventOrphanSeats()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.Book(evnt.Key, new[] {"A-4", "A-5"});

            var bestAvailableResult = Client.Events.ChangeObjectStatus(evnt.Key, new BestAvailable(2, tryToPreventOrphanSeats: false), "foo");

            Assert.Equal(new[] {"A-2", "A-3"}, bestAvailableResult.Objects);
        }

        [Fact]
        public void TicketTypes()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            var extraData = new[]
            {
                new Dictionary<string, object> {{"foo", "bar"}},
                new Dictionary<string, object> {{"foo", "baz"}}
            };

            var bestAvailableResult = Client.Events.ChangeObjectStatus(evnt.Key, new BestAvailable(2, ticketTypes: new[]{"adult", "child"}), "foo");

            Assert.Equal(new[] {"A-4", "A-5"}, bestAvailableResult.Objects);
            Assert.Equal("adult", Client.Events.RetrieveObjectInfo(evnt.Key, "A-4").TicketType);
            Assert.Equal("child", Client.Events.RetrieveObjectInfo(evnt.Key, "A-5").TicketType);
        }

        [Fact]
        public void KeepExtraDataTrue()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            var extraData = new Dictionary<string, object> {{"foo1", "bar1"}};
            Client.Events.UpdateExtraData(evnt.Key, "A-5", extraData);

            Client.Events.ChangeObjectStatus(evnt.Key, new BestAvailable(1), "someStatus", null, null, true);

            Assert.Equal(extraData, Client.Events.RetrieveObjectInfo(evnt.Key, "A-5").ExtraData);
        }

        [Fact]
        public void KeepExtraDataFalse()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            var extraData = new Dictionary<string, object> {{"foo1", "bar1"}};
            Client.Events.UpdateExtraData(evnt.Key, "A-5", extraData);

            Client.Events.ChangeObjectStatus(evnt.Key, new BestAvailable(1), "someStatus", null, null, false);

            Assert.Null(Client.Events.RetrieveObjectInfo(evnt.Key, "A-5").ExtraData);
        }

        [Fact]
        public void NoKeepExtraData()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            var extraData = new Dictionary<string, object> {{"foo1", "bar1"}};
            Client.Events.UpdateExtraData(evnt.Key, "A-5", extraData);

            Client.Events.ChangeObjectStatus(evnt.Key, new BestAvailable(1), "someStatus");

            Assert.Null(Client.Events.RetrieveObjectInfo(evnt.Key, "A-5").ExtraData);
        }

        [Fact]
        public void ChannelKeys()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.Channels.Replace(evnt.Key, new List<Channel>
            {
                new("channelKey1", "channel 1", "#FFFF00", 1, new[] {"A-6"})
            });

            var bestAvailableResult = Client.Events.ChangeObjectStatus(evnt.Key, new BestAvailable(1), "someStatus", channelKeys: new[] {"channelKey1"});

            Assert.Equal(new[] {"A-6"}, bestAvailableResult.Objects);
        }   
        
        [Fact]
        public void IgnoreChannels()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.Channels.Replace(evnt.Key, new List<Channel>
            {
                new("channelKey1", "channel 1", "#FFFF00", 1, new[] {"A-5"})
            });

            var bestAvailableResult = Client.Events.ChangeObjectStatus(evnt.Key, new BestAvailable(1), "someStatus", ignoreChannels: true);

            Assert.Equal(new[] {"A-5"}, bestAvailableResult.Objects);
        }
    }
}
