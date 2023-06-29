using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using SeatsioDotNet.Events;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class ListStatusChangesTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.ChangeObjectStatus(new[]
            {
                new StatusChangeRequest(evnt.Key, new[] {"A-1"}, "s1"),
                new StatusChangeRequest(evnt.Key, new[] {"A-2"}, "s2"),
                new StatusChangeRequest(evnt.Key, new[] {"A-3"}, "s3"),
            });
            WaitForStatusChanges(Client, evnt, 3);

            var statusChanges = Client.Events.StatusChanges(evnt.Key).All();

            Assert.Equal(new[] {"s3", "s2", "s1"}, statusChanges.Select(s => s.Status));
        }

        [Fact]
        public void PropertiesOfStatusChange()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            var extraData = new Dictionary<string, object> {{"foo", "bar"}};
            Client.Events.ChangeObjectStatus(evnt.Key, new[] {new ObjectProperties("A-1", extraData)}, "s1", null,
                "order1");
            WaitForStatusChanges(Client, evnt, 1);

            var statusChanges = Client.Events.StatusChanges(evnt.Key).All();
            var statusChange = statusChanges.First();

            Assert.NotEqual(0, statusChange.Id);
            CustomAssert.CloseTo(DateTimeOffset.Now, statusChange.Date);
            Assert.Equal("order1", statusChange.OrderId);
            Assert.Equal("s1", statusChange.Status);
            Assert.Equal("A-1", statusChange.ObjectLabel);
            Assert.Equal(evnt.Id, statusChange.EventId);
            Assert.Equal(extraData, statusChange.ExtraData);
            Assert.Equal("API_CALL", statusChange.Origin.Type);
            Assert.NotNull(statusChange.Origin.Ip);
            Assert.True(statusChange.IsPresentOnChart);
            Assert.Null(statusChange.NotPresentOnChartReason);
        }   
        
        [Fact]
        public void PropertiesOfStatusChange_HoldToken()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            var holdToken = Client.HoldTokens.Create();
            Client.Events.Hold(evnt.Key, new[] {"A-1"}, holdToken.Token);
            WaitForStatusChanges(Client, evnt, 1);

            var statusChanges = Client.Events.StatusChanges(evnt.Key).All();
            var statusChange = statusChanges.First();

            Assert.Equal(holdToken.Token, statusChange.HoldToken);
        }

        [Fact]
        public void NotPresentOnChartAnymore()
        {
            var chartKey = CreateTestChartWithTables();
            var evnt = Client.Events.Create(chartKey, new CreateEventParams().WithTableBookingConfig(TableBookingConfig.AllByTable()));
            Client.Events.Book(evnt.Key, new[] {"T1"});
            Client.Events.Update(evnt.Key, new UpdateEventParams().WithTableBookingConfig(TableBookingConfig.AllBySeat()));
            WaitForStatusChanges(Client, evnt, 1);

            var statusChanges = Client.Events.StatusChanges(evnt.Key).All();
            var statusChange = statusChanges.First();

            Assert.False(statusChange.IsPresentOnChart);
            Assert.Equal("SWITCHED_TO_BOOK_BY_SEAT", statusChange.NotPresentOnChartReason);
        }

        [Fact]
        public void Filter()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.ChangeObjectStatus(new[]
            {
                new StatusChangeRequest(evnt.Key, new[] {"A-1"}, "booked"),
                new StatusChangeRequest(evnt.Key, new[] {"A-2"}, "booked"),
                new StatusChangeRequest(evnt.Key, new[] {"B-1"}, "booked"),
                new StatusChangeRequest(evnt.Key, new[] {"A-3"}, "booked")
            });
            WaitForStatusChanges(Client, evnt, 4);

            var statusChanges = Client.Events.StatusChanges(evnt.Key, filter: "A-").All();

            Assert.Equal(new[] {"A-3", "A-2", "A-1"}, statusChanges.Select(s => s.ObjectLabel));
        }

        [Fact]
        public void SortAsc()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.ChangeObjectStatus(new[]
            {
                new StatusChangeRequest(evnt.Key, new[] {"A-1"}, "booked"),
                new StatusChangeRequest(evnt.Key, new[] {"A-2"}, "booked"),
                new StatusChangeRequest(evnt.Key, new[] {"B-1"}, "booked"),
                new StatusChangeRequest(evnt.Key, new[] {"A-3"}, "booked")
            });
            WaitForStatusChanges(Client, evnt, 4);

            var statusChanges = Client.Events.StatusChanges(evnt.Key, sortField: "objectLabel").All();

            Assert.Equal(new[] {"A-1", "A-2", "A-3", "B-1"}, statusChanges.Select(s => s.ObjectLabel));
        }

        [Fact]
        public void SortAscPageBefore()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.ChangeObjectStatus(new[]
            {
                new StatusChangeRequest(evnt.Key, new[] {"A-1"}, "booked"),
                new StatusChangeRequest(evnt.Key, new[] {"A-2"}, "booked"),
                new StatusChangeRequest(evnt.Key, new[] {"B-1"}, "booked"),
                new StatusChangeRequest(evnt.Key, new[] {"A-3"}, "booked")
            });
            WaitForStatusChanges(Client, evnt, 4);

            var statusChangeLister = Client.Events.StatusChanges(evnt.Key, sortField: "objectLabel");
            var statusChangeA3 = statusChangeLister.All().ToList()[2];
            var statusChanges = statusChangeLister.PageBefore(statusChangeA3.Id, 2).Items;

            Assert.Equal(new[] {"A-1", "A-2"}, statusChanges.Select(s => s.ObjectLabel));
        }

        [Fact]
        public void SortAscPageAfter()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.ChangeObjectStatus(new[]
            {
                new StatusChangeRequest(evnt.Key, new[] {"A-1"}, "booked"),
                new StatusChangeRequest(evnt.Key, new[] {"A-2"}, "booked"),
                new StatusChangeRequest(evnt.Key, new[] {"B-1"}, "booked"),
                new StatusChangeRequest(evnt.Key, new[] {"A-3"}, "booked")
            });
            WaitForStatusChanges(Client, evnt, 4);

            var statusChangeLister = Client.Events.StatusChanges(evnt.Key, sortField: "objectLabel");
            var statusChangeA1 = statusChangeLister.All().ToList()[0];
            var statusChanges = statusChangeLister.PageAfter(statusChangeA1.Id, 2).Items;

            Assert.Equal(new[] {"A-2", "A-3"}, statusChanges.Select(s => s.ObjectLabel));
        }

        [Fact]
        public void SortDesc()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.ChangeObjectStatus(new[]
            {
                new StatusChangeRequest(evnt.Key, new[] {"A-1"}, "booked"),
                new StatusChangeRequest(evnt.Key, new[] {"A-2"}, "booked"),
                new StatusChangeRequest(evnt.Key, new[] {"B-1"}, "booked"),
                new StatusChangeRequest(evnt.Key, new[] {"A-3"}, "booked")
            });
            WaitForStatusChanges(Client, evnt, 4);

            var statusChanges = Client.Events.StatusChanges(evnt.Key, sortField: "objectLabel", sortDirection: "DESC")
                .All();

            Assert.Equal(new[] {"B-1", "A-3", "A-2", "A-1"}, statusChanges.Select(s => s.ObjectLabel));
        }
    }
}