using System.Collections.Generic;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class UpdateExtraDataTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            var extraData = new Dictionary<string, object> {{"foo", "bar"}};

            Client.Events.UpdateExtraData(evnt.Key, "A-1", extraData);

            Assert.Equal(extraData, Client.Events.RetrieveObjectStatus(evnt.Key, "A-1").ExtraData);
        }
    }
}