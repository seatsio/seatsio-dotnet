using System.Threading.Tasks;
using SeatsioDotNet.Events;
using Xunit;
using static SeatsioDotNet.EventReports.EventObjectInfo;

namespace SeatsioDotNet.Test.Reports.Events;

public class EventReportsDeepSummaryTest : SeatsioClientTest
{
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
}