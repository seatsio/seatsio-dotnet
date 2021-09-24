using System.Collections.Generic;
using SeatsioDotNet.Events;
using Xunit;

namespace SeatsioDotNet.Test.Events
{    
    public class UpdateExtraDatasTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            var extraData1 = new Dictionary<string, object> {{"foo1", "bar1"}};
            var extraData2 = new Dictionary<string, object> {{"foo2", "bar2"}};
            var extraDatas = new Dictionary<string, Dictionary<string, object>> {{"A-1", extraData1}, {"A-2", extraData2}};
            
            Client.Events.UpdateExtraDatas(evnt.Key, extraDatas);

            Assert.Equal(extraData1, Client.Events.RetrieveObjectInfo(evnt.Key, "A-1").ExtraData);
            Assert.Equal(extraData2, Client.Events.RetrieveObjectInfo(evnt.Key, "A-2").ExtraData);
        }
    }
}