using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SeatsioDotNet.Charts;
using SeatsioDotNet.Events;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class UpdateEventTest : SeatsioClientTest
{
    [Fact]
    public async Task UpdateEventKey()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);

        await Client.Events.UpdateAsync(evnt.Key, new UpdateEventParams().WithKey("newKey"));

        var retrievedEvent = await Client.Events.RetrieveAsync("newKey");
        Assert.Equal("newKey", retrievedEvent.Key);
        Assert.Equal(chartKey, retrievedEvent.ChartKey);
    }

    [Fact]
    public async Task UpdateTableBookingConfig()
    {
        var chartKey = CreateTestChartWithTables();
        var evnt = await Client.Events.CreateAsync(chartKey,
            new CreateEventParams().WithTableBookingConfig(TableBookingConfig.Custom(new() {{"T1", "BY_TABLE"}})));

        await Client.Events.UpdateAsync(evnt.Key, new UpdateEventParams().WithTableBookingConfig(TableBookingConfig.Custom(new() {{"T1", "BY_SEAT"}})));

        var retrievedEvent = await Client.Events.RetrieveAsync(evnt.Key);
        Assert.Equal(evnt.Key, retrievedEvent.Key);
        Assert.Equal(chartKey, retrievedEvent.ChartKey);
        Assert.Equal("CUSTOM", retrievedEvent.TableBookingConfig.Mode);
        Assert.Equal(new() {{"T1", "BY_SEAT"}}, retrievedEvent.TableBookingConfig.Tables);
    }

    [Fact]
    public async Task UpdateObjectCategories()
    {
        var chartKey = CreateTestChart();
        var objectCategories = new Dictionary<string, object>()
        {
            {"A-1", 10L}
        };
        var evnt = await Client.Events.CreateAsync(chartKey, new CreateEventParams().WithObjectCategories(objectCategories));

        var newObjectCategories = new Dictionary<string, object>()
        {
            {"A-2", 9L}
        };
        await Client.Events.UpdateAsync(evnt.Key, new UpdateEventParams().WithObjectCategories(newObjectCategories));

        var retrievedEvent = await Client.Events.RetrieveAsync(evnt.Key);
        Assert.Equal(newObjectCategories, retrievedEvent.ObjectCategories);
    }

    [Fact]
    public async Task RemoveObjectCategories()
    {
        var chartKey = CreateTestChart();
        var objectCategories = new Dictionary<string, object>()
        {
            {"A-1", 10L}
        };
        var evnt = await Client.Events.CreateAsync(chartKey, new CreateEventParams().WithObjectCategories(objectCategories));

        await Client.Events.UpdateAsync(evnt.Key, new UpdateEventParams().WithObjectCategories(new Dictionary<string, object>()));

        var retrievedEvent = await Client.Events.RetrieveAsync(evnt.Key);
        Assert.Null(retrievedEvent.ObjectCategories);
    }

    [Fact]
    public async Task UpdateCategories()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        Category eventCategory = new Category("eventCategory", "event-level category", "#AAABBB");
        Category[] categories = new[] {eventCategory};

        await Client.Events.UpdateAsync(evnt.Key, new UpdateEventParams().WithCategories(categories));

        var retrievedEvent = await Client.Events.RetrieveAsync(evnt.Key);
        Assert.Equal(TestChartCategories.Count + categories.Length, retrievedEvent.Categories.Count);
        Assert.Contains(eventCategory, retrievedEvent.Categories);
    }

    [Fact]
    public async Task RemoveCategories()
    {
        var chartKey = CreateTestChart();
        Category eventCategory = new Category("eventCategory", "event-level category", "#AAABBB");
        Category[] categories = new[] {eventCategory};

        var evnt = await Client.Events.CreateAsync(chartKey, new CreateEventParams().WithCategories(categories));

        await Client.Events.UpdateAsync(evnt.Key, new UpdateEventParams().WithCategories(new Category[] { }));

        var retrievedEvent = await Client.Events.RetrieveAsync(evnt.Key);
        Assert.Equal(TestChartCategories.Count, retrievedEvent.Categories.Count);
        Assert.DoesNotContain(eventCategory, retrievedEvent.Categories);
    }

    [Fact]
    public async Task UpdateName()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey, new CreateEventParams().WithName("An event"));

        await Client.Events.UpdateAsync(evnt.Key, new UpdateEventParams().WithName("Another event"));

        var retrievedEvent = await Client.Events.RetrieveAsync(evnt.Key);
        Assert.Equal("Another event", retrievedEvent.Name);
    }

    [Fact]
    public async Task UpdateDate()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey, new CreateEventParams().WithName("An event"));

        await Client.Events.UpdateAsync(evnt.Key, new UpdateEventParams().WithDate(new DateOnly(2022, 1, 10)));

        var retrievedEvent = await Client.Events.RetrieveAsync(evnt.Key);
        Assert.Equal(new DateOnly(2022, 1, 10), retrievedEvent.Date);
    }

    [Fact]
    public async Task UpdateIsInThePast()
    {
        var chartKey = CreateTestChart();
        await Client.Seasons.CreateAsync(chartKey, eventKeys: new[] {"event1"});

        await Client.Events.UpdateAsync("event1", new UpdateEventParams().WithIsInThePast(true));
        var retrievedEvent1 = await Client.Events.RetrieveAsync("event1");
        Assert.True(retrievedEvent1.IsInThePast);
    }
}