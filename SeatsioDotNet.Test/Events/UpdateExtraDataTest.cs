using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class UpdateExtraDataTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        var extraData = new Dictionary<string, object> {{"foo", "bar"}};

        await Client.Events.UpdateExtraDataAsync(evnt.Key, "A-1", extraData);

        Assert.Equal(extraData, (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-1")).ExtraData);
    }
}