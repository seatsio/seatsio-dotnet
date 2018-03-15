using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class BookObjectsTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events().Create(chartKey);

            Client.Events().Book(evnt.Key, new[] {"A-1", "A-2"});

            Assert.Equal(ObjectStatus.Booked, Client.Events().RetrieveObjectStatus(evnt.Key, "A-1").Status);
            Assert.Equal(ObjectStatus.Booked, Client.Events().RetrieveObjectStatus(evnt.Key, "A-2").Status);
            Assert.Equal(ObjectStatus.Free, Client.Events().RetrieveObjectStatus(evnt.Key, "A-3").Status);
        }
    }
}