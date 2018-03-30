using System.Collections.Generic;
using SeatsioDotNet.Events;
using Xunit;
using static SeatsioDotNet.Events.ObjectStatus;

namespace SeatsioDotNet.Test.EventReports
{
    public class EventReportsSummaryTest : SeatsioClientTest
    {
        [Fact]
        public void SummaryByStatus()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.Book(evnt.Key, new[] {new ObjectProperties("A-1", "ticketType1")}, null, "order1");

            var report = Client.EventReports.SummaryByStatus(evnt.Key);

            Assert.Equal(1, report[Booked].Count);
            Assert.Equal(new Dictionary<string, int> {{"NO_SECTION", 1}}, report[Booked].bySection);
            Assert.Equal(new Dictionary<string, int> {{"9", 1}}, report[Booked].byCategoryKey);
            Assert.Equal(new Dictionary<string, int> {{"Cat1", 1}}, report[Booked].byCategoryLabel);

            Assert.Equal(33, report[Free].Count);
            Assert.Equal(new Dictionary<string, int> {{"NO_SECTION", 33}}, report[Free].bySection);
            Assert.Equal(new Dictionary<string, int> {{"9", 16}, {"10", 17}}, report[Free].byCategoryKey);
            Assert.Equal(new Dictionary<string, int> {{"Cat1", 16}, {"Cat2", 17}}, report[Free].byCategoryLabel);
        }  
        
        [Fact]
        public void SummaryByCategoryKey()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.Book(evnt.Key, new[] {new ObjectProperties("A-1", "ticketType1")}, null, "order1");

            var report = Client.EventReports.SummaryByCategoryKey(evnt.Key);

            Assert.Equal(17, report["9"].Count);
            Assert.Equal(new Dictionary<string, int> {{"NO_SECTION", 17}}, report["9"].bySection);
            Assert.Equal(new Dictionary<string, int> {{Booked, 1}, {Free, 16}}, report["9"].byStatus);

            Assert.Equal(17, report["10"].Count);
            Assert.Equal(new Dictionary<string, int> {{"NO_SECTION", 17}}, report["10"].bySection);
            Assert.Equal(new Dictionary<string, int> {{Free, 17}}, report["10"].byStatus);
        }  
        
        [Fact]
        public void SummaryByCategoryLabel()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.Book(evnt.Key, new[] {new ObjectProperties("A-1", "ticketType1")}, null, "order1");

            var report = Client.EventReports.SummaryByCategoryLabel(evnt.Key);

            Assert.Equal(17, report["Cat1"].Count);
            Assert.Equal(new Dictionary<string, int> {{"NO_SECTION", 17}}, report["Cat1"].bySection);
            Assert.Equal(new Dictionary<string, int> {{Booked, 1}, {Free, 16}}, report["Cat1"].byStatus);

            Assert.Equal(17, report["Cat2"].Count);
            Assert.Equal(new Dictionary<string, int> {{"NO_SECTION", 17}}, report["Cat2"].bySection);
            Assert.Equal(new Dictionary<string, int> {{Free, 17}}, report["Cat2"].byStatus);
        }

        [Fact]
        public void SummaryBySection()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.Book(evnt.Key, new[] {new ObjectProperties("A-1", "ticketType1")}, null, "order1");

            var report = Client.EventReports.SummaryBySection(evnt.Key);

            Assert.Equal(34, report["NO_SECTION"].Count);
            Assert.Equal(new Dictionary<string, int> {{Booked, 1}, {Free, 33}}, report["NO_SECTION"].byStatus);
            Assert.Equal(new Dictionary<string, int> {{"9", 17}, {"10", 17}}, report["NO_SECTION"].byCategoryKey);
            Assert.Equal(new Dictionary<string, int> {{"Cat1", 17}, {"Cat2", 17}}, report["NO_SECTION"].byCategoryLabel);
        }
    }
}