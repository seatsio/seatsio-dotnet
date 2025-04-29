using System.Collections.Generic;
using System.Threading.Tasks;
using SeatsioDotNet.EventReports;
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
            new StatusChangeRequest(type: StatusChangeRequest.CHANGE_STATUS_TO, eventKey: evnt1.Key, objects: new[] {"A-1"}, status: "lolzor"),
            new StatusChangeRequest(type: StatusChangeRequest.CHANGE_STATUS_TO, eventKey: evnt2.Key, objects: new[] {"A-2"}, status: "lolzor")
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
            new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"A-1"}, status: "lolzor", channelKeys: new[] {"channelKey1"}),
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
            new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"A-1"}, status: "lolzor", ignoreChannels: true),
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
                new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"A-1"}, status: "lolzor", ignoreChannels: true, allowedPreviousStatuses: new[] {"someOtherStatus"}),
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
                new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"A-1"}, status: "lolzor", ignoreChannels: true, rejectedPreviousStatuses: new[] {"free"}),
            });
        });
    }

    [Fact]
    public async Task Release()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.BookAsync(evnt.Key, new[] {"A-1"});

        var result = await Client.Events.ChangeObjectStatusAsync(new[]
        {
            new StatusChangeRequest(type: StatusChangeRequest.RELEASE, eventKey: evnt.Key, objects: new[] {"A-1"}),
        });

        Assert.Equal(EventObjectInfo.Free, result[0].Objects["A-1"].Status);
        Assert.Equal(EventObjectInfo.Free, (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-1")).Status);
    }

    [Fact]
    public async Task OverrideSeasonStatus()
    {
        var chartKey = CreateTestChart();
        var season = await Client.Seasons.CreateAsync(chartKey, null, null, null, new[] {"event1"});
        await Client.Events.BookAsync(season.Key, new[] {"A-1"});

        var result = await Client.Events.ChangeObjectStatusAsync(new[]
        {
            new StatusChangeRequest(type: StatusChangeRequest.OVERRIDE_SEASON_STATUS, eventKey: "event1", objects: new[] {"A-1"}),
        });

        Assert.Equal(EventObjectInfo.Free, result[0].Objects["A-1"].Status);
        Assert.Equal(EventObjectInfo.Free, (await Client.Events.RetrieveObjectInfoAsync("event1", "A-1")).Status);
    }

    [Fact]
    public async Task UseSeasonStatus()
    {
        var chartKey = CreateTestChart();
        var season = await Client.Seasons.CreateAsync(chartKey, null, null, null, new[] {"event1"});
        await Client.Events.BookAsync(season.Key, new[] {"A-1"});
        await Client.Events.OverrideSeasonObjectStatusAsync("event1", new[] {"A-1"});

        var result = await Client.Events.ChangeObjectStatusAsync(new[]
        {
            new StatusChangeRequest(type: StatusChangeRequest.USE_SEASON_STATUS, eventKey: "event1", objects: new[] {"A-1"}),
        });

        Assert.Equal(EventObjectInfo.Booked, result[0].Objects["A-1"].Status);
        Assert.Equal(EventObjectInfo.Booked, (await Client.Events.RetrieveObjectInfoAsync("event1", "A-1")).Status);
    }

    [Fact]
    public async Task ResaleListingId()
    {
        var chartKey1 = CreateTestChart();
        var chartKey2 = CreateTestChart();
        var evnt1 = await Client.Events.CreateAsync(chartKey1);
        var evnt2 = await Client.Events.CreateAsync(chartKey2);

        var result = await Client.Events.ChangeObjectStatusAsync(new[]
        {
            new StatusChangeRequest(type: StatusChangeRequest.CHANGE_STATUS_TO, eventKey: evnt1.Key, objects: new[] {"A-1"}, status: EventObjectInfo.Resale, resaleListingId: "listing1"),
            new StatusChangeRequest(type: StatusChangeRequest.CHANGE_STATUS_TO, eventKey: evnt2.Key, objects: new[] {"A-2"}, status: EventObjectInfo.Resale, resaleListingId: "listing1")
        });

        Assert.Equal("listing1", result[0].Objects["A-1"].ResaleListingId);
        Assert.Equal("listing1", (await Client.Events.RetrieveObjectInfoAsync(evnt1.Key, "A-1")).ResaleListingId);

        Assert.Equal("listing1", result[1].Objects["A-2"].ResaleListingId);
        Assert.Equal("listing1", (await Client.Events.RetrieveObjectInfoAsync(evnt2.Key, "A-2")).ResaleListingId);
    }
}