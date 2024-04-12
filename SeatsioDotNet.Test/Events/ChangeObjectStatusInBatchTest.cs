using System.Collections.Generic;
using System.Threading.Tasks;
using SeatsioDotNet.Events;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class ChangeObjectStatusInBatchTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chartKey1 = CreateTestChart();
        var chartKey2 = CreateTestChart();
        var evnt1 = await Client.Events.CreateAsync(chartKey1);
        var evnt2 = await Client.Events.CreateAsync(chartKey2);

        var result = await Client.Events.ChangeObjectStatusAsync(new[]
        {
            new StatusChangeRequest(evnt1.Key, new[] {"A-1"}, "lolzor"),
            new StatusChangeRequest(evnt2.Key, new[] {"A-2"}, "lolzor")
        });

        Assert.Equal("lolzor", result[0].Objects["A-1"].Status);
        Assert.Equal("lolzor", (await Client.Events.RetrieveObjectInfoAsync(evnt1.Key, "A-1")).Status);

        Assert.Equal("lolzor", result[1].Objects["A-2"].Status);
        Assert.Equal("lolzor", (await Client.Events.RetrieveObjectInfoAsync(evnt2.Key, "A-2")).Status);
    }

    [Fact]
    public async Task ChannelKeys()
    {
        var chartKey = CreateTestChart();
        var channels = new List<Channel>
        {
            new("channelKey1", "channel 1", "#FFFF00", 1, new[] {"A-1"})
        };
        var evnt = await Client.Events.CreateAsync(chartKey, new CreateEventParams().WithChannels(channels));

        var result = await Client.Events.ChangeObjectStatusAsync(new[]
        {
            new StatusChangeRequest(evnt.Key, new[] {"A-1"}, "lolzor", channelKeys: new[] {"channelKey1"}),
        });

        Assert.Equal("lolzor", result[0].Objects["A-1"].Status);
    }

    [Fact]
    public async Task IgnoreChannels()
    {
        var chartKey = CreateTestChart();
        var channels = new List<Channel>
        {
            new("channelKey1", "channel 1", "#FFFF00", 1, new[] {"A-1"})
        };
        var evnt = await Client.Events.CreateAsync(chartKey, new CreateEventParams().WithChannels(channels));

        var result = await Client.Events.ChangeObjectStatusAsync(new[]
        {
            new StatusChangeRequest(evnt.Key, new[] {"A-1"}, "lolzor", ignoreChannels: true),
        });

        Assert.Equal("lolzor", result[0].Objects["A-1"].Status);
    }

    [Fact]
    public async Task AllowedPreviousStatuses()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);

        await Assert.ThrowsAsync<SeatsioException>(async () =>
        {
            await Client.Events.ChangeObjectStatusAsync(new[]
            {
                new StatusChangeRequest(evnt.Key, new[] {"A-1"}, "lolzor", ignoreChannels: true, allowedPreviousStatuses: new[] {"someOtherStatus"}),
            });
        });
    }

    [Fact]
    public async Task RejectedPreviousStatuses()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);

        await Assert.ThrowsAsync<SeatsioException>(async () =>
        {
            await Client.Events.ChangeObjectStatusAsync(new[]
            {
                new StatusChangeRequest(evnt.Key, new[] {"A-1"}, "lolzor", ignoreChannels: true, rejectedPreviousStatuses: new[] {"free"}),
            });
        });
    }
}