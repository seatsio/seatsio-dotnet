using System.Collections.Generic;
using SeatsioDotNet.Events;
using Xunit;
using static SeatsioDotNet.EventReports.EventObjectInfo;

namespace SeatsioDotNet.Test.Reports.Events;

public class EventReportsSummaryTest : SeatsioClientTest
{
    [Fact]
    public void SummaryByStatus()
    {
        var chartKey = CreateTestChart();
        var evnt = Client.Events.Create(chartKey);
        Client.Events.Book(evnt.Key, new[] {new ObjectProperties("A-1")});

        var report = Client.EventReports.SummaryByStatus(evnt.Key);

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
    public void SummaryByObjectType()
    {
        var chartKey = CreateTestChart();
        var evnt = Client.Events.Create(chartKey);

        var report = Client.EventReports.SummaryByObjectType(evnt.Key);

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
    public void SummaryByCategoryKey()
    {
        var chartKey = CreateTestChart();
        var evnt = Client.Events.Create(chartKey);
        Client.Events.Book(evnt.Key, new[] {new ObjectProperties("A-1")});

        var report = Client.EventReports.SummaryByCategoryKey(evnt.Key);

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
    public void SummaryByCategoryLabel()
    {
        var chartKey = CreateTestChart();
        var evnt = Client.Events.Create(chartKey);
        Client.Events.Book(evnt.Key, new[] {new ObjectProperties("A-1")});

        var report = Client.EventReports.SummaryByCategoryLabel(evnt.Key);

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
    public void SummaryBySection()
    {
        var chartKey = CreateTestChart();
        var evnt = Client.Events.Create(chartKey);
        Client.Events.Book(evnt.Key, new[] {new ObjectProperties("A-1")});

        var report = Client.EventReports.SummaryBySection(evnt.Key);

        Assert.Equal(232, report[NoSection].Count);
        Assert.Equal(new() {{Booked, 1}, {Free, 231}}, report[NoSection].byStatus);
        Assert.Equal(new() {{"9", 116}, {"10", 116}}, report[NoSection].byCategoryKey);
        Assert.Equal(new() {{"Cat1", 116}, {"Cat2", 116}}, report[NoSection].byCategoryLabel);
        Assert.Equal(new() {{Available, 231}, {NotAvailable, 1}}, report[NoSection].byAvailability);
        Assert.Equal(new() {{NoChannel, 232}}, report[NoSection].byChannel);
    }

    [Fact]
    public void SummaryByAvailability()
    {
        var chartKey = CreateTestChart();
        var evnt = Client.Events.Create(chartKey);
        Client.Events.Book(evnt.Key, new[] {new ObjectProperties("A-1")});

        var report = Client.EventReports.SummaryByAvailability(evnt.Key);

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
    public void SummaryByAvailabilityReason()
    {
        var chartKey = CreateTestChart();
        var evnt = Client.Events.Create(chartKey);
        Client.Events.Book(evnt.Key, new[] {new ObjectProperties("A-1")});

        var report = Client.EventReports.SummaryByAvailabilityReason(evnt.Key);

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
    public void SummaryByChannel()
    {
        var chartKey = CreateTestChart();
        var channels = new List<Channel>
        {
            new("channelKey1", "channel 1", "#FFFF00", 1, new[] {"A-1", "A-2"})
        };
        var evnt = Client.Events.Create(chartKey, new CreateEventParams().WithChannels(channels));

        var report = Client.EventReports.SummaryByChannel(evnt.Key);

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
}