using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class RetrieveObjectStatusTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events().Create(chartKey);

            var objectStatus = Client.Events().RetrieveObjectStatus(evnt.Key, "A-1");

            Assert.Equal(ObjectStatus.Free, objectStatus.Status);
        }
    }
}