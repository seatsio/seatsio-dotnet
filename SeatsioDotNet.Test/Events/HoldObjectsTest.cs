using System.Collections.Generic;
using System.Threading.Tasks;
using SeatsioDotNet.EventReports;
using SeatsioDotNet.Events;
using SeatsioDotNet.HoldTokens;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class HoldObjectsTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        HoldToken holdToken = await Client.HoldTokens.CreateAsync();

        var result = await Client.Events.HoldAsync(evnt.Key, new[] {"A-1", "A-2"}, holdToken.Token);

        var objectInfo1 = await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-1");
        Assert.Equal(EventObjectInfo.Held, objectInfo1.Status);
        Assert.Equal(holdToken.Token, objectInfo1.HoldToken);

        var objectInfo2 = await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-2");
        Assert.Equal(EventObjectInfo.Held, objectInfo2.Status);
        Assert.Equal(holdToken.Token, objectInfo2.HoldToken);
        CustomAssert.ContainsOnly(new[] {"A-1", "A-2"}, result.Objects.Keys);
    }

    [Fact]
    public async Task OrderId()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        HoldToken holdToken = await Client.HoldTokens.CreateAsync();

        await Client.Events.HoldAsync(evnt.Key, new[] {"A-1", "A-2"}, holdToken.Token, "order1");

        Assert.Equal("order1", (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-1")).OrderId);
        Assert.Equal("order1", (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-2")).OrderId);
    }

    [Fact]
    public async Task BestAvailable()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        HoldToken holdToken = await Client.HoldTokens.CreateAsync();

        var bestAvailableResult = await Client.Events.HoldAsync(evnt.Key, new BestAvailable(3), holdToken.Token, "order1");

        Assert.True(bestAvailableResult.NextToEachOther);
        Assert.Equal(new[] {"A-4", "A-5", "A-6"}, bestAvailableResult.Objects);
        Assert.Equal("order1", (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-4")).OrderId);
    }

    [Fact]
    public async Task KeepExtraData()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        var extraData = new Dictionary<string, object> {{"foo1", "bar1"}};
        await Client.Events.UpdateExtraDataAsync(evnt.Key, "A-1", extraData);
        HoldToken holdToken = await Client.HoldTokens.CreateAsync();

        await Client.Events.HoldAsync(evnt.Key, new[] {"A-1"}, holdToken.Token, null, true);

        Assert.Equal(extraData, (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-1")).ExtraData);
    }

    [Fact]
    public async Task ChannelKeys()
    {
        var chartKey = CreateTestChart();
        var channels = new List<Channel>
        {
            new("channelKey1", "channel 1", "#FFFF00", 1, new[] {"A-1", "A-2"})
        };
        var evnt = await Client.Events.CreateAsync(chartKey, new CreateEventParams().WithChannels(channels));
        HoldToken holdToken = await Client.HoldTokens.CreateAsync();

        await Client.Events.HoldAsync(evnt.Key, new[] {"A-1"}, holdToken.Token, null, true, null, new[] {"channelKey1"});

        Assert.Equal(EventObjectInfo.Held, (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-1")).Status);
    }    
        
    [Fact]
    public async Task IgnoreChannels()
    {
        var chartKey = CreateTestChart();
        var channels = new List<Channel>
        {
            new("channelKey1", "channel 1", "#FFFF00", 1, new[] {"A-1", "A-2"})
        };
        var evnt = await Client.Events.CreateAsync(chartKey, new CreateEventParams().WithChannels(channels));
        HoldToken holdToken = await Client.HoldTokens.CreateAsync();

        await Client.Events.HoldAsync(evnt.Key, new[] {"A-1"}, holdToken.Token, null, true, true);

        Assert.Equal(EventObjectInfo.Held, (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-1")).Status);
    }
}