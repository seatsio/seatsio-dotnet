using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SeatsioDotNet.Charts;
using SeatsioDotNet.Events;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class CreateEventsTest : SeatsioClientTest
{
    [Fact]
    public async Task MultipleEvents()
    {
        var chartKey = CreateTestChart();
        var eventCreationParams = new[]
        {
            new CreateEventParams(),
            new CreateEventParams()
        };

        var events = await Client.Events.CreateAsync(chartKey, eventCreationParams);

        Assert.Equal(2, events.Length);
    }

    [Fact]
    public async Task SingleEvent()
    {
        var chartKey = CreateTestChart();
        var eventCreationParams = new[]
        {
            new CreateEventParams()
        };

        var events = await Client.Events.CreateAsync(chartKey, eventCreationParams);

        Assert.Single(events);
        var e = events[0];
        Assert.NotNull(e.Id);
        Assert.NotNull(e.Key);
        Assert.Equal(chartKey, e.ChartKey);
        Assert.Equal("INHERIT", e.TableBookingConfig.Mode);
        Assert.True(e.SupportsBestAvailable);
        Assert.Null(e.ForSaleConfig);
        Assert.NotNull(e.CreatedOn);
        Assert.Null(e.UpdatedOn);
    }

    [Fact]
    public async Task EventKeyCanBePassedIn()
    {
        var chartKey = CreateTestChart();
        var eventCreationParams = new[]
        {
            new CreateEventParams().WithKey("event1"),
            new CreateEventParams().WithKey("event2")
        };

        var events = await Client.Events.CreateAsync(chartKey, eventCreationParams);

        Assert.Equal("event1", events[0].Key);
        Assert.Equal("event2", events[1].Key);
    }

    [Fact]
    public async Task TableBookingModesCanBePassedIn()
    {
        var chartKey = CreateTestChartWithTables();
        var eventCreationParams = new[]
        {
            new CreateEventParams().WithTableBookingConfig(TableBookingConfig.Custom(new()
                {{"T1", "BY_TABLE"}, {"T2", "BY_SEAT"}})),
            new CreateEventParams().WithTableBookingConfig(TableBookingConfig.Custom(new()
                {{"T1", "BY_SEAT"}, {"T2", "BY_TABLE"}}))
        };

        var events = await Client.Events.CreateAsync(chartKey, eventCreationParams);

        Assert.Equal("CUSTOM", events[0].TableBookingConfig.Mode);
        Assert.Equal(new() {{"T1", "BY_TABLE"}, {"T2", "BY_SEAT"}}, events[0].TableBookingConfig.Tables);
        Assert.Equal("CUSTOM", events[1].TableBookingConfig.Mode);
        Assert.Equal(new() {{"T1", "BY_SEAT"}, {"T2", "BY_TABLE"}}, events[1].TableBookingConfig.Tables);
    }

    [Fact]
    public async Task ObjectCategoriesCanBePassedIn()
    {
        var chartKey = CreateTestChart();
        var objectCategories = new Dictionary<string, object>()
        {
            {"A-1", 10L}
        };
        var eventCreationParams = new[]
        {
            new CreateEventParams().WithObjectCategories(objectCategories)
        };

        var events = await Client.Events.CreateAsync(chartKey, eventCreationParams);

        Assert.Equal(objectCategories, events[0].ObjectCategories);
    }

    [Fact]
    public async Task CategoriesCanBePassedIn()
    {
        var chartKey = CreateTestChart();
        var eventCategory = new Category("eventCategory", "event-level category", "#AAABBB");
        var categories = new[] {eventCategory};
        var eventCreationParams = new[]
        {
            new CreateEventParams().WithCategories(categories)
        };
        var events = await Client.Events.CreateAsync(chartKey, eventCreationParams);

        Assert.Equal(1, events.Length);
        Assert.Equal(TestChartCategories.Count + categories.Length, events[0].Categories.Count);
        Assert.Contains(eventCategory, events[0].Categories);
    }

    [Fact]
    public async Task ErrorOnDuplicateKeys()
    {
        var chartKey = CreateTestChart();
        var eventCreationParams = new[]
        {
            new CreateEventParams().WithKey("e1"),
            new CreateEventParams().WithKey("e1")
        };

        await Assert.ThrowsAsync<SeatsioException>(async () => await Client.Events.CreateAsync(chartKey, eventCreationParams));
    }

    [Fact]
    public async Task NameCanBePassedIn()
    {
        var chartKey = CreateTestChart();
        var eventCreationParams = new[]
        {
            new CreateEventParams().WithName("My event")
        };

        var events = await Client.Events.CreateAsync(chartKey, eventCreationParams);

        Assert.Equal("My event", events[0].Name);
    }

    [Fact]
    public async Task DateCanBePassedIn()
    {
        var chartKey = CreateTestChart();
        var eventCreationParams = new[]
        {
            new CreateEventParams().WithDate(new DateOnly(2022, 1, 10))
        };

        var events = await Client.Events.CreateAsync(chartKey, eventCreationParams);

        Assert.Equal(new DateOnly(2022, 1, 10), events[0].Date);
    }

    [Fact]
    public async Task ChannelsCanBePassedIn()
    {
        var chartKey = CreateTestChart();
        var channels = new List<Channel>
        {
            new("channelKey1", "channel 1", "#FFFF00", 1, new[] {"A-1", "A-2"}),
            new("channelKey2", "channel 2", "#00FFFF", 2, new String[] { })
        };

        var events = await Client.Events.CreateAsync(chartKey, new[]
        {
            new CreateEventParams().WithChannels(channels)
        });

        Assert.Equivalent(channels, events[0].Channels);
    }

    [Fact]
    public async Task ForSaleConfigCanBePassedIn()
    {
        var chartKey = CreateTestChart();
        var forSaleConfig1 = new ForSaleConfig().WithForSale(false).WithObjects(new[] {"A-1"}).WithAreaPlaces(new() {{"GA1", 3}}).WithCategories(new[] {"Cat1"});
        var forSaleConfig2 = new ForSaleConfig().WithForSale(false).WithObjects(new[] {"A-2"}).WithAreaPlaces(new() {{"GA1", 7}}).WithCategories(new[] {"Cat1"});

        var events = await Client.Events.CreateAsync(chartKey, new[]
        {
            new CreateEventParams().WithForSaleConfig(forSaleConfig1),
            new CreateEventParams().WithForSaleConfig(forSaleConfig2)
        });

        Assert.Equivalent(forSaleConfig1, events[0].ForSaleConfig);
        Assert.Equivalent(forSaleConfig2, events[1].ForSaleConfig);
    }
}
