using System.Threading.Tasks;
using SeatsioDotNet.Events;
using Xunit;
using static SeatsioDotNet.EventReports.EventObjectInfo;

namespace SeatsioDotNet.Test.Reports.Events;

public class EventReportsDeepSummaryTest : SeatsioClientTest
{
    [Fact]
    public async Task WithSeasonBookingsNotPropagatedCanBeUsedToFetchAReportForAnEventInASeason()
    {
        var chartKey = CreateTestChart();
        var season = await Client.Seasons.CreateAsync(chartKey, numberOfEvents: 1);
        var evnt = season.Events[0];
        await Client.Events.BookAsync(season.Key, new[] {"A-1", "A-2"});

        var report = await Client.EventReports.WithSeasonBookingsNotPropagated().DeepSummaryByStatusAsync(evnt.Key);

        Assert.Equal(232, report[Free].Count);
    }

    [Fact]
    public async Task DeepSummaryByStatus()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.BookAsync(evnt.Key, new[] {new ObjectProperties("A-1")});

        var report = await Client.EventReports.DeepSummaryByStatusAsync(evnt.Key);

        Assert.Equal(1, report[Booked].Count);
        Assert.Equal(1, report[Booked].bySection[NoSection].Count);
        Assert.Equal(1, report[Booked].bySection[NoSection].byAvailability[NotAvailable]);
    }

    [Fact]
    public async Task DeepSummaryByStatusWithSeasonBookingsNotPropagated()
    {
        var chartKey = CreateTestChart();
        var season = await Client.Seasons.CreateAsync(chartKey, numberOfEvents: 1);
        var evnt = season.Events[0];
        await Client.Events.BookAsync(season.Key, new[] {"A-1", "A-2"});
        await Client.Events.BookAsync(evnt.Key, new[] {"A-3"});

        var reportWithPropagation = await Client.EventReports.DeepSummaryByStatusAsync(season.Key);
        var reportWithoutPropagation = await Client.EventReports.WithSeasonBookingsNotPropagated().DeepSummaryByStatusAsync(season.Key);

        Assert.Equal(3, reportWithPropagation[Booked].Count);
        Assert.Equal(2, reportWithoutPropagation[Booked].Count);
    }

    [Fact]
    public async Task DeepSummaryByObjectType()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);

        var report = await Client.EventReports.DeepSummaryByObjectTypeAsync(evnt.Key);

        Assert.Equal(32, report["seat"].Count);
        Assert.Equal(32, report["seat"].bySection[NoSection].Count);
        Assert.Equal(32, report["seat"].bySection[NoSection].byAvailability[Available]);
    }

    [Fact]
    public async Task DeepSummaryByObjectTypeWithSeasonBookingsNotPropagated()
    {
        var chartKey = CreateTestChart();
        var season = await Client.Seasons.CreateAsync(chartKey, numberOfEvents: 1);
        var evnt = season.Events[0];
        await Client.Events.BookAsync(season.Key, new[] {"A-1", "A-2"});
        await Client.Events.BookAsync(evnt.Key, new[] {"A-3"});

        var reportWithPropagation = await Client.EventReports.DeepSummaryByObjectTypeAsync(season.Key);
        var reportWithoutPropagation = await Client.EventReports.WithSeasonBookingsNotPropagated().DeepSummaryByObjectTypeAsync(season.Key);

        Assert.Equal(3, reportWithPropagation["seat"].byStatus[Booked].Count);
        Assert.Equal(2, reportWithoutPropagation["seat"].byStatus[Booked].Count);
    }

    [Fact]
    public async Task DeepSummaryByCategoryKey()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.BookAsync(evnt.Key, new[] {new ObjectProperties("A-1")});

        var report = await Client.EventReports.DeepSummaryByCategoryKeyAsync(evnt.Key);

        Assert.Equal(116, report["9"].Count);
        Assert.Equal(116, report["9"].bySection[NoSection].Count);
        Assert.Equal(1, report["9"].bySection[NoSection].byAvailability[NotAvailable]);
    }

    [Fact]
    public async Task DeepSummaryByCategoryKeyWithSeasonBookingsNotPropagated()
    {
        var chartKey = CreateTestChart();
        var season = await Client.Seasons.CreateAsync(chartKey, numberOfEvents: 1);
        var evnt = season.Events[0];
        await Client.Events.BookAsync(season.Key, new[] {"A-1", "A-2"});
        await Client.Events.BookAsync(evnt.Key, new[] {"A-3"});

        var reportWithPropagation = await Client.EventReports.DeepSummaryByCategoryKeyAsync(season.Key);
        var reportWithoutPropagation = await Client.EventReports.WithSeasonBookingsNotPropagated().DeepSummaryByCategoryKeyAsync(season.Key);

        Assert.Equal(3, reportWithPropagation["9"].byStatus[Booked].Count);
        Assert.Equal(2, reportWithoutPropagation["9"].byStatus[Booked].Count);
    }

    [Fact]
    public async Task DeepSummaryByCategoryLabel()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.BookAsync(evnt.Key, new[] {new ObjectProperties("A-1")});

        var report = await Client.EventReports.DeepSummaryByCategoryLabelAsync(evnt.Key);

        Assert.Equal(116, report["Cat1"].Count);
        Assert.Equal(116, report["Cat1"].bySection[NoSection].Count);
        Assert.Equal(1, report["Cat1"].bySection[NoSection].byAvailability[NotAvailable]);
    }

    [Fact]
    public async Task DeepSummaryByCategoryLabelWithSeasonBookingsNotPropagated()
    {
        var chartKey = CreateTestChart();
        var season = await Client.Seasons.CreateAsync(chartKey, numberOfEvents: 1);
        var evnt = season.Events[0];
        await Client.Events.BookAsync(season.Key, new[] {"A-1", "A-2"});
        await Client.Events.BookAsync(evnt.Key, new[] {"A-3"});

        var reportWithPropagation = await Client.EventReports.DeepSummaryByCategoryLabelAsync(season.Key);
        var reportWithoutPropagation = await Client.EventReports.WithSeasonBookingsNotPropagated().DeepSummaryByCategoryLabelAsync(season.Key);

        Assert.Equal(3, reportWithPropagation["Cat1"].byStatus[Booked].Count);
        Assert.Equal(2, reportWithoutPropagation["Cat1"].byStatus[Booked].Count);
    }

    [Fact]
    public async Task DeepSummaryBySection()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.BookAsync(evnt.Key, new[] {new ObjectProperties("A-1")});

        var report = await Client.EventReports.DeepSummaryBySectionAsync(evnt.Key);

        Assert.Equal(232, report[NoSection].Count);
        Assert.Equal(116, report[NoSection].byCategoryLabel["Cat1"].Count);
        Assert.Equal(1, report[NoSection].byCategoryLabel["Cat1"].byAvailability[NotAvailable]);
    }

    [Fact]
    public async Task DeepSummaryBySectionWithSeasonBookingsNotPropagated()
    {
        var chartKey = CreateTestChart();
        var season = await Client.Seasons.CreateAsync(chartKey, numberOfEvents: 1);
        var evnt = season.Events[0];
        await Client.Events.BookAsync(season.Key, new[] {"A-1", "A-2"});
        await Client.Events.BookAsync(evnt.Key, new[] {"A-3"});

        var reportWithPropagation = await Client.EventReports.DeepSummaryBySectionAsync(season.Key);
        var reportWithoutPropagation = await Client.EventReports.WithSeasonBookingsNotPropagated().DeepSummaryBySectionAsync(season.Key);

        Assert.Equal(3, reportWithPropagation[NoSection].byStatus[Booked].Count);
        Assert.Equal(2, reportWithoutPropagation[NoSection].byStatus[Booked].Count);
    }

    [Fact]
    public async Task DeepSummaryByZone()
    {
        var chartKey = CreateTestChartWithZones();
        var evnt = await Client.Events.CreateAsync(chartKey);

        var report = await Client.EventReports.DeepSummaryByZoneAsync(evnt.Key);

        Assert.Equal(6032, report["midtrack"].Count);
        Assert.Equal(6032, report["midtrack"].byCategoryLabel["Mid Track Stand"].Count);
    }

    [Fact]
    public async Task DeepSummaryByZoneWithSeasonBookingsNotPropagated()
    {
        var chartKey = CreateTestChart();
        var season = await Client.Seasons.CreateAsync(chartKey, numberOfEvents: 1);
        var evnt = season.Events[0];
        await Client.Events.BookAsync(season.Key, new[] {"A-1", "A-2"});
        await Client.Events.BookAsync(evnt.Key, new[] {"A-3"});

        var reportWithPropagation = await Client.EventReports.DeepSummaryByZoneAsync(season.Key);
        var reportWithoutPropagation = await Client.EventReports.WithSeasonBookingsNotPropagated().DeepSummaryByZoneAsync(season.Key);

        Assert.Equal(3, reportWithPropagation[NoZone].byStatus[Booked].Count);
        Assert.Equal(2, reportWithoutPropagation[NoZone].byStatus[Booked].Count);
    }

    [Fact]
    public async Task DeepSummaryByAvailability()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.BookAsync(evnt.Key, new[] {new ObjectProperties("A-1")});

        var report = await Client.EventReports.DeepSummaryByAvailabilityAsync(evnt.Key);

        Assert.Equal(1, report[NotAvailable].Count);
        Assert.Equal(1, report[NotAvailable].byCategoryLabel["Cat1"].Count);
        Assert.Equal(1, report[NotAvailable].byCategoryLabel["Cat1"].bySection[NoSection]);
    }

    [Fact]
    public async Task DeepSummaryByAvailabilityWithSeasonBookingsNotPropagated()
    {
        var chartKey = CreateTestChart();
        var season = await Client.Seasons.CreateAsync(chartKey, numberOfEvents: 1);
        var evnt = season.Events[0];
        await Client.Events.BookAsync(season.Key, new[] {"A-1", "A-2"});
        await Client.Events.BookAsync(evnt.Key, new[] {"A-3"});

        var reportWithPropagation = await Client.EventReports.DeepSummaryByAvailabilityAsync(season.Key);
        var reportWithoutPropagation = await Client.EventReports.WithSeasonBookingsNotPropagated().DeepSummaryByAvailabilityAsync(season.Key);

        Assert.Equal(3, reportWithPropagation[NotAvailable].Count);
        Assert.Equal(2, reportWithoutPropagation[NotAvailable].Count);
    }

    [Fact]
    public async Task DeepSummaryByAvailabilityReason()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.BookAsync(evnt.Key, new[] {new ObjectProperties("A-1")});

        var report = await Client.EventReports.DeepSummaryByAvailabilityReasonAsync(evnt.Key);

        Assert.Equal(1, report[Booked].Count);
        Assert.Equal(1, report[Booked].byCategoryLabel["Cat1"].Count);
        Assert.Equal(1, report[Booked].byCategoryLabel["Cat1"].bySection[NoSection]);
    }

    [Fact]
    public async Task DeepSummaryByAvailabilityReasonWithSeasonBookingsNotPropagated()
    {
        var chartKey = CreateTestChart();
        var season = await Client.Seasons.CreateAsync(chartKey, numberOfEvents: 1);
        var evnt = season.Events[0];
        await Client.Events.BookAsync(season.Key, new[] {"A-1", "A-2"});
        await Client.Events.BookAsync(evnt.Key, new[] {"A-3"});

        var reportWithPropagation = await Client.EventReports.DeepSummaryByAvailabilityReasonAsync(season.Key);
        var reportWithoutPropagation = await Client.EventReports.WithSeasonBookingsNotPropagated().DeepSummaryByAvailabilityReasonAsync(season.Key);

        Assert.Equal(3, reportWithPropagation[Booked].Count);
        Assert.Equal(2, reportWithoutPropagation[Booked].Count);
    }

    [Fact]
    public async Task DeepSummaryByChannel()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.BookAsync(evnt.Key, new[] {new ObjectProperties("A-1")});

        var report = await Client.EventReports.DeepSummaryByChannelAsync(evnt.Key);

        Assert.Equal(232, report[NoChannel].Count);
        Assert.Equal(116, report[NoChannel].byCategoryLabel["Cat1"].Count);
        Assert.Equal(116, report[NoChannel].byCategoryLabel["Cat1"].bySection[NoSection]);
    }

    [Fact]
    public async Task DeepSummaryByChannelWithSeasonBookingsNotPropagated()
    {
        var chartKey = CreateTestChart();
        var season = await Client.Seasons.CreateAsync(chartKey, numberOfEvents: 1);
        var evnt = season.Events[0];
        await Client.Events.BookAsync(season.Key, new[] {"A-1", "A-2"});
        await Client.Events.BookAsync(evnt.Key, new[] {"A-3"});

        var reportWithPropagation = await Client.EventReports.DeepSummaryByChannelAsync(season.Key);
        var reportWithoutPropagation = await Client.EventReports.WithSeasonBookingsNotPropagated().DeepSummaryByChannelAsync(season.Key);

        Assert.Equal(3, reportWithPropagation[NoChannel].byStatus[Booked].Count);
        Assert.Equal(2, reportWithoutPropagation[NoChannel].byStatus[Booked].Count);
    }
}