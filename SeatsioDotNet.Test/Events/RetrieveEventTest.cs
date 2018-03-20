using System;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class RetrieveEventTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events().Create(chartKey);

            var retrievedEvent = Client.Events().Retrieve(evnt.Key);
            
            Assert.NotNull(retrievedEvent.Key);
            Assert.NotEqual(0, retrievedEvent.Id);
            Assert.Equal(chartKey, retrievedEvent.ChartKey);
            Assert.False(retrievedEvent.BookWholeTables);
            Assert.Null(retrievedEvent.ForSaleConfig);
            CustomAssert.CloseTo(DateTime.Now, retrievedEvent.CreatedOn.Value);
            Assert.Null(retrievedEvent.UpdatedOn);
        }
    }
}