using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using SeatsioDotNet.EventReports;
using SeatsioDotNet.Events;
using SeatsioDotNet.HoldTokens;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class BookObjectsTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);

        var result = await Client.Events.BookAsync(evnt.Key, new[] {"A-1", "A-2"});

        Assert.Equal(EventObjectInfo.Booked, (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-1")).Status);
        Assert.Equal(EventObjectInfo.Booked, (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-2")).Status);
        Assert.Equal(EventObjectInfo.Free, (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-3")).Status);
        CustomAssert.ContainsOnly(new[] {"A-1", "A-2"}, result.Objects.Keys);
    }

    [Fact]
    public async Task Sections()
    {
        var chartKey = CreateTestChartWithSections();
        var evnt = await Client.Events.CreateAsync(chartKey);

        var result = await Client.Events.BookAsync(evnt.Key, new[] {"Section A-A-1", "Section A-A-2"});

        Assert.Equal(EventObjectInfo.Booked, (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "Section A-A-1")).Status);
        Assert.Equal(EventObjectInfo.Booked, (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "Section A-A-2")).Status);
        Assert.Equal(EventObjectInfo.Free, (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "Section A-A-3")).Status);

        var reportItem = result.Objects["Section A-A-1"];
        Assert.Equal("Section A", reportItem.Section);
        Assert.Equal("Entrance 1", reportItem.Entrance);
        reportItem.Labels.Should().BeEquivalentTo(new Labels("1", "seat", "A", "row", "Section A"));
        reportItem.IDs.Should().BeEquivalentTo(new IDs("1", "A", "Section A"));
    }

    [Fact]
    public async Task HoldToken()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        HoldToken holdToken = await Client.HoldTokens.CreateAsync();
        await Client.Events.HoldAsync(evnt.Key, new[] {"A-1", "A-2"}, holdToken.Token);

        await Client.Events.BookAsync(evnt.Key, new[] {"A-1", "A-2"}, holdToken.Token);

        var objectInfo1 = await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-1");
        Assert.Equal(EventObjectInfo.Booked, objectInfo1.Status);
        Assert.Null(objectInfo1.HoldToken);

        var objectInfo2 = await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-2");
        Assert.Equal(EventObjectInfo.Booked, objectInfo2.Status);
        Assert.Null(objectInfo2.HoldToken);
    }

    [Fact]
    public async Task OrderId()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);

        await Client.Events.BookAsync(evnt.Key, new[] {"A-1", "A-2"}, null, "order1");

        Assert.Equal("order1", (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-1")).OrderId);
        Assert.Equal("order1", (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-2")).OrderId);
    }

    [Fact]
    public async Task KeepExtraData()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        var extraData = new Dictionary<string, object> {{"foo1", "bar1"}};
        await Client.Events.UpdateExtraDataAsync(evnt.Key, "A-1", extraData);

        await Client.Events.BookAsync(evnt.Key, new[] {"A-1"}, null, null, true);

        var loadedExtraData = await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-1");
        Assert.Equal(extraData, loadedExtraData.ExtraData);
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

        await Client.Events.BookAsync(evnt.Key, new[] {"A-1"}, null, null, true, null, new[] {"channelKey1"});

        Assert.Equal(EventObjectInfo.Booked, (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-1")).Status);
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

        await Client.Events.BookAsync(evnt.Key, new[] {"A-1"}, null, null, true, true);

        Assert.Equal(EventObjectInfo.Booked, (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-1")).Status);
    }
}