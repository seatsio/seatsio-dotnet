using System;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class CreateEventTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chartKey = CreateTestChart();

            var evnt = Client.Events().Create(chartKey);

            Assert.NotNull(evnt.Key);
            Assert.NotEqual(0, evnt.Id);
            Assert.Equal(chartKey, evnt.ChartKey);
            Assert.False(evnt.BookWholeTables);
            Assert.Null(evnt.ForSaleConfig);
            CustomAssert.CloseTo(evnt.CreatedOn.Value, DateTime.Now);
            Assert.Null(evnt.UpdatedOn);
        }
    }
}