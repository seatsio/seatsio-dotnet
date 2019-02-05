using System;
using System.Collections.Generic;
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
            CustomAssert.CloseTo(DateTimeOffset.Now, evnt.CreatedOn.Value);
            Assert.Null(evnt.UpdatedOn);
        }

        [Fact]
        public void EventKeyIsOptional()
        {
            var chartKey = CreateTestChart();

            var evnt = Client.Events.Create(chartKey, "eventje");

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

        [Fact]
        public void TableBookingModesAreOptional()
        {
            var chartKey = CreateTestChartWithTables();
            var tableBookingModes = new Dictionary<string, string> {{"T1", "BY_TABLE"}, {"T2", "BY_SEAT"}};

            var evnt = Client.Events.Create(chartKey, null, tableBookingModes);

            Assert.NotNull(evnt.Key);
            Assert.False(evnt.BookWholeTables);
            Assert.Equal(evnt.TableBookingModes, tableBookingModes);
        }
    }
}