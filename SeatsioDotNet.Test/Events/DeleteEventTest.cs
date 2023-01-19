using System;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class DeleteEventTest : SeatsioClientTest
    {
        [Fact]
        public void Delete()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            Client.Events.Delete(evnt.Key);

            Exception ex = Assert.Throws<SeatsioException>(() => Client.Events.Retrieve(evnt.Key));

            Assert.Equal("Event not found: 1d7ea65c-fa60-4b68-a0de-3b71affd2e7e.", ex.Message);
        }
    }
}