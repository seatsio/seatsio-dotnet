using System;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class CreateEventTest : SeatsioClientTest
    {
        [Fact]
        public void ChartKeyIsMandatory()
        {
            var chartKey = CreateTestChart();

            var evnt = Client.Events.Create(chartKey);

            Assert.NotNull(evnt.Key);
            Assert.NotEqual(0, evnt.Id);
            Assert.Equal(chartKey, evnt.ChartKey);
            Assert.False(evnt.BookWholeTables);
            Assert.True(evnt.SupportsBestAvailable);
            Assert.Null(evnt.ForSaleConfig);
            CustomAssert.CloseTo(DateTime.Now, evnt.CreatedOn.Value);
            Assert.Null(evnt.UpdatedOn);
        }   
        
        [Fact]
        public void EventKeyIsOptional()
        {
            var chartKey = CreateTestChart();

            var evnt = Client.Events.Create(chartKey, "eventje", null);

            Assert.Equal("eventje", evnt.Key);
            Assert.False(evnt.BookWholeTables);
        }    
        
        [Fact]
        public void BookWholeTablesIsOptional()
        {
            var chartKey = CreateTestChart();

            var evnt = Client.Events.Create(chartKey, null, true);

            Assert.NotNull(evnt.Key);
            Assert.True(evnt.BookWholeTables);
        }
    }
}