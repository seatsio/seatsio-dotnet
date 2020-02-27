using SeatsioDotNet.Events;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class ChangeObjectStatusInBatchTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chartKey1 = CreateTestChart();
            var chartKey2 = CreateTestChart();
            var evnt1 = Client.Events.Create(chartKey1);
            var evnt2 = Client.Events.Create(chartKey2);

            var result = Client.Events.ChangeObjectStatus(new[]
            {
                new StatusChangeRequest(evnt1.Key, new[] {"A-1"}, "lolzor"),
                new StatusChangeRequest(evnt2.Key, new[] {"A-2"}, "lolzor")
            });

            Assert.Equal("lolzor", result[0].Objects["A-1"].Status);
            Assert.Equal("lolzor", Client.Events.RetrieveObjectStatus(evnt1.Key, "A-1").Status);

            Assert.Equal("lolzor", result[1].Objects["A-2"].Status);
            Assert.Equal("lolzor", Client.Events.RetrieveObjectStatus(evnt2.Key, "A-2").Status);
        }
    }
}