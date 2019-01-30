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
        
    }    
    
}