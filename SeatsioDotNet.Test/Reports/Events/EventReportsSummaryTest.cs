using System.Collections.Generic;
using System.Threading.Tasks;
using SeatsioDotNet.Events;
using Xunit;
using static SeatsioDotNet.EventReports.EventObjectInfo;

namespace SeatsioDotNet.Test.Reports.Events;

public class EventReportsSummaryTest : SeatsioClientTest
{
    [Fact]
    public async Task WithSeasonBookingsNotPropagatedCanBeUsedToFetchAReportForAnEventInASeason()
    {
        var chartKey = CreateTestChart();
        var season = await Client.Seasons.CreateAsync(chartKey, numberOfEvents: 1);
        var evnt = season.Events[0];
        await Client.Events.BookAsync(season.Key, new[] {"A-1", "A-2"});

        var report = await Client.EventReports.WithSeasonBookingsNotPropagated().SummaryByStatusAsync(evnt.Key);

        Assert.Equal(232, report[Free].Count);
    }

    [Fact]
    public async Task SummaryByStatus()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.BookAsync(evnt.Key, new[] {new ObjectProperties("A-1")});

        var report = await Client.EventReports.SummaryByStatusAsync(evnt.Key);

        Assert.Equal(1, report[Booked].Count);
        Assert.Equal(new() {{NoSection, 1}}, report[Booked].bySection);
        Assert.Equal(new() {{"9", 1}}, report[Booked].byCategoryKey);
        Assert.Equal(new() {{"Cat1", 1}}, report[Booked].byCategoryLabel);
        Assert.Equal(new() {{NotAvailable, 1}}, report[Booked].byAvailability);
        Assert.Equal(new() {{NoChannel, 1}}, report[Booked].byChannel);

        Assert.Equal(231, report[Free].Count);
        Assert.Equal(new() {{NoSection, 231}}, report[Free].bySection);
        Assert.Equal(new() {{"9", 115}, {"10", 116}}, report[Free].byCategoryKey);
        Assert.Equal(new() {{"Cat1", 115}, {"Cat2", 116}}, report[Free].byCategoryLabel);
        Assert.Equal(new() {{Available, 231}}, report[Free].byAvailability);
        Assert.Equal(new() {{NoChannel, 231}}, report[Free].byChannel);
    }

    [Fact]
    public async Task SummaryByStatusWithSeasonBookingsNotPropagated()
    {
        var chartKey = CreateTestChart();
        var season = await Client.Seasons.CreateAsync(chartKey, numberOfEvents: 1);
        var evnt = season.Events[0];
        await Client.Events.BookAsync(season.Key, new[] {"A-1", "A-2"});
        await Client.Events.BookAsync(evnt.Key, new[] {"A-3"});

        var reportWithPropagation = await Client.EventReports.SummaryByStatusAsync(season.Key);
        var reportWithoutPropagation = await Client.EventReports.WithSeasonBookingsNotPropagated().SummaryByStatusAsync(season.Key);

        Assert.Equal(3, reportWithPropagation[Booked].Count);
        Assert.Equal(2, reportWithoutPropagation[Booked].Count);
    }

    [Fact]
    public async Task SummaryByObjectType()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);

        var report = await Client.EventReports.SummaryByObjectTypeAsync(evnt.Key);

        Assert.Equal(32, report["seat"].Count);
        Assert.Equal(new() {{NoSection, 32}}, report["seat"].bySection);
        Assert.Equal(new() {{"9", 16}, {"10", 16}}, report["seat"].byCategoryKey);
        Assert.Equal(new() {{"Cat1", 16}, {"Cat2", 16}}, report["seat"].byCategoryLabel);
        Assert.Equal(new() {{Available, 32}}, report["seat"].byAvailability);
        Assert.Equal(new() {{NoChannel, 32}}, report["seat"].byChannel);

        Assert.Equal(200, report["generalAdmission"].Count);
        Assert.Equal(new() {{NoSection, 200}}, report["generalAdmission"].bySection);
        Assert.Equal(new() {{"9", 100}, {"10", 100}}, report["generalAdmission"].byCategoryKey);
        Assert.Equal(new() {{"Cat1", 100}, {"Cat2", 100}}, report["generalAdmission"].byCategoryLabel);
        Assert.Equal(new() {{Available, 200}}, report["generalAdmission"].byAvailability);
        Assert.Equal(new() {{NoChannel, 200}}, report["generalAdmission"].byChannel);
    }

    [Fact]
    public async Task SummaryByObjectTypeWithSeasonBookingsNotPropagated()
    {
        var chartKey = CreateTestChart();
        var season = await Client.Seasons.CreateAsync(chartKey, numberOfEvents: 1);
        var evnt = season.Events[0];
        await Client.Events.BookAsync(season.Key, new[] {"A-1", "A-2"});
        await Client.Events.BookAsync(evnt.Key, new[] {"A-3"});

        var reportWithPropagation = await Client.EventReports.SummaryByObjectTypeAsync(season.Key);
        var reportWithoutPropagation = await Client.EventReports.WithSeasonBookingsNotPropagated().SummaryByObjectTypeAsync(season.Key);

        Assert.Equal(3, reportWithPropagation["seat"].byStatus[Booked]);
        Assert.Equal(2, reportWithoutPropagation["seat"].byStatus[Booked]);
    }

    [Fact]
    public async Task SummaryByCategoryKey()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.BookAsync(evnt.Key, new[] {new ObjectProperties("A-1")});

        var report = await Client.EventReports.SummaryByCategoryKeyAsync(evnt.Key);

        Assert.Equal(116, report["9"].Count);
        Assert.Equal(new() {{NoSection, 116}}, report["9"].bySection);
        Assert.Equal(new() {{Booked, 1}, {Free, 115}}, report["9"].byStatus);
        Assert.Equal(new() {{Available, 115}, {NotAvailable, 1}}, report["9"].byAvailability);
        Assert.Equal(new() {{NoChannel, 116}}, report["9"].byChannel);

        Assert.Equal(116, report["10"].Count);
        Assert.Equal(new() {{NoSection, 116}}, report["10"].bySection);
        Assert.Equal(new() {{Free, 116}}, report["10"].byStatus);
        Assert.Equal(new() {{Available, 116}}, report["10"].byAvailability);
        Assert.Equal(new() {{NoChannel, 116}}, report["10"].byChannel);

        Assert.Equal(0, report["NO_CATEGORY"].Count);
    }

    [Fact]
    public async Task SummaryByCategoryKeyWithSeasonBookingsNotPropagated()
    {
        var chartKey = CreateTestChart();
        var season = await Client.Seasons.CreateAsync(chartKey, numberOfEvents: 1);
        var evnt = season.Events[0];
        await Client.Events.BookAsync(season.Key, new[] {"A-1", "A-2"});
        await Client.Events.BookAsync(evnt.Key, new[] {"A-3"});

        var reportWithPropagation = await Client.EventReports.SummaryByCategoryKeyAsync(season.Key);
        var reportWithoutPropagation = await Client.EventReports.WithSeasonBookingsNotPropagated().SummaryByCategoryKeyAsync(season.Key);

        Assert.Equal(3, reportWithPropagation["9"].byStatus[Booked]);
        Assert.Equal(2, reportWithoutPropagation["9"].byStatus[Booked]);
    }

    [Fact]
    public async Task SummaryByCategoryLabel()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.BookAsync(evnt.Key, new[] {new ObjectProperties("A-1")});

        var report = await Client.EventReports.SummaryByCategoryLabeAsync(evnt.Key);

        Assert.Equal(116, report["Cat1"].Count);
        Assert.Equal(new() {{NoSection, 116}}, report["Cat1"].bySection);
        Assert.Equal(new() {{Booked, 1}, {Free, 115}}, report["Cat1"].byStatus);
        Assert.Equal(new() {{Available, 115}, {NotAvailable, 1}}, report["Cat1"].byAvailability);
        Assert.Equal(new() {{NoChannel, 116}}, report["Cat1"].byChannel);


        Assert.Equal(116, report["Cat2"].Count);
        Assert.Equal(new() {{NoSection, 116}}, report["Cat2"].bySection);
        Assert.Equal(new() {{Free, 116}}, report["Cat2"].byStatus);
        Assert.Equal(new() {{Available, 116}}, report["Cat2"].byAvailability);
        Assert.Equal(new() {{NoChannel, 116}}, report["Cat2"].byChannel);

        Assert.Equal(0, report["NO_CATEGORY"].Count);
    }

    [Fact]
    public async Task SummaryByCategoryLabelWithSeasonBookingsNotPropagated()
    {
        var chartKey = CreateTestChart();
        var season = await Client.Seasons.CreateAsync(chartKey, numberOfEvents: 1);
        var evnt = season.Events[0];
        await Client.Events.BookAsync(season.Key, new[] {"A-1", "A-2"});
        await Client.Events.BookAsync(evnt.Key, new[] {"A-3"});

        var reportWithPropagation = await Client.EventReports.SummaryByCategoryLabeAsync(season.Key);
        var reportWithoutPropagation = await Client.EventReports.WithSeasonBookingsNotPropagated().SummaryByCategoryLabeAsync(season.Key);

        Assert.Equal(3, reportWithPropagation["Cat1"].byStatus[Booked]);
        Assert.Equal(2, reportWithoutPropagation["Cat1"].byStatus[Booked]);
    }

    [Fact]
    public async Task SummaryBySection()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.BookAsync(evnt.Key, new[] {new ObjectProperties("A-1")});

        var report = await Client.EventReports.SummaryBySectionAsync(evnt.Key);

        Assert.Equal(232, report[NoSection].Count);
        Assert.Equal(new() {{Booked, 1}, {Free, 231}}, report[NoSection].byStatus);
        Assert.Equal(new() {{"9", 116}, {"10", 116}}, report[NoSection].byCategoryKey);
        Assert.Equal(new() {{"Cat1", 116}, {"Cat2", 116}}, report[NoSection].byCategoryLabel);
        Assert.Equal(new() {{Available, 231}, {NotAvailable, 1}}, report[NoSection].byAvailability);
        Assert.Equal(new() {{NoChannel, 232}}, report[NoSection].byChannel);
        Assert.Equal(new() {{NoZone, 232}}, report[NoSection].byZone);
    }

    [Fact]
    public async Task SummaryBySectionWithSeasonBookingsNotPropagated()
    {
        var chartKey = CreateTestChart();
        var season = await Client.Seasons.CreateAsync(chartKey, numberOfEvents: 1);
        var evnt = season.Events[0];
        await Client.Events.BookAsync(season.Key, new[] {"A-1", "A-2"});
        await Client.Events.BookAsync(evnt.Key, new[] {"A-3"});

        var reportWithPropagation = await Client.EventReports.SummaryBySectionAsync(season.Key);
        var reportWithoutPropagation = await Client.EventReports.WithSeasonBookingsNotPropagated().SummaryBySectionAsync(season.Key);

        Assert.Equal(3, reportWithPropagation[NoSection].byStatus[Booked]);
        Assert.Equal(2, reportWithoutPropagation[NoSection].byStatus[Booked]);
    }

    [Fact]
    public async Task SummaryByZone()
    {
        var chartKey = CreateTestChartWithZones();
        var evnt = await Client.Events.CreateAsync(chartKey);

        var report = await Client.EventReports.SummaryByZoneAsync(evnt.Key);

        Assert.Equal(6032, report["midtrack"].Count);
        Assert.Equal(new() {{Free, 6032}}, report["midtrack"].byStatus);
    }

    [Fact]
    public async Task SummaryByZoneWithSeasonBookingsNotPropagated()
    {
        var chartKey = CreateTestChart();
        var season = await Client.Seasons.CreateAsync(chartKey, numberOfEvents: 1);
        var evnt = season.Events[0];
        await Client.Events.BookAsync(season.Key, new[] {"A-1", "A-2"});
        await Client.Events.BookAsync(evnt.Key, new[] {"A-3"});

        var reportWithPropagation = await Client.EventReports.SummaryByZoneAsync(season.Key);
        var reportWithoutPropagation = await Client.EventReports.WithSeasonBookingsNotPropagated().SummaryByZoneAsync(season.Key);

        Assert.Equal(3, reportWithPropagation[NoZone].byStatus[Booked]);
        Assert.Equal(2, reportWithoutPropagation[NoZone].byStatus[Booked]);
    }

    [Fact]
    public async Task SummaryByAvailability()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.BookAsync(evnt.Key, new[] {new ObjectProperties("A-1")});

        var report = await Client.EventReports.SummaryByAvailabilityAsync(evnt.Key);

        Assert.Equal(231, report[Available].Count);
        Assert.Equal(new() {{NoSection, 231}}, report[Available].bySection);
        Assert.Equal(new() {{Free, 231}}, report[Available].byStatus);
        Assert.Equal(new() {{"9", 115}, {"10", 116}}, report[Available].byCategoryKey);
        Assert.Equal(new() {{NoChannel, 231}}, report[Available].byChannel);

        Assert.Equal(1, report[NotAvailable].Count);
        Assert.Equal(new() {{NoSection, 1}}, report[NotAvailable].bySection);
        Assert.Equal(new() {{Booked, 1}}, report[NotAvailable].byStatus);
        Assert.Equal(new() {{"9", 1}}, report[NotAvailable].byCategoryKey);
        Assert.Equal(new() {{NoChannel, 1}}, report[NotAvailable].byChannel);
    }

    [Fact]
    public async Task SummaryByAvailabilityWithSeasonBookingsNotPropagated()
    {
        var chartKey = CreateTestChart();
        var season = await Client.Seasons.CreateAsync(chartKey, numberOfEvents: 1);
        var evnt = season.Events[0];
        await Client.Events.BookAsync(season.Key, new[] {"A-1", "A-2"});
        await Client.Events.BookAsync(evnt.Key, new[] {"A-3"});

        var reportWithPropagation = await Client.EventReports.SummaryByAvailabilityAsync(season.Key);
        var reportWithoutPropagation = await Client.EventReports.WithSeasonBookingsNotPropagated().SummaryByAvailabilityAsync(season.Key);

        Assert.Equal(3, reportWithPropagation[NotAvailable].Count);
        Assert.Equal(2, reportWithoutPropagation[NotAvailable].Count);
    }

    [Fact]
    public async Task SummaryByAvailabilityReason()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.BookAsync(evnt.Key, new[] {new ObjectProperties("A-1")});

        var report = await Client.EventReports.SummaryByAvailabilityReasonAsync(evnt.Key);

        Assert.Equal(231, report[Available].Count);
        Assert.Equal(new() {{NoSection, 231}}, report[Available].bySection);
        Assert.Equal(new() {{Free, 231}}, report[Available].byStatus);
        Assert.Equal(new() {{"9", 115}, {"10", 116}}, report[Available].byCategoryKey);
        Assert.Equal(new() {{NoChannel, 231}}, report[Available].byChannel);

        Assert.Equal(1, report[Booked].Count);
        Assert.Equal(new() {{NoSection, 1}}, report[Booked].bySection);
        Assert.Equal(new() {{Booked, 1}}, report[Booked].byStatus);
        Assert.Equal(new() {{"9", 1}}, report[Booked].byCategoryKey);
        Assert.Equal(new() {{NoChannel, 1}}, report[Booked].byChannel);
    }

    [Fact]
    public async Task SummaryByAvailabilityReasonWithSeasonBookingsNotPropagated()
    {
        var chartKey = CreateTestChart();
        var season = await Client.Seasons.CreateAsync(chartKey, numberOfEvents: 1);
        var evnt = season.Events[0];
        await Client.Events.BookAsync(season.Key, new[] {"A-1", "A-2"});
        await Client.Events.BookAsync(evnt.Key, new[] {"A-3"});

        var reportWithPropagation = await Client.EventReports.SummaryByAvailabilityReasonAsync(season.Key);
        var reportWithoutPropagation = await Client.EventReports.WithSeasonBookingsNotPropagated().SummaryByAvailabilityReasonAsync(season.Key);

        Assert.Equal(3, reportWithPropagation[Booked].Count);
        Assert.Equal(2, reportWithoutPropagation[Booked].Count);
    }

    [Fact]
    public async Task SummaryByChannel()
    {
        var chartKey = CreateTestChart();
        var channels = new List<Channel>
        {
            new("channelKey1", null, "channel 1", "#FFFF00", 1, new[] {"A-1", "A-2"}, new Dictionary<string, int>())
        };
        var evnt = await Client.Events.CreateAsync(chartKey, new CreateEventParams().WithChannels(channels));

        var report = await Client.EventReports.SummaryByChannelAsync(evnt.Key);

        Assert.Equal(230, report[NoChannel].Count);
        Assert.Equal(new() {{NoSection, 230}}, report[NoChannel].bySection);
        Assert.Equal(new() {{Free, 230}}, report[NoChannel].byStatus);
        Assert.Equal(new() {{"9", 114}, {"10", 116}}, report[NoChannel].byCategoryKey);
        Assert.Equal(new() {{Available, 230}}, report[NoChannel].byAvailability);

        Assert.Equal(2, report["channelKey1"].Count);
        Assert.Equal(new() {{NoSection, 2}}, report["channelKey1"].bySection);
        Assert.Equal(new() {{Free, 2}}, report["channelKey1"].byStatus);
        Assert.Equal(new() {{"9", 2}}, report["channelKey1"].byCategoryKey);
        Assert.Equal(new() {{Available, 2}}, report["channelKey1"].byAvailability);
    }

    [Fact]
    public async Task SummaryByChannelWithSeasonBookingsNotPropagated()
    {
        var chartKey = CreateTestChart();
        var season = await Client.Seasons.CreateAsync(chartKey, numberOfEvents: 1);
        var evnt = season.Events[0];
        await Client.Events.BookAsync(season.Key, new[] {"A-1", "A-2"});
        await Client.Events.BookAsync(evnt.Key, new[] {"A-3"});

        var reportWithPropagation = await Client.EventReports.SummaryByChannelAsync(season.Key);
        var reportWithoutPropagation = await Client.EventReports.WithSeasonBookingsNotPropagated().SummaryByChannelAsync(season.Key);

        Assert.Equal(3, reportWithPropagation[NoChannel].byStatus[Booked]);
        Assert.Equal(2, reportWithoutPropagation[NoChannel].byStatus[Booked]);
    }
}