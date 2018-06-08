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

            Assert.Contains("404", ex.Message);
        }
    }
}