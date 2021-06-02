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
            Assert.Equal(new[] {"B-4", "B-5", "B-6"}, bestAvailableResult.Objects);
        }

        [Fact]
        public void ObjectDetails()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            var bestAvailableResult = Client.Events.ChangeObjectStatus(evnt.Key, new BestAvailable(2), "foo");

            var reportItem = bestAvailableResult.ObjectDetails["B-4"];
            Assert.Equal("B-4", reportItem.Label);
            reportItem.Labels.Should().BeEquivalentTo(new Labels("4", "seat", "B", "row"));
            reportItem.IDs.Should().BeEquivalentTo(new IDs("4", "B", null));
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
            Assert.Equal("B-3", reportItem.LeftNeighbour);
            Assert.Equal("B-5", reportItem.RightNeighbour);
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

            Assert.Equal(new[] {"B-4", "B-5"}, bestAvailableResult.Objects);
            Assert.Equal(new Dictionary<string, object> {{"foo", "bar"}}, Client.Events.RetrieveObjectStatus(evnt.Key, "B-4").ExtraData);
            Assert.Equal(new Dictionary<string, object> {{"foo", "baz"}}, Client.Events.RetrieveObjectStatus(evnt.Key, "B-5").ExtraData);
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

            Assert.Equal(new[] {"B-4", "B-5"}, bestAvailableResult.Objects);
            Assert.Equal("adult", Client.Events.RetrieveObjectStatus(evnt.Key, "B-4").TicketType);
            Assert.Equal("child", Client.Events.RetrieveObjectStatus(evnt.Key, "B-5").TicketType);
        }

        [Fact]
        public void KeepExtraDataTrue()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            var extraData = new Dictionary<string, object> {{"foo1", "bar1"}};
            Client.Events.UpdateExtraData(evnt.Key, "B-5", extraData);

            Client.Events.ChangeObjectStatus(evnt.Key, new BestAvailable(1), "someStatus", null, null, true);

            Assert.Equal(extraData, Client.Events.RetrieveObjectStatus(evnt.Key, "B-5").ExtraData);
        }

        [Fact]
        public void KeepExtraDataFalse()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            var extraData = new Dictionary<string, object> {{"foo1", "bar1"}};
            Client.Events.UpdateExtraData(evnt.Key, "B-5", extraData);

            Client.Events.ChangeObjectStatus(evnt.Key, new BestAvailable(1), "someStatus", null, null, false);

            Assert.Null(Client.Events.RetrieveObjectStatus(evnt.Key, "B-5").ExtraData);
        }

        [Fact]
        public void NoKeepExtraData()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            var extraData = new Dictionary<string, object> {{"foo1", "bar1"}};
            Client.Events.UpdateExtraData(evnt.Key, "B-5", extraData);

            Client.Events.ChangeObjectStatus(evnt.Key, new BestAvailable(1), "someStatus");

            Assert.Null(Client.Events.RetrieveObjectStatus(evnt.Key, "B-5").ExtraData);
        }

        [Fact]
        public void ChannelKeys()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            var channels = new Dictionary<string, Channel>()
            {
                {"channelKey1", new Channel("channel 1", "#FFFF00", 1)}
            };
            Client.Events.UpdateChannels(evnt.Key, channels);
            Client.Events.AssignObjectsToChannel(evnt.Key, new
            {
                channelKey1 = new[] {"B-6"}
            });

            var bestAvailableResult = Client.Events.ChangeObjectStatus(evnt.Key, new BestAvailable(1), "someStatus", channelKeys: new[] {"channelKey1"});

            Assert.Equal(new[] {"B-6"}, bestAvailableResult.Objects);
        }   
        
        [Fact]
        public void IgnoreChannels()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            var channels = new Dictionary<string, Channel>()
            {
                {"channelKey1", new Channel("channel 1", "#FFFF00", 1)}
            };
            Client.Events.UpdateChannels(evnt.Key, channels);
            Client.Events.AssignObjectsToChannel(evnt.Key, new
            {
                channelKey1 = new[] {"B-5"}
            });

            var bestAvailableResult = Client.Events.ChangeObjectStatus(evnt.Key, new BestAvailable(1), "someStatus", ignoreChannels: true);

            Assert.Equal(new[] {"B-5"}, bestAvailableResult.Objects);
        }
    }
}
