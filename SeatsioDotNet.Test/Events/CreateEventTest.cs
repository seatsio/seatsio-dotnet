using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SeatsioDotNet.Charts;
using SeatsioDotNet.Events;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class CreateEventTest : SeatsioClientTest
{
    [Fact]
    public async Task ChartKeyIsMandatory()
    {
        var chartKey = CreateTestChart();

        var evnt = await Client.Events.CreateAsync(chartKey);

        Assert.NotNull(evnt.Key);
        Assert.NotEqual(0, evnt.Id);
        Assert.Equal(chartKey, evnt.ChartKey);
        Assert.Equal("INHERIT", evnt.TableBookingConfig.Mode);
        Assert.True(evnt.SupportsBestAvailable);
        Assert.Null(evnt.ForSaleConfig);
        CustomAssert.CloseTo(DateTimeOffset.Now, evnt.CreatedOn.Value);
        Assert.Null(evnt.UpdatedOn);
        Assert.Equal(TestChartCategories, evnt.Categories);
    }

    [Fact]
    public async Task EventKeyIsOptional()
    {
        var chartKey = CreateTestChart();

        var evnt = await Client.Events.CreateAsync(chartKey, new CreateEventParams().WithKey("eventje"));

        Assert.Equal("eventje", evnt.Key);
    }

    [Fact]
    public async Task TableBookingModeCustomCanBeUsed()
    {
        var chartKey = CreateTestChartWithTables();
        var tableBookingConfig = TableBookingConfig.Custom(new() {{"T1", "BY_TABLE"}, {"T2", "BY_SEAT"}});

        var evnt = await Client.Events.CreateAsync(chartKey,
            new CreateEventParams().WithTableBookingConfig(tableBookingConfig));

        Assert.NotNull(evnt.Key);
        Assert.Equal("CUSTOM", evnt.TableBookingConfig.Mode);
        Assert.Equal(new Dictionary<string, string> {{"T1", "BY_TABLE"}, {"T2", "BY_SEAT"}},
            evnt.TableBookingConfig.Tables);
    }

    [Fact]
    public async Task TableBookingModeInheritCanBeUsed()
    {
        var chartKey = CreateTestChartWithTables();

        var evnt = await Client.Events.CreateAsync(chartKey,
            new CreateEventParams().WithTableBookingConfig(TableBookingConfig.Inherit()));

        Assert.NotNull(evnt.Key);
        Assert.Equal("INHERIT", evnt.TableBookingConfig.Mode);
    }

    [Fact]
    public async Task ObjectCategoriesCanBePassedIn()
    {
        var chartKey = CreateTestChart();

        var objectCategories = new Dictionary<string, object>()
        {
            {"A-1", 10L}
        };
        var evnt = await Client.Events.CreateAsync(chartKey, new CreateEventParams().WithObjectCategories(objectCategories));
        Assert.Equal(objectCategories, evnt.ObjectCategories);
    }

    [Fact]
    public async Task CategoriesCanBePassedIn()
    {
        var chartKey = CreateTestChart();
        var eventCategory = new Category("eventCategory", "event-level category", "#AAABBB");
        var categories = new[] {eventCategory};

        var evnt = await Client.Events.CreateAsync(chartKey, new CreateEventParams().WithCategories(categories));

        Assert.Equal(TestChartCategories.Count + categories.Length, evnt.Categories.Count);
        Assert.Contains(eventCategory, evnt.Categories);
    }

    [Fact]
    public async Task NameCanBePassedIn()
    {
        var chartKey = CreateTestChart();

        var evnt = await Client.Events.CreateAsync(chartKey, new CreateEventParams().WithName("My event"));

        Assert.Equal("My event", evnt.Name);
    }

    [Fact]
    public async Task DateCanBePassedIn()
    {
        var chartKey = CreateTestChart();

        var evnt = await Client.Events.CreateAsync(chartKey, new CreateEventParams().WithDate(new DateOnly(2022, 1, 10)));

        Assert.Equal(new DateOnly(2022, 1, 10), evnt.Date);
    }

    [Fact]
    public async Task ChannelsCanBePassedIn()
    {
        var chartKey = CreateTestChart();
        var channels = new List<Channel>
        {
            new("channelKey1", "channel 1", "#FFFF00", 1, new[] {"A-1", "A-2"}, new Dictionary<string, int> { { "GA1", 3 } }),
            new("channelKey2", "channel 2", "#00FFFF", 2, new String[] { })
        };

        var evnt = await Client.Events.CreateAsync(chartKey, new CreateEventParams().WithChannels(channels));

        Assert.Equal(2, evnt.Channels.Count);

        var channel1 = evnt.Channels[0];
        Assert.Equal("channelKey1", channel1.Key);
        Assert.Equal("channel 1", channel1.Name);
        Assert.Equal("#FFFF00", channel1.Color);
        Assert.Equal(1, channel1.Index);
        Assert.Equal(new[] {"A-1", "A-2"}, channel1.Objects);
        Assert.Equal(new Dictionary<string, int> { { "GA1", 3 } }, channel1.AreaPlaces);
        Assert.NotNull(channel1.Id);

        var channel2 = evnt.Channels[1];
        Assert.Equal("channelKey2", channel2.Key);
        Assert.Equal("channel 2", channel2.Name);
        Assert.Equal("#00FFFF", channel2.Color);
        Assert.Equal(2, channel2.Index);
        Assert.NotNull(channel2.Id);
    }

    [Fact]
    public async Task ForSaleConfigCanBePassedIn()
    {
        var chartKey = CreateTestChart();
        var forSaleConfig = new ForSaleConfig().WithForSale(false).WithObjects(new[] {"A-1"}).WithAreaPlaces(new() {{"GA1", 5}}).WithCategories(new[] {"Cat1"});

        var evnt = await Client.Events.CreateAsync(chartKey, new CreateEventParams().WithForSaleConfig(forSaleConfig));

        Assert.Equivalent(forSaleConfig, evnt.ForSaleConfig);
    }
}