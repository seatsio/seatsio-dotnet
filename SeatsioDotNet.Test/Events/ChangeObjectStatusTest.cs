using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using SeatsioDotNet.EventReports;
using SeatsioDotNet.Events;
using SeatsioDotNet.HoldTokens;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class ChangeObjectStatusTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);

        var result = await Client.Events.ChangeObjectStatusAsync(evnt.Key, new[] {"A-1", "A-2"}, "foo");

        Assert.Equal("foo", (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-1")).Status);
        Assert.Equal("foo", (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-2")).Status);
        Assert.Equal(EventObjectInfo.Free, (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-3")).Status);
        CustomAssert.ContainsOnly(new[] {"A-1", "A-2"}, result.Objects.Keys);

        var reportItem = result.Objects["A-1"];
        Assert.Equal("A-1", reportItem.Label);
        reportItem.Labels.Should().BeEquivalentTo(new Labels("1", "seat", "A", "row"));
        reportItem.IDs.Should().BeEquivalentTo(new IDs("1", "A", null));
        Assert.Equal("foo", reportItem.Status);
        Assert.Equal("Cat1", reportItem.CategoryLabel);
        Assert.Equal("9", reportItem.CategoryKey);
        Assert.Equal("seat", reportItem.ObjectType);
        Assert.Null(reportItem.TicketType);
        Assert.Null(reportItem.OrderId);
        Assert.True(reportItem.ForSale);
        Assert.Null(reportItem.Section);
        Assert.Null(reportItem.Entrance);
        Assert.Null(reportItem.NumBooked);
        Assert.Null(reportItem.Capacity);
        Assert.Null(reportItem.LeftNeighbour);
        Assert.Equal("A-2", reportItem.RightNeighbour);
    }

    [Fact]
    public async Task HoldToken()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        HoldToken holdToken = await Client.HoldTokens.CreateAsync();
        await Client.Events.HoldAsync(evnt.Key, new[] {"A-1", "A-2"}, holdToken.Token);

        await Client.Events.ChangeObjectStatusAsync(evnt.Key, new[] {"A-1", "A-2"}, "foo", holdToken.Token);

        var objectInfo1 = await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-1");
        Assert.Equal("foo", objectInfo1.Status);
        Assert.Null(objectInfo1.HoldToken);

        var objectInfo2 = await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-2");
        Assert.Equal("foo", objectInfo2.Status);
        Assert.Null(objectInfo2.HoldToken);
    }

    [Fact]
    public async Task OrderId()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);

        await Client.Events.ChangeObjectStatusAsync(evnt.Key, new[] {"A-1", "A-2"}, "foo", null, "order1");

        Assert.Equal("order1", (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-1")).OrderId);
        Assert.Equal("order1", (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-2")).OrderId);
    }

    [Fact]
    public async Task TicketType()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        ObjectProperties object1 = new ObjectProperties("A-1", "T1");
        ObjectProperties object2 = new ObjectProperties("A-2", "T2");

        await Client.Events.ChangeObjectStatusAsync(evnt.Key, new[] {object1, object2}, "foo");

        var objectInfo1 = await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-1");
        Assert.Equal("foo", objectInfo1.Status);
        Assert.Equal("T1", objectInfo1.TicketType);

        var objectInfo2 = await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-2");
        Assert.Equal("foo", objectInfo2.Status);
        Assert.Equal("T2", objectInfo2.TicketType);
    }

    [Fact]
    public async Task Quantity()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        ObjectProperties object1 = new ObjectProperties("GA1", 5);

        await Client.Events.ChangeObjectStatusAsync(evnt.Key, new[] {object1}, "foo");

        var objectInfo1 = await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "GA1");
        Assert.Equal(5, objectInfo1.NumBooked);
    }

    [Fact]
    public async Task ExtraData()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        var extraData1 = new Dictionary<string, object> {{"foo", "bar"}};
        ObjectProperties object1 = new ObjectProperties("A-1", extraData1);
        var extraData2 = new Dictionary<string, object> {{"foo", "baz"}};
        ObjectProperties object2 = new ObjectProperties("A-2", extraData2);

        await Client.Events.ChangeObjectStatusAsync(evnt.Key, new[] {object1, object2}, "foo");

        var objectInfo1 = await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-1");
        Assert.Equal(extraData1, objectInfo1.ExtraData);

        var objectInfo2 = await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-2");
        Assert.Equal(extraData2, objectInfo2.ExtraData);
    }

    [Fact]
    public async Task KeepExtraDataTrue()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        var extraData = new Dictionary<string, object> {{"foo1", "bar1"}};
        await Client.Events.UpdateExtraDataAsync(evnt.Key, "A-1", extraData);

        await Client.Events.ChangeObjectStatusAsync(evnt.Key, new[] {"A-1"}, "someStatus", null, null, true);

        Assert.Equal(extraData, (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-1")).ExtraData);
    }

    [Fact]
    public async Task KeepExtraDataFalse()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        var extraData = new Dictionary<string, object> {{"foo1", "bar1"}};
        await Client.Events.UpdateExtraDataAsync(evnt.Key, "A-1", extraData);

        await Client.Events.ChangeObjectStatusAsync(evnt.Key, new[] {"A-1"}, "someStatus", null, null, false);

        Assert.Null((await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-1")).ExtraData);
    }

    [Fact]
    public async Task NoKeepExtraData()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        var extraData = new Dictionary<string, object> {{"foo1", "bar1"}};
        await Client.Events.UpdateExtraDataAsync(evnt.Key, "A-1", extraData);

        await Client.Events.ChangeObjectStatusAsync(evnt.Key, new[] {"A-1"}, "someStatus");

        Assert.Null((await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-1")).ExtraData);
    }

    [Fact]
    public async Task AllowedPreviousStatuses()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);

        await Assert.ThrowsAsync<SeatsioException>(async () =>
        {
            await Client.Events.ChangeObjectStatusAsync(evnt.Key, new[] {"A-1"}, "someStatus",
                null, null, null, null, null,
                new[] {"somePreviousStatus"}
            );
        });
    }

    [Fact]
    public async Task RejectedPreviousStatuses()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);

        await Assert.ThrowsAsync<SeatsioException>(async () =>
        {
            await Client.Events.ChangeObjectStatusAsync(evnt.Key, new[] {"A-1"}, "someStatus",
                null, null, null, null, null,
                null, new[] {"free"}
            );
        });
    }
}