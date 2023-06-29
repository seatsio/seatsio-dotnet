using System.Collections.Generic;
using FluentAssertions;
using SeatsioDotNet.Charts;
using SeatsioDotNet.EventReports;
using SeatsioDotNet.Events;
using SeatsioDotNet.HoldTokens;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class ChangeObjectStatusTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            var result = Client.Events.ChangeObjectStatus(evnt.Key, new[] {"A-1", "A-2"}, "foo");

            Assert.Equal("foo", Client.Events.RetrieveObjectInfo(evnt.Key, "A-1").Status);
            Assert.Equal("foo", Client.Events.RetrieveObjectInfo(evnt.Key, "A-2").Status);
            Assert.Equal(EventObjectInfo.Free, Client.Events.RetrieveObjectInfo(evnt.Key, "A-3").Status);
            CustomAssert.ContainsOnly(new[] {"A-1", "A-2"}, result.Objects.Keys);

            var reportItem = result.Objects["A-1"];
            Assert.Equal("A-1", reportItem.Label);
            reportItem.Labels.Should().BeEquivalentTo(new Labels("1", "seat", "A", "row"));
            reportItem.IDs.Should().BeEquivalentTo(new IDs("1", "A", null));
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
            Assert.Null(reportItem.LeftNeighbour);
            Assert.Equal("A-2", reportItem.RightNeighbour);
        }

        [Fact]
        public void HoldToken()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            HoldToken holdToken = Client.HoldTokens.Create();
            Client.Events.Hold(evnt.Key, new[] {"A-1", "A-2"}, holdToken.Token);

            Client.Events.ChangeObjectStatus(evnt.Key, new[] {"A-1", "A-2"}, "foo", holdToken.Token);

            var objectInfo1 = Client.Events.RetrieveObjectInfo(evnt.Key, "A-1");
            Assert.Equal("foo", objectInfo1.Status);
            Assert.Null(objectInfo1.HoldToken);

            var objectInfo2 = Client.Events.RetrieveObjectInfo(evnt.Key, "A-2");
            Assert.Equal("foo", objectInfo2.Status);
            Assert.Null(objectInfo2.HoldToken);
        }

        [Fact]
        public void OrderId()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            Client.Events.ChangeObjectStatus(evnt.Key, new[] {"A-1", "A-2"}, "foo", null, "order1");

            Assert.Equal("order1", Client.Events.RetrieveObjectInfo(evnt.Key, "A-1").OrderId);
            Assert.Equal("order1", Client.Events.RetrieveObjectInfo(evnt.Key, "A-2").OrderId);
        }

        [Fact]
        public void TicketType()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            ObjectProperties object1 = new ObjectProperties("A-1", "T1");
            ObjectProperties object2 = new ObjectProperties("A-2", "T2");

            Client.Events.ChangeObjectStatus(evnt.Key, new[] {object1, object2}, "foo");

            var objectInfo1 = Client.Events.RetrieveObjectInfo(evnt.Key, "A-1");
            Assert.Equal("foo", objectInfo1.Status);
            Assert.Equal("T1", objectInfo1.TicketType);

            var objectInfo2 = Client.Events.RetrieveObjectInfo(evnt.Key, "A-2");
            Assert.Equal("foo", objectInfo2.Status);
            Assert.Equal("T2", objectInfo2.TicketType);
        }

        [Fact]
        public void Quantity()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            ObjectProperties object1 = new ObjectProperties("GA1", 5);

            Client.Events.ChangeObjectStatus(evnt.Key, new[] {object1}, "foo");

            var objectInfo1 = Client.Events.RetrieveObjectInfo(evnt.Key, "GA1");
            Assert.Equal(5, objectInfo1.NumBooked);
        }

        [Fact]
        public void ExtraData()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            var extraData1 = new Dictionary<string, object> {{"foo", "bar"}};
            ObjectProperties object1 = new ObjectProperties("A-1", extraData1);
            var extraData2 = new Dictionary<string, object> {{"foo", "baz"}};
            ObjectProperties object2 = new ObjectProperties("A-2", extraData2);

            Client.Events.ChangeObjectStatus(evnt.Key, new[] {object1, object2}, "foo");

            var objectInfo1 = Client.Events.RetrieveObjectInfo(evnt.Key, "A-1");
            Assert.Equal(extraData1, objectInfo1.ExtraData);

            var objectInfo2 = Client.Events.RetrieveObjectInfo(evnt.Key, "A-2");
            Assert.Equal(extraData2, objectInfo2.ExtraData);
        }

        [Fact]
        public void KeepExtraDataTrue()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            var extraData = new Dictionary<string, object> {{"foo1", "bar1"}};
            Client.Events.UpdateExtraData(evnt.Key, "A-1", extraData);

            Client.Events.ChangeObjectStatus(evnt.Key, new[] {"A-1"}, "someStatus", null, null, true);

            Assert.Equal(extraData, Client.Events.RetrieveObjectInfo(evnt.Key, "A-1").ExtraData);
        }

        [Fact]
        public void KeepExtraDataFalse()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            var extraData = new Dictionary<string, object> {{"foo1", "bar1"}};
            Client.Events.UpdateExtraData(evnt.Key, "A-1", extraData);

            Client.Events.ChangeObjectStatus(evnt.Key, new[] {"A-1"}, "someStatus", null, null, false);

            Assert.Null(Client.Events.RetrieveObjectInfo(evnt.Key, "A-1").ExtraData);
        }

        [Fact]
        public void NoKeepExtraData()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            var extraData = new Dictionary<string, object> {{"foo1", "bar1"}};
            Client.Events.UpdateExtraData(evnt.Key, "A-1", extraData);

            Client.Events.ChangeObjectStatus(evnt.Key, new[] {"A-1"}, "someStatus");

            Assert.Null(Client.Events.RetrieveObjectInfo(evnt.Key, "A-1").ExtraData);
        }
        
        [Fact]
        public void IgnoreSocialDistancing()
        {
            var chartKey = CreateTestChart();
            var ruleset = SocialDistancingRuleset.Fixed("ruleset")
                .WithDisabledSeats(new List<string> {"A-1"})
                .Build();
            var rulesets = new Dictionary<string, SocialDistancingRuleset>
            {
                {"ruleset", ruleset},
            };
            Client.Charts.SaveSocialDistancingRulesets(chartKey, rulesets);
            var evnt = Client.Events.Create(chartKey, new CreateEventParams().WithSocialDistancingRulesetKey("ruleset"));

            Client.Events.ChangeObjectStatus(evnt.Key, new[] {"A-1"}, "someStatus", null, null, null, null, null, true);

            Assert.Null(Client.Events.RetrieveObjectInfo(evnt.Key, "A-1").ExtraData);
        }

        [Fact]
        public void AllowedPreviousStatuses()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            Assert.Throws<SeatsioException>(() =>
            {
                Client.Events.ChangeObjectStatus(evnt.Key, new[] {"A-1"}, "someStatus", 
                    null, null, null, null, null, true,
                    new []{"somePreviousStatus"}
                    );
            });
        }

        [Fact]
        public void RejectedPreviousStatuses()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            Assert.Throws<SeatsioException>(() =>
            {
                Client.Events.ChangeObjectStatus(evnt.Key, new[] {"A-1"}, "someStatus", 
                    null, null, null, null, null, true, 
                    null, new []{"free"}
                );
            });
        }
    }
}
