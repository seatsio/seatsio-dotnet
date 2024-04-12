using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class UpdateExtraDatasTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        var extraData1 = new Dictionary<string, object> {{"foo1", "bar1"}};
        var extraData2 = new Dictionary<string, object> {{"foo2", "bar2"}};
        var extraDatas = new Dictionary<string, Dictionary<string, object>> {{"A-1", extraData1}, {"A-2", extraData2}};

        await Client.Events.UpdateExtraDatasAsync(evnt.Key, extraDatas);

        Assert.Equal(extraData1, (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-1")).ExtraData);
        Assert.Equal(extraData2, (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-2")).ExtraData);
    }
}