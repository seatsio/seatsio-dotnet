using System.Linq;
using SeatsioDotNet.Events;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class ListStatusChangesForObjectTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.ChangeObjectStatus(new[]
            {
                new StatusChangeRequest(evnt.Key, new[] {"A-1"}, "s1"),
                new StatusChangeRequest(evnt.Key, new[] {"A-1"}, "s2"),
                new StatusChangeRequest(evnt.Key, new[] {"A-1"}, "s3"),
                new StatusChangeRequest(evnt.Key, new[] {"A-1"}, "s4"),
                new StatusChangeRequest(evnt.Key, new[] {"A-2"}, "s5")
            });
            WaitForStatusChanges(Client, evnt, 5);

            var statusChanges = Client.Events.StatusChangesForObject(evnt.Key, "A-1").All();

            Assert.Equal(new[] {"s4", "s3", "s2", "s1"}, statusChanges.Select(s => s.Status));
        }
    }
}