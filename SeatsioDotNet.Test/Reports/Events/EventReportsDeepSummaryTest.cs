﻿using SeatsioDotNet.Events;
using Xunit;
using static SeatsioDotNet.EventReports.EventObjectInfo;

namespace SeatsioDotNet.Test.Reports.Events;

public class EventReportsDeepSummaryTest : SeatsioClientTest
{
    [Fact]
    public void DeepSummaryByStatus()
    {
        var chartKey = CreateTestChart();
        var evnt = Client.Events.Create(chartKey);
        Client.Events.Book(evnt.Key, new[] {new ObjectProperties("A-1")});

        var report = Client.EventReports.DeepSummaryByStatus(evnt.Key);

        Assert.Equal(1, report[Booked].Count);
        Assert.Equal(1, report[Booked].bySection[NoSection].Count);
        Assert.Equal(1, report[Booked].bySection[NoSection].byAvailability[NotAvailable]);
    }

    [Fact]
    public void DeepSummaryByObjectType()
    {
        var chartKey = CreateTestChart();
        var evnt = Client.Events.Create(chartKey);

        var report = Client.EventReports.DeepSummaryByObjectType(evnt.Key);

        Assert.Equal(32, report["seat"].Count);
        Assert.Equal(32, report["seat"].bySection[NoSection].Count);
        Assert.Equal(32, report["seat"].bySection[NoSection].byAvailability[Available]);
    }

    [Fact]
    public void DeepSummaryByCategoryKey()
    {
        var chartKey = CreateTestChart();
        var evnt = Client.Events.Create(chartKey);
        Client.Events.Book(evnt.Key, new[] {new ObjectProperties("A-1")});

        var report = Client.EventReports.DeepSummaryByCategoryKey(evnt.Key);

        Assert.Equal(116, report["9"].Count);
        Assert.Equal(116, report["9"].bySection[NoSection].Count);
        Assert.Equal(1, report["9"].bySection[NoSection].byAvailability[NotAvailable]);
    }

    [Fact]
    public void DeepSummaryByCategoryLabel()
    {
        var chartKey = CreateTestChart();
        var evnt = Client.Events.Create(chartKey);
        Client.Events.Book(evnt.Key, new[] {new ObjectProperties("A-1")});

        var report = Client.EventReports.DeepSummaryByCategoryLabel(evnt.Key);

        Assert.Equal(116, report["Cat1"].Count);
        Assert.Equal(116, report["Cat1"].bySection[NoSection].Count);
        Assert.Equal(1, report["Cat1"].bySection[NoSection].byAvailability[NotAvailable]);
    }

    [Fact]
    public void DeepSummaryBySection()
    {
        var chartKey = CreateTestChart();
        var evnt = Client.Events.Create(chartKey);
        Client.Events.Book(evnt.Key, new[] {new ObjectProperties("A-1")});

        var report = Client.EventReports.DeepSummaryBySection(evnt.Key);

        Assert.Equal(232, report[NoSection].Count);
        Assert.Equal(116, report[NoSection].byCategoryLabel["Cat1"].Count);
        Assert.Equal(1, report[NoSection].byCategoryLabel["Cat1"].byAvailability[NotAvailable]);
    }

    [Fact]
    public void DeepSummaryByAvailability()
    {
        var chartKey = CreateTestChart();
        var evnt = Client.Events.Create(chartKey);
        Client.Events.Book(evnt.Key, new[] {new ObjectProperties("A-1")});

        var report = Client.EventReports.DeepSummaryByAvailability(evnt.Key);

        Assert.Equal(1, report[NotAvailable].Count);
        Assert.Equal(1, report[NotAvailable].byCategoryLabel["Cat1"].Count);
        Assert.Equal(1, report[NotAvailable].byCategoryLabel["Cat1"].bySection[NoSection]);
    }

    [Fact]
    public void DeepSummaryByAvailabilityReason()
    {
        var chartKey = CreateTestChart();
        var evnt = Client.Events.Create(chartKey);
        Client.Events.Book(evnt.Key, new[] {new ObjectProperties("A-1")});

        var report = Client.EventReports.DeepSummaryByAvailabilityReason(evnt.Key);

        Assert.Equal(1, report[Booked].Count);
        Assert.Equal(1, report[Booked].byCategoryLabel["Cat1"].Count);
        Assert.Equal(1, report[Booked].byCategoryLabel["Cat1"].bySection[NoSection]);
    }

    [Fact]
    public void DeepSummaryByChannel()
    {
        var chartKey = CreateTestChart();
        var evnt = Client.Events.Create(chartKey);
        Client.Events.Book(evnt.Key, new[] {new ObjectProperties("A-1")});

        var report = Client.EventReports.DeepSummaryByChannel(evnt.Key);

        Assert.Equal(232, report[NoChannel].Count);
        Assert.Equal(116, report[NoChannel].byCategoryLabel["Cat1"].Count);
        Assert.Equal(116, report[NoChannel].byCategoryLabel["Cat1"].bySection[NoSection]);
    }
}