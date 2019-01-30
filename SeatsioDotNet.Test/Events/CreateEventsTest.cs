using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using SeatsioDotNet.Events;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class CreateEventsTest : SeatsioClientTest
    {
        [Fact]
        public void MultipleEvents()
        {
            var chartKey = CreateTestChart();
            var eventCreationParams = new[]
            {
                new EventCreationParams(),
                new EventCreationParams()
            };

            var events = Client.Events.Create(chartKey, eventCreationParams);
            
            Assert.Equal(2, events.Length);            
        }

        [Fact]
        public void SingleEvent()
        {
            var chartKey = CreateTestChart();
            var eventCreationParams = new[]
            {
                new EventCreationParams()
            };

            var events = Client.Events.Create(chartKey, eventCreationParams);

            Assert.Equal(1, events.Length);
            var e = events[0];
            Assert.NotNull(e.Id);
            Assert.NotNull(e.Key);
            Assert.Equal(chartKey, e.ChartKey);
            Assert.False(e.BookWholeTables);
            Assert.False(e.SupportsBestAvailable);
            Assert.Null(e.ForSaleConfig);
            Assert.NotNull(e.CreatedOn);
            Assert.Null(e.UpdatedOn);
        }

        [Fact]
        public void EventKeyCanBePassedIn()
        {
            var chartKey = CreateTestChart();
            var eventCreationParams = new[]
            {
                new EventCreationParams("event1"),
                new EventCreationParams("event2")
            };

            var events = Client.Events.Create(chartKey, eventCreationParams);
            
            Assert.Equal("event1", events[0].Key);
            Assert.Equal("event2", events[1].Key);
        }

        [Fact]
        public void BookWholeTablesCanBePassedIn()
        {
            var chartKey = CreateTestChart();
            var eventCreationParams = new[]
            {
                new EventCreationParams(null, true),
                new EventCreationParams(null, false)
            };

            var events = Client.Events.Create(chartKey, eventCreationParams);
            
            Assert.True(events[0].BookWholeTables);
            Assert.False(events[1].BookWholeTables);
            Assert.Null(events[0].TableBookingModes);
            Assert.Null(events[1].TableBookingModes);
        }

        [Fact]
        public void TableBookingModesCanBePassedIn()
        {
            var chartKey = CreateTestChartWithTables();
            var eventCreationParams = new[]
            {
                new EventCreationParams(null, new Dictionary<string, string> {{"T1", "BY_TABLE"}, {"T2", "BY_SEAT"}}),
                new EventCreationParams(null, new Dictionary<string, string> {{"T1", "BY_SEAT"}, {"T2", "BY_TABLE"}})
            };

            var events = Client.Events.Create(chartKey, eventCreationParams);
            
            Assert.False(events[0].BookWholeTables);
            Assert.False(events[1].BookWholeTables);
            Assert.Equal(new Dictionary<string, string> {{"T1", "BY_TABLE"}, {"T2", "BY_SEAT"}}, events[0].TableBookingModes);
            Assert.Equal(new Dictionary<string, string> {{"T1", "BY_SEAT"}, {"T2", "BY_TABLE"}}, events[1].TableBookingModes);

        }

        [Fact]               
        public void ErrorOnDuplicateKeys()
        {
            var chartKey = CreateTestChart();
            var eventCreationParams = new[]
            {
                new EventCreationParams("e1"),
                new EventCreationParams("e1")
            };

            Assert.Throws<SeatsioException>(() => Client.Events.Create(chartKey, eventCreationParams));
        }
        
    }    
    
}