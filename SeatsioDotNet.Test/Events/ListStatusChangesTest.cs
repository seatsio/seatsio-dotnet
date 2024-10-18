using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SeatsioDotNet.Events;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class ListStatusChangesTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.ChangeObjectStatusAsync(new[]
        {
            new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"A-1"}, status: "s1"),
            new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"A-2"}, status: "s2"),
            new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"A-3"}, status: "s3"),
        });
        await WaitForStatusChanges(Client, evnt, 3);

        var statusChanges = Client.Events.StatusChanges(evnt.Key).AllAsync();

        Assert.Equal(new[] {"s3", "s2", "s1"}, statusChanges.Select(s => s.Status));
    }

    [Fact]
    public async Task PropertiesOfStatusChange()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        var extraData = new Dictionary<string, object> {{"foo", "bar"}};
        await Client.Events.ChangeObjectStatusAsync(evnt.Key, new[] {new ObjectProperties("A-1", extraData)}, "s1", null,
            "order1");
        await WaitForStatusChanges(Client, evnt, 1);

        var statusChange = await Client.Events.StatusChanges(evnt.Key).AllAsync().FirstAsync();

        Assert.NotEqual(0, statusChange.Id);
        CustomAssert.CloseTo(DateTimeOffset.Now, statusChange.Date);
        Assert.Equal("order1", statusChange.OrderId);
        Assert.Equal("s1", statusChange.Status);
        Assert.Equal("A-1", statusChange.ObjectLabel);
        Assert.Equal(evnt.Id, statusChange.EventId);
        Assert.Equal(extraData, statusChange.ExtraData);
        Assert.Equal("API_CALL", statusChange.Origin.Type);
        Assert.NotNull(statusChange.Origin.Ip);
        Assert.True(statusChange.IsPresentOnChart);
        Assert.Null(statusChange.NotPresentOnChartReason);
    }

    [Fact]
    public async Task PropertiesOfStatusChange_HoldToken()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        var holdToken = await Client.HoldTokens.CreateAsync();
        await Client.Events.HoldAsync(evnt.Key, new[] {"A-1"}, holdToken.Token);
        await WaitForStatusChanges(Client, evnt, 1);

        var statusChange = await Client.Events.StatusChanges(evnt.Key).AllAsync().FirstAsync();

        Assert.Equal(holdToken.Token, statusChange.HoldToken);
    }

    [Fact]
    public async Task NotPresentOnChartAnymore()
    {
        var chartKey = CreateTestChartWithTables();
        var evnt = await Client.Events.CreateAsync(chartKey, new CreateEventParams().WithTableBookingConfig(TableBookingConfig.AllByTable()));
        await Client.Events.BookAsync(evnt.Key, new[] {"T1"});
        await Client.Events.UpdateAsync(evnt.Key, new UpdateEventParams().WithTableBookingConfig(TableBookingConfig.AllBySeat()));
        await WaitForStatusChanges(Client, evnt, 1);

        var statusChange = await Client.Events.StatusChanges(evnt.Key).AllAsync().FirstAsync();

        Assert.False(statusChange.IsPresentOnChart);
        Assert.Equal("SWITCHED_TO_BOOK_BY_SEAT", statusChange.NotPresentOnChartReason);
    }

    [Fact]
    public async Task Filter()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.ChangeObjectStatusAsync(new[]
        {
            new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"A-1"}, status: "booked"),
            new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"A-2"}, status: "booked"),
            new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"B-1"}, status: "booked"),
            new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"A-3"}, status: "booked")
        });
        await WaitForStatusChanges(Client, evnt, 4);

        var statusChanges = Client.Events.StatusChanges(evnt.Key, filter: "A-").AllAsync();

        Assert.Equal(new[] {"A-3", "A-2", "A-1"}, statusChanges.Select(s => s.ObjectLabel));
    }

    [Fact]
    public async Task SortAsc()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.ChangeObjectStatusAsync(new[]
        {
            new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"A-1"}, status: "booked"),
            new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"A-2"}, status: "booked"),
            new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"B-1"}, status: "booked"),
            new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"A-3"}, status: "booked")
        });
        await WaitForStatusChanges(Client, evnt, 4);

        var statusChanges = Client.Events.StatusChanges(evnt.Key, sortField: "objectLabel").AllAsync();

        Assert.Equal(new[] {"A-1", "A-2", "A-3", "B-1"}, statusChanges.Select(s => s.ObjectLabel));
    }

    [Fact]
    public async Task SortAscPageBefore()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.ChangeObjectStatusAsync(new[]
        {
            new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"A-1"}, status: "booked"),
            new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"A-2"}, status: "booked"),
            new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"B-1"}, status: "booked"),
            new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"A-3"}, status: "booked")
        });
        await WaitForStatusChanges(Client, evnt, 4);

        var statusChangeLister = Client.Events.StatusChanges(evnt.Key, sortField: "objectLabel");
        var statusChangeA3 = (await statusChangeLister.AllAsync().ToListAsync())[2];
        var statusChanges = (await statusChangeLister.PageBeforeAsync(statusChangeA3.Id, 2)).Items;

        Assert.Equal(new[] {"A-1", "A-2"}, statusChanges.Select(s => s.ObjectLabel));
    }

    [Fact]
    public async Task SortAscPageAfter()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.ChangeObjectStatusAsync(new[]
        {
            new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"A-1"}, status: "booked"),
            new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"A-2"}, status: "booked"),
            new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"B-1"}, status: "booked"),
            new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"A-3"}, status: "booked")
        });
        await WaitForStatusChanges(Client, evnt, 4);

        var statusChangeLister = Client.Events.StatusChanges(evnt.Key, sortField: "objectLabel");
        var statusChangeA1 = await statusChangeLister.AllAsync().FirstAsync();
        var statusChanges = (await statusChangeLister.PageAfterAsync(statusChangeA1.Id, 2)).Items;

        Assert.Equal(new[] {"A-2", "A-3"}, statusChanges.Select(s => s.ObjectLabel));
    }

    [Fact]
    public async Task SortDesc()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.ChangeObjectStatusAsync(new[]
        {
            new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"A-1"}, status: "booked"),
            new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"A-2"}, status: "booked"),
            new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"B-1"}, status: "booked"),
            new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"A-3"}, status: "booked")
        });
        await WaitForStatusChanges(Client, evnt, 4);

        var statusChanges = Client.Events.StatusChanges(evnt.Key, sortField: "objectLabel", sortDirection: "DESC").AllAsync();

        Assert.Equal(new[] {"B-1", "A-3", "A-2", "A-1"}, statusChanges.Select(s => s.ObjectLabel));
    }
}