using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using SeatsioDotNet.Events;
using Xunit;
using static SeatsioDotNet.EventReports.EventObjectInfo;

namespace SeatsioDotNet.Test.Reports.Events;

public class EventReportsTest : SeatsioClientTest
{
    [Fact]
    public async Task ReportItemProperties()
    {
        var chartKey = CreateTestChart();
        var channels = new List<Channel>
        {
            new("channelKey1", "channel 1", "#FFFF00", 1, new[] {"A-1", "A-2"})
        };
        var evnt = await Client.Events.CreateAsync(chartKey, new CreateEventParams().WithChannels(channels));
        var extraData = new Dictionary<string, object> {{"foo", "bar"}};
        await Client.Events.BookAsync(evnt.Key, new[] {new ObjectProperties("A-1", "ticketType1", extraData)}, null, "order1", null, true);

        var report = await Client.EventReports.ByLabelAsync(evnt.Key);

        var reportItem = report["A-1"].First();
        Assert.Equal("A-1", reportItem.Label);
        reportItem.Labels.Should().BeEquivalentTo(new Labels("1", "seat", "A", "row"));
        reportItem.IDs.Should().BeEquivalentTo(new IDs("1", "A", null));
        Assert.Equal(Booked, reportItem.Status);
        Assert.Equal("Cat1", reportItem.CategoryLabel);
        Assert.Equal("9", reportItem.CategoryKey);
        Assert.Equal("ticketType1", reportItem.TicketType);
        Assert.Equal("order1", reportItem.OrderId);
        Assert.Equal("seat", reportItem.ObjectType);
        Assert.True(reportItem.ForSale);
        Assert.Null(reportItem.Section);
        Assert.Null(reportItem.Entrance);
        Assert.Null(reportItem.NumBooked);
        Assert.Null(reportItem.Capacity);
        Assert.Equal(extraData, reportItem.ExtraData);
        Assert.False(reportItem.IsAccessible);
        Assert.False(reportItem.IsCompanionSeat);
        Assert.False(reportItem.HasRestrictedView);
        Assert.Null(reportItem.DisplayedObjectType);
        Assert.Null(reportItem.LeftNeighbour);
        Assert.Equal("A-2", reportItem.RightNeighbour);
        Assert.False(reportItem.IsAvailable);
        Assert.Equal("channelKey1", reportItem.Channel);
        Assert.Null(reportItem.BookAsAWhole);
        Assert.NotNull(reportItem.DistanceToFocalPoint);
        Assert.Equal(0, reportItem.SeasonStatusOverriddenQuantity);

        var gaItem = report["GA1"].First();
        Assert.True(gaItem.VariableOccupancy);
        Assert.Equal(1, gaItem.MinOccupancy);
        Assert.Equal(100, gaItem.MaxOccupancy);
    }

    [Fact]
    public async Task HoldToken()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        var holdToken = await Client.HoldTokens.CreateAsync();
        await Client.Events.HoldAsync(evnt.Key, new[] {"A-1"}, holdToken.Token);

        var report = await Client.EventReports.ByLabelAsync(evnt.Key);

        var reportItem = report["A-1"].First();
        Assert.Equal(holdToken.Token, reportItem.HoldToken);
    }  
    
    [Fact]
    public async Task SeasonStatusOverriddenQuantity()
    {
        var chartKey = CreateTestChart();
        var season = await Client.Seasons.CreateAsync(chartKey, numberOfEvents: 1);
        var evnt = season.Events[0];
        await Client.Events.OverrideSeasonObjectStatusAsync(evnt.Key, new[] {"A-1"});

        var report = await Client.EventReports.ByLabelAsync(evnt.Key);

        var reportItem = report["A-1"].First();
        Assert.Equal(1, reportItem.SeasonStatusOverriddenQuantity);
    }

    [Fact]
    public async Task ReportItemPropertiesForGA()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.BookAsync(evnt.Key, new[] {new ObjectProperties("GA1", 5)});
        var holdToken = await Client.HoldTokens.CreateAsync();
        await Client.Events.HoldAsync(evnt.Key, new[] {new ObjectProperties("GA1", 3)}, holdToken.Token);

        var report = await Client.EventReports.ByLabelAsync(evnt.Key);

        var reportItem = report["GA1"].First();
        Assert.Equal(5, reportItem.NumBooked);
        Assert.Equal(3, reportItem.NumHeld);
        Assert.Equal(92, reportItem.NumFree);
        Assert.Equal(100, reportItem.Capacity);
        Assert.Equal("generalAdmission", reportItem.ObjectType);
        Assert.Null(reportItem.IsAccessible);
        Assert.Null(reportItem.IsCompanionSeat);
        Assert.Null(reportItem.HasRestrictedView);
        Assert.Null(reportItem.DisplayedObjectType);
        Assert.False(reportItem.BookAsAWhole);
    }

    [Fact]
    public async Task ReportItemPropertiesForTable()
    {
        var chartKey = CreateTestChartWithTables();
        var evnt = await Client.Events.CreateAsync(chartKey, new CreateEventParams().WithTableBookingConfig(TableBookingConfig.AllByTable()));

        var report = await Client.EventReports.ByLabelAsync(evnt.Key);

        var reportItem = report["T1"].First();
        Assert.Equal(6, reportItem.NumSeats);
        Assert.False(reportItem.BookAsAWhole);
    }

    [Fact]
    public async Task ByStatus()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.ChangeObjectStatusAsync(evnt.Key, new[] {"A-1", "A-2"}, "lolzor");
        await Client.Events.ChangeObjectStatusAsync(evnt.Key, new[] {"A-3"}, Booked);

        var report = await Client.EventReports.ByStatusAsync(evnt.Key);

        Assert.Equal(2, report["lolzor"].Count());
        Assert.Single(report[Booked]);
        Assert.Equal(31, report[Free].Count());
    }

    [Fact]
    public async Task ByStatusEmptyChart()
    {
        var chartKey = (await Client.Charts.CreateAsync()).Key;
        var evnt = await Client.Events.CreateAsync(chartKey);

        var report = await Client.EventReports.ByStatusAsync(evnt.Key);

        Assert.Empty(report);
    }

    [Fact]
    public async Task BySpecificStatus()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.ChangeObjectStatusAsync(evnt.Key, new[] {"A-1", "A-2"}, "lolzor");

        var report = await Client.EventReports.ByStatusAsync(evnt.Key, "lolzor");

        Assert.Equal(2, report.Count());
    }

    [Fact]
    public async Task BySpecificNonExistingStatus()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);

        var report = await Client.EventReports.ByStatusAsync(evnt.Key, "lolzor");

        Assert.Empty(report);
    }

    [Fact]
    public async Task ByObjectType()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);

        var report = await Client.EventReports.ByObjectTypeAsync(evnt.Key);

        Assert.Equal(32, report["seat"].Count());
        Assert.Equal(2, report["generalAdmission"].Count());
        Assert.Equal(0, report["booth"].Count());
        Assert.Equal(0, report["table"].Count());
    }

    [Fact]
    public async Task BySpecificObjectType()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);

        var report = await Client.EventReports.ByObjectTypeAsync(evnt.Key, "seat");

        Assert.Equal(32, report.Count());
    }

    [Fact]
    public async Task ByCategoryLabel()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);

        var report = await Client.EventReports.ByCategoryLabelAsync(evnt.Key);

        Assert.Equal(17, report["Cat1"].Count());
        Assert.Equal(17, report["Cat2"].Count());
    }

    [Fact]
    public async Task BySpecificCategoryLabel()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);

        var report = await Client.EventReports.ByCategoryLabelAsync(evnt.Key, "Cat1");

        Assert.Equal(17, report.Count());
    }

    [Fact]
    public async Task ByCategoryKey()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);

        var report = await Client.EventReports.ByCategoryKeyAsync(evnt.Key);

        Assert.Equal(17, report["9"].Count());
        Assert.Equal(17, report["10"].Count());
    }

    [Fact]
    public async Task BySpecificCategoryKey()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);

        var report = await Client.EventReports.ByCategoryKeyAsync(evnt.Key, "9");

        Assert.Equal(17, report.Count());
    }

    [Fact]
    public async Task ByLabel()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);

        var report = await Client.EventReports.ByLabelAsync(evnt.Key);

        Assert.Single(report["A-1"]);
        Assert.Single(report["A-2"]);
    }

    [Fact]
    public async Task BySpecificLabel()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);

        var report = await Client.EventReports.ByLabelAsync(evnt.Key, "A-1");

        Assert.Single(report);
    }

    [Fact]
    public async Task ByOrderId()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.BookAsync(evnt.Key, new[] {"A-1", "A-2"}, null, "order1");
        await Client.Events.BookAsync(evnt.Key, new[] {"A-3"}, null, "order2");

        var report = await Client.EventReports.ByOrderIdAsync(evnt.Key);

        Assert.Equal(2, report["order1"].Count());
        Assert.Single(report["order2"]);
        Assert.Equal(31, report["NO_ORDER_ID"].Count());
    }

    [Fact]
    public async Task BySpecificOrderId()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.BookAsync(evnt.Key, new[] {"A-1", "A-2"}, null, "order1");

        var report = await Client.EventReports.ByOrderIdAsync(evnt.Key, "order1");

        Assert.Equal(2, report.Count());
    }

    [Fact]
    public async Task BySection()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);

        var report = await Client.EventReports.BySectionAsync(evnt.Key);

        Assert.Equal(34, report[NoSection].Count());
    }

    [Fact]
    public async Task BySpecificSection()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);

        var report = await Client.EventReports.BySectionAsync(evnt.Key, NoSection);

        Assert.Equal(34, report.Count());
    }
    
    [Fact]
    public async Task ByZone()
    {
        var chartKey = CreateTestChartWithZones();
        var evnt = await Client.Events.CreateAsync(chartKey);

        var report = await Client.EventReports.ByZoneAsync(evnt.Key);

        Assert.Equal(6032, report["midtrack"].Count());
        Assert.Equal("midtrack", report["midtrack"].First().Zone);
    }

    [Fact]
    public async Task BySpecificZone()
    {
        var chartKey = CreateTestChartWithZones();
        var evnt = await Client.Events.CreateAsync(chartKey);

        var report = await Client.EventReports.ByZoneAsync(evnt.Key, "midtrack");

        Assert.Equal(6032, report.Count());
    }

    [Fact]
    public async Task ByAvailability()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.BookAsync(evnt.Key, new[] {"A-1", "A-2"});

        var report = await Client.EventReports.ByAvailabilityAsync(evnt.Key);

        Assert.Equal(32, report[Available].Count());
        Assert.Equal(2, report[NotAvailable].Count());
    }

    [Fact]
    public async Task BySpecificAvailability()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.BookAsync(evnt.Key, new[] {"A-1", "A-2"});

        var report = await Client.EventReports.ByAvailabilityAsync(evnt.Key, NotAvailable);

        Assert.Equal(2, report.Count());
    }

    [Fact]
    public async Task ByAvailabilityReason()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.BookAsync(evnt.Key, new[] {"A-1", "A-2"});

        var report = await Client.EventReports.ByAvailabilityReasonAsync(evnt.Key);

        Assert.Equal(32, report[Available].Count());
        Assert.Equal(2, report[Booked].Count());
    }

    [Fact]
    public async Task BySpecificAvailabilityReason()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.BookAsync(evnt.Key, new[] {"A-1", "A-2"});

        var report = await Client.EventReports.ByAvailabilityReasonAsync(evnt.Key, Booked);

        Assert.Equal(2, report.Count());
    }

    [Fact]
    public async Task ByChannel()
    {
        var chartKey = CreateTestChart();
        var channels = new List<Channel>
        {
            new("channelKey1", "channel 1", "#FFFF00", 1, new[] {"A-1", "A-2"})
        };
        var evnt = await Client.Events.CreateAsync(chartKey, new CreateEventParams().WithChannels(channels));

        var report = await Client.EventReports.ByChannelAsync(evnt.Key);

        Assert.Equal(32, report[NoChannel].Count());
        Assert.Equal(2, report["channelKey1"].Count());
    }

    [Fact]
    public async Task BySpecificChannel()
    {
        var chartKey = CreateTestChart();
        var channels = new List<Channel>
        {
            new("channelKey1", "channel 1", "#FFFF00", 1, new[] {"A-1", "A-2"})
        };
        var evnt = await Client.Events.CreateAsync(chartKey, new CreateEventParams().WithChannels(channels));

        var report = await Client.EventReports.ByChannelAsync(evnt.Key, "channelKey1");

        Assert.Equal(2, report.Count());
    }
}