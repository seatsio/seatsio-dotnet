using System;
using System.Collections.Generic;
using System.Linq;
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
            Client.Events.ChangeObjectStatus(evnt.Key, new[] {"A-1"}, "s1");
            Client.Events.ChangeObjectStatus(evnt.Key, new[] {"A-2"}, "s2");
            Client.Events.ChangeObjectStatus(evnt.Key, new[] {"A-3"}, "s3");

            var statusChanges = Client.Events.StatusChanges(evnt.Key).All();

            Assert.Equal(new[] {"s3", "s2", "s1"}, statusChanges.Select(s => s.Status));
        }

        [Fact]
        public void PropertiesOfStatusChange()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            var extraData = new Dictionary<string, object> {{"foo", "bar"}};
            Client.Events.ChangeObjectStatus(evnt.Key, new[] {new ObjectProperties("A-1", extraData)}, "s1", null, "order1");

            var statusChanges = Client.Events.StatusChanges(evnt.Key).All();
            var statusChange = statusChanges.First();

            Assert.NotEqual(0, statusChange.Id);
            CustomAssert.CloseTo(DateTimeOffset.Now, statusChange.Date);
            Assert.Equal("order1", statusChange.OrderId);
            Assert.Equal("s1", statusChange.Status);
            Assert.Equal("A-1", statusChange.ObjectLabel);
            Assert.Equal(evnt.Id, statusChange.EventId);
            Assert.Equal(extraData, statusChange.ExtraData);
        }
        
        [Fact]
        public void Filter()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.Book(evnt.Key, new[] {"A-1"});
            Client.Events.Book(evnt.Key, new[] {"A-2"});
            Client.Events.Book(evnt.Key, new[] {"B-1"});
            Client.Events.Book(evnt.Key, new[] {"A-3"});

            var statusChanges = Client.Events.StatusChanges(evnt.Key, filter: "A-").All();

            Assert.Equal(new[] {"A-3", "A-2", "A-1"}, statusChanges.Select(s => s.ObjectLabel));
        }    
        
        [Fact]
        public void SortAsc()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.Book(evnt.Key, new[] {"A-1"});
            Client.Events.Book(evnt.Key, new[] {"A-2"});
            Client.Events.Book(evnt.Key, new[] {"B-1"});
            Client.Events.Book(evnt.Key, new[] {"A-3"});

            var statusChanges = Client.Events.StatusChanges(evnt.Key, sortField: "objectLabel").All();

            Assert.Equal(new[] {"A-1", "A-2", "A-3", "B-1"}, statusChanges.Select(s => s.ObjectLabel));
        }       
        
        [Fact]
        public void SortDesc()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.Book(evnt.Key, new[] {"A-1"});
            Client.Events.Book(evnt.Key, new[] {"A-2"});
            Client.Events.Book(evnt.Key, new[] {"B-1"});
            Client.Events.Book(evnt.Key, new[] {"A-3"});

            var statusChanges = Client.Events.StatusChanges(evnt.Key, sortField: "objectLabel", sortDirection: "DESC").All();

            Assert.Equal(new[] {"B-1", "A-3", "A-2", "A-1"}, statusChanges.Select(s => s.ObjectLabel));
        }
    }
}