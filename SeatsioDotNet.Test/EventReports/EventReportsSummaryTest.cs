using System.Collections.Generic;
using SeatsioDotNet.Events;
using Xunit;
using static SeatsioDotNet.Events.ObjectStatus;
using static SeatsioDotNet.EventReports.EventReportItem;

namespace SeatsioDotNet.Test.EventReports
{
    public class EventReportsSummaryTest : SeatsioClientTest
    {
        [Fact]
        public void SummaryByStatus()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.Book(evnt.Key, new[] {new ObjectProperties("A-1", "ticketType1")});

            var report = Client.EventReports.SummaryByStatus(evnt.Key);

            Assert.Equal(1, report[Booked].Count);
            Assert.Equal(new Dictionary<string, int> {{"NO_SECTION", 1}}, report[Booked].bySection);
            Assert.Equal(new Dictionary<string, int> {{"9", 1}}, report[Booked].byCategoryKey);
            Assert.Equal(new Dictionary<string, int> {{"Cat1", 1}}, report[Booked].byCategoryLabel);
            Assert.Equal(new Dictionary<string, int> {{NotSelectable, 1}}, report[Booked].bySelectability);

            Assert.Equal(231, report[Free].Count);
            Assert.Equal(new Dictionary<string, int> {{"NO_SECTION", 231}}, report[Free].bySection);
            Assert.Equal(new Dictionary<string, int> {{"9", 115}, {"10", 116}}, report[Free].byCategoryKey);
            Assert.Equal(new Dictionary<string, int> {{"Cat1", 115}, {"Cat2", 116}}, report[Free].byCategoryLabel);
            Assert.Equal(new Dictionary<string, int> {{Selectable, 231}}, report[Free].bySelectability);
        }  
        
        [Fact]
        public void SummaryByCategoryKey()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.Book(evnt.Key, new[] {new ObjectProperties("A-1", "ticketType1")});

            var report = Client.EventReports.SummaryByCategoryKey(evnt.Key);

            Assert.Equal(116, report["9"].Count);
            Assert.Equal(new Dictionary<string, int> {{"NO_SECTION", 116}}, report["9"].bySection);
            Assert.Equal(new Dictionary<string, int> {{Booked, 1}, {Free, 115}}, report["9"].byStatus);
            Assert.Equal(new Dictionary<string, int> {{Selectable, 115}, {NotSelectable, 1}}, report["9"].bySelectability);

            Assert.Equal(116, report["10"].Count);
            Assert.Equal(new Dictionary<string, int> {{"NO_SECTION", 116}}, report["10"].bySection);
            Assert.Equal(new Dictionary<string, int> {{Free, 116}}, report["10"].byStatus);
            Assert.Equal(new Dictionary<string, int> {{Selectable, 116}}, report["10"].bySelectability);
        }  
        
        [Fact]
        public void SummaryByCategoryLabel()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.Book(evnt.Key, new[] {new ObjectProperties("A-1", "ticketType1")});

            var report = Client.EventReports.SummaryByCategoryLabel(evnt.Key);

            Assert.Equal(116, report["Cat1"].Count);
            Assert.Equal(new Dictionary<string, int> {{"NO_SECTION", 116}}, report["Cat1"].bySection);
            Assert.Equal(new Dictionary<string, int> {{Booked, 1}, {Free, 115}}, report["Cat1"].byStatus);
            Assert.Equal(new Dictionary<string, int> {{Selectable, 115}, {NotSelectable, 1}}, report["Cat1"].bySelectability);

            Assert.Equal(116, report["Cat2"].Count);
            Assert.Equal(new Dictionary<string, int> {{"NO_SECTION", 116}}, report["Cat2"].bySection);
            Assert.Equal(new Dictionary<string, int> {{Free, 116}}, report["Cat2"].byStatus);
            Assert.Equal(new Dictionary<string, int> {{Selectable, 116}}, report["Cat2"].bySelectability);
        }

        [Fact]
        public void SummaryBySection()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.Book(evnt.Key, new[] {new ObjectProperties("A-1", "ticketType1")});

            var report = Client.EventReports.SummaryBySection(evnt.Key);

            Assert.Equal(232, report["NO_SECTION"].Count);
            Assert.Equal(new Dictionary<string, int> {{Booked, 1}, {Free, 231}}, report["NO_SECTION"].byStatus);
            Assert.Equal(new Dictionary<string, int> {{"9", 116}, {"10", 116}}, report["NO_SECTION"].byCategoryKey);
            Assert.Equal(new Dictionary<string, int> {{"Cat1", 116}, {"Cat2", 116}}, report["NO_SECTION"].byCategoryLabel);
            Assert.Equal(new Dictionary<string, int> {{Selectable, 231}, {NotSelectable, 1}}, report["NO_SECTION"].bySelectability);
        }
        
        [Fact]
        public void SummaryBySelectability()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.Book(evnt.Key, new[] {new ObjectProperties("A-1", "ticketType1")});

            var report = Client.EventReports.SummaryBySelectability(evnt.Key);

            Assert.Equal(231, report[Selectable].Count);
            Assert.Equal(new Dictionary<string, int> {{"NO_SECTION", 231}}, report[Selectable].bySection);
            Assert.Equal(new Dictionary<string, int> {{Free, 231}}, report[Selectable].byStatus);
            Assert.Equal(new Dictionary<string, int> {{"9", 115}, {"10", 116}}, report[Selectable].byCategoryKey);

            Assert.Equal(1, report[NotSelectable].Count);
            Assert.Equal(new Dictionary<string, int> {{"NO_SECTION", 1}}, report[NotSelectable].bySection);
            Assert.Equal(new Dictionary<string, int> {{Booked, 1}}, report[NotSelectable].byStatus);
            Assert.Equal(new Dictionary<string, int> {{"9", 1}}, report[NotSelectable].byCategoryKey);
        }
    }
}