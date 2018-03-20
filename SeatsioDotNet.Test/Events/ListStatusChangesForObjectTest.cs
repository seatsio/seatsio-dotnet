using System.Linq;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class ListStatusChangesForObjectTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events().Create(chartKey);
            Client.Events().ChangeObjectStatus(evnt.Key, new[] {"A-1"}, "s1");
            Client.Events().ChangeObjectStatus(evnt.Key, new[] {"A-1"}, "s2");
            Client.Events().ChangeObjectStatus(evnt.Key, new[] {"A-1"}, "s3");
            Client.Events().ChangeObjectStatus(evnt.Key, new[] {"A-1"}, "s4");

            var statusChanges = Client.Events().StatusChanges(evnt.Key, "A-1").All();

            Assert.Equal(new[] {"s4", "s3", "s2", "s1"}, statusChanges.Select(s => s.Status));
        }
    }
}