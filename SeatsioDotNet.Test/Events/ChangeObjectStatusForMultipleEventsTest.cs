using System.Threading.Tasks;
using SeatsioDotNet.EventReports;
using SeatsioDotNet.HoldTokens;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class ChangeObjectStatusForMultipleEventsTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chartKey = CreateTestChart();
        var event1 = await Client.Events.CreateAsync(chartKey);
        var event2 = await Client.Events.CreateAsync(chartKey);

        await Client.Events.ChangeObjectStatusAsync(new[] {event1.Key, event2.Key}, new[] {"A-1", "A-2"}, "foo");

        Assert.Equal("foo", (await Client.Events.RetrieveObjectInfoAsync(event1.Key, "A-1")).Status);
        Assert.Equal("foo", (await Client.Events.RetrieveObjectInfoAsync(event1.Key, "A-2")).Status);
        Assert.Equal("foo", (await Client.Events.RetrieveObjectInfoAsync(event2.Key, "A-1")).Status);
        Assert.Equal("foo", (await Client.Events.RetrieveObjectInfoAsync(event2.Key, "A-2")).Status);
    }

    [Fact]
    public async Task Book()
    {
        var chartKey = CreateTestChart();
        var event1 = await Client.Events.CreateAsync(chartKey);
        var event2 = await Client.Events.CreateAsync(chartKey);

        await Client.Events.BookAsync(new[] {event1.Key, event2.Key}, new[] {"A-1", "A-2"});

        Assert.Equal(EventObjectInfo.Booked, (await Client.Events.RetrieveObjectInfoAsync(event1.Key, "A-1")).Status);
        Assert.Equal(EventObjectInfo.Booked, (await Client.Events.RetrieveObjectInfoAsync(event1.Key, "A-2")).Status);
        Assert.Equal(EventObjectInfo.Booked, (await Client.Events.RetrieveObjectInfoAsync(event2.Key, "A-1")).Status);
        Assert.Equal(EventObjectInfo.Booked, (await Client.Events.RetrieveObjectInfoAsync(event2.Key, "A-2")).Status);
    }
    
    [Fact]
    public async Task PutUpForResale()
    {
        var chartKey = CreateTestChart();
        var event1 = await Client.Events.CreateAsync(chartKey);
        var event2 = await Client.Events.CreateAsync(chartKey);

        await Client.Events.PutUpForResaleAsync(new[] {event1.Key, event2.Key}, new[] {"A-1", "A-2"});

        Assert.Equal(EventObjectInfo.Resale, (await Client.Events.RetrieveObjectInfoAsync(event1.Key, "A-1")).Status);
        Assert.Equal(EventObjectInfo.Resale, (await Client.Events.RetrieveObjectInfoAsync(event1.Key, "A-2")).Status);
        Assert.Equal(EventObjectInfo.Resale, (await Client.Events.RetrieveObjectInfoAsync(event2.Key, "A-1")).Status);
        Assert.Equal(EventObjectInfo.Resale, (await Client.Events.RetrieveObjectInfoAsync(event2.Key, "A-2")).Status);
    }

    [Fact]
    public async Task Hold()
    {
        var chartKey = CreateTestChart();
        var event1 = await Client.Events.CreateAsync(chartKey);
        var event2 = await Client.Events.CreateAsync(chartKey);
        HoldToken holdToken = await Client.HoldTokens.CreateAsync();

        await Client.Events.HoldAsync(new[] {event1.Key, event2.Key}, new[] {"A-1", "A-2"}, holdToken.Token);

        Assert.Equal(EventObjectInfo.Held, (await Client.Events.RetrieveObjectInfoAsync(event1.Key, "A-1")).Status);
        Assert.Equal(EventObjectInfo.Held, (await Client.Events.RetrieveObjectInfoAsync(event1.Key, "A-2")).Status);
        Assert.Equal(EventObjectInfo.Held, (await Client.Events.RetrieveObjectInfoAsync(event2.Key, "A-1")).Status);
        Assert.Equal(EventObjectInfo.Held, (await Client.Events.RetrieveObjectInfoAsync(event2.Key, "A-2")).Status);
    }

    [Fact]
    public async Task Release()
    {
        var chartKey = CreateTestChart();
        var event1 = await Client.Events.CreateAsync(chartKey);
        var event2 = await Client.Events.CreateAsync(chartKey);
        await Client.Events.BookAsync(new[] {event1.Key, event2.Key}, new[] {"A-1", "A-2"});

        await Client.Events.ReleaseAsync(new[] {event1.Key, event2.Key}, new[] {"A-1", "A-2"});

        Assert.Equal(EventObjectInfo.Free, (await Client.Events.RetrieveObjectInfoAsync(event1.Key, "A-1")).Status);
        Assert.Equal(EventObjectInfo.Free, (await Client.Events.RetrieveObjectInfoAsync(event1.Key, "A-2")).Status);
        Assert.Equal(EventObjectInfo.Free, (await Client.Events.RetrieveObjectInfoAsync(event2.Key, "A-1")).Status);
        Assert.Equal(EventObjectInfo.Free, (await Client.Events.RetrieveObjectInfoAsync(event2.Key, "A-2")).Status);
    }
    
    [Fact]
    public async Task ResaleListingId()
    {
        var chartKey = CreateTestChart();
        var event1 = await Client.Events.CreateAsync(chartKey);
        var event2 = await Client.Events.CreateAsync(chartKey);

        await Client.Events.ChangeObjectStatusAsync(new[] {event1.Key, event2.Key}, new[] {"A-1"}, EventObjectInfo.Resale, resaleListingId: "listing1");

        Assert.Equal("listing1", (await Client.Events.RetrieveObjectInfoAsync(event1.Key, "A-1")).ResaleListingId);
        Assert.Equal("listing1", (await Client.Events.RetrieveObjectInfoAsync(event2.Key, "A-1")).ResaleListingId);
    }
}