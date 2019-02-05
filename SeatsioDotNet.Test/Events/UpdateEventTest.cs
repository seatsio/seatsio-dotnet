using System;
using System.Collections.Generic;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class UpdateEventTest : SeatsioClientTest
    {
        [Fact]
        public void UpdateChartKey()
        {
            var chartKey1 = CreateTestChart();
            var chartKey2 = CreateTestChart();
            var evnt = Client.Events.Create(chartKey1);

            Client.Events.Update(evnt.Key, chartKey2, null);

            var retrievedEvent = Client.Events.Retrieve(evnt.Key);
            Assert.Equal(evnt.Key, retrievedEvent.Key);
            Assert.Equal(chartKey2, retrievedEvent.ChartKey);
            Assert.False(retrievedEvent.BookWholeTables);
            CustomAssert.CloseTo(DateTimeOffset.Now, retrievedEvent.UpdatedOn.Value);
        }    
        
        [Fact]
        public void UpdateEventKey()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            Client.Events.Update(evnt.Key, null, "newKey");

            var retrievedEvent = Client.Events.Retrieve("newKey");
            Assert.Equal("newKey", retrievedEvent.Key);
            Assert.Equal(chartKey, retrievedEvent.ChartKey);
            Assert.False(retrievedEvent.BookWholeTables);
        }   
        
        [Fact]
        public void UpdateBookWholeTables()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            Client.Events.Update(evnt.Key, null, null, true);

            var retrievedEvent = Client.Events.Retrieve(evnt.Key);
            Assert.Equal(evnt.Key, retrievedEvent.Key);
            Assert.Equal(chartKey, retrievedEvent.ChartKey);
            Assert.True(retrievedEvent.BookWholeTables);
        }    
        
        [Fact]
        public void UpdateTableBookingModes()
        {
            var chartKey = CreateTestChartWithTables();
            var evnt = Client.Events.Create(chartKey, null, new Dictionary<string, string> {{"T1", "BY_TABLE"}});

            Client.Events.Update(evnt.Key, null, null, new Dictionary<string, string> {{"T1", "BY_SEAT"}});

            var retrievedEvent = Client.Events.Retrieve(evnt.Key);
            Assert.Equal(evnt.Key, retrievedEvent.Key);
            Assert.Equal(chartKey, retrievedEvent.ChartKey);
            Assert.False(retrievedEvent.BookWholeTables);
            Assert.Equal(retrievedEvent.TableBookingModes, new Dictionary<string, string> {{"T1", "BY_SEAT"}});
        }
    }
}