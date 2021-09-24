using System.Collections.Generic;
using SeatsioDotNet.Events;
using Xunit;
using static SeatsioDotNet.EventReports.EventObjectInfo;

namespace SeatsioDotNet.Test.EventReports
{
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
            Assert.Equal(new Dictionary<string, int> {{NoSection, 1}}, report[Booked].bySection);
            Assert.Equal(new Dictionary<string, int> {{"9", 1}}, report[Booked].byCategoryKey);
            Assert.Equal(new Dictionary<string, int> {{"Cat1", 1}}, report[Booked].byCategoryLabel);
            Assert.Equal(new Dictionary<string, int> {{NotSelectable, 1}}, report[Booked].bySelectability);
            Assert.Equal(new Dictionary<string, int> {{NoChannel, 1}}, report[Booked].byChannel);

            Assert.Equal(231, report[Free].Count);
            Assert.Equal(new Dictionary<string, int> {{NoSection, 231}}, report[Free].bySection);
            Assert.Equal(new Dictionary<string, int> {{"9", 115}, {"10", 116}}, report[Free].byCategoryKey);
            Assert.Equal(new Dictionary<string, int> {{"Cat1", 115}, {"Cat2", 116}}, report[Free].byCategoryLabel);
            Assert.Equal(new Dictionary<string, int> {{Selectable, 231}}, report[Free].bySelectability);
            Assert.Equal(new Dictionary<string, int> {{NoChannel, 231}}, report[Free].byChannel);
        } 
        
        [Fact]
        public void SummaryByObjectType()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            var report = Client.EventReports.SummaryByObjectType(evnt.Key);

            Assert.Equal(32, report["seat"].Count);
            Assert.Equal(new Dictionary<string, int> {{NoSection, 32}}, report["seat"].bySection);
            Assert.Equal(new Dictionary<string, int> {{"9", 16}, {"10", 16}}, report["seat"].byCategoryKey);
            Assert.Equal(new Dictionary<string, int> {{"Cat1", 16}, {"Cat2", 16}}, report["seat"].byCategoryLabel);
            Assert.Equal(new Dictionary<string, int> {{Selectable, 32}}, report["seat"].bySelectability);
            Assert.Equal(new Dictionary<string, int> {{NoChannel, 32}}, report["seat"].byChannel);

            Assert.Equal(200, report["generalAdmission"].Count);
            Assert.Equal(new Dictionary<string, int> {{NoSection, 200}}, report["generalAdmission"].bySection);
            Assert.Equal(new Dictionary<string, int> {{"9", 100}, {"10", 100}}, report["generalAdmission"].byCategoryKey);
            Assert.Equal(new Dictionary<string, int> {{"Cat1", 100}, {"Cat2", 100}}, report["generalAdmission"].byCategoryLabel);
            Assert.Equal(new Dictionary<string, int> {{Selectable, 200}}, report["generalAdmission"].bySelectability);
            Assert.Equal(new Dictionary<string, int> {{NoChannel, 200}}, report["generalAdmission"].byChannel);
        }  
        
        [Fact]
        public void SummaryByCategoryKey()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.Book(evnt.Key, new[] {new ObjectProperties("A-1")});

            var report = Client.EventReports.SummaryByCategoryKey(evnt.Key);

            Assert.Equal(116, report["9"].Count);
            Assert.Equal(new Dictionary<string, int> {{NoSection, 116}}, report["9"].bySection);
            Assert.Equal(new Dictionary<string, int> {{Booked, 1}, {Free, 115}}, report["9"].byStatus);
            Assert.Equal(new Dictionary<string, int> {{Selectable, 115}, {NotSelectable, 1}}, report["9"].bySelectability);
            Assert.Equal(new Dictionary<string, int> {{NoChannel, 116}}, report["9"].byChannel);

            Assert.Equal(116, report["10"].Count);
            Assert.Equal(new Dictionary<string, int> {{NoSection, 116}}, report["10"].bySection);
            Assert.Equal(new Dictionary<string, int> {{Free, 116}}, report["10"].byStatus);
            Assert.Equal(new Dictionary<string, int> {{Selectable, 116}}, report["10"].bySelectability);
            Assert.Equal(new Dictionary<string, int> {{NoChannel, 116}}, report["10"].byChannel);
            
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
            Assert.Equal(new Dictionary<string, int> {{NoSection, 116}}, report["Cat1"].bySection);
            Assert.Equal(new Dictionary<string, int> {{Booked, 1}, {Free, 115}}, report["Cat1"].byStatus);
            Assert.Equal(new Dictionary<string, int> {{Selectable, 115}, {NotSelectable, 1}}, report["Cat1"].bySelectability);
            Assert.Equal(new Dictionary<string, int> {{NoChannel, 116}}, report["Cat1"].byChannel);


            Assert.Equal(116, report["Cat2"].Count);
            Assert.Equal(new Dictionary<string, int> {{NoSection, 116}}, report["Cat2"].bySection);
            Assert.Equal(new Dictionary<string, int> {{Free, 116}}, report["Cat2"].byStatus);
            Assert.Equal(new Dictionary<string, int> {{Selectable, 116}}, report["Cat2"].bySelectability);
            Assert.Equal(new Dictionary<string, int> {{NoChannel, 116}}, report["Cat2"].byChannel);
            
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
            Assert.Equal(new Dictionary<string, int> {{Booked, 1}, {Free, 231}}, report[NoSection].byStatus);
            Assert.Equal(new Dictionary<string, int> {{"9", 116}, {"10", 116}}, report[NoSection].byCategoryKey);
            Assert.Equal(new Dictionary<string, int> {{"Cat1", 116}, {"Cat2", 116}}, report[NoSection].byCategoryLabel);
            Assert.Equal(new Dictionary<string, int> {{Selectable, 231}, {NotSelectable, 1}}, report[NoSection].bySelectability);
            Assert.Equal(new Dictionary<string, int> {{NoChannel, 232}}, report[NoSection].byChannel);
        }
        
        [Fact]
        public void SummaryBySelectability()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.Book(evnt.Key, new[] {new ObjectProperties("A-1")});

            var report = Client.EventReports.SummaryBySelectability(evnt.Key);

            Assert.Equal(231, report[Selectable].Count);
            Assert.Equal(new Dictionary<string, int> {{NoSection, 231}}, report[Selectable].bySection);
            Assert.Equal(new Dictionary<string, int> {{Free, 231}}, report[Selectable].byStatus);
            Assert.Equal(new Dictionary<string, int> {{"9", 115}, {"10", 116}}, report[Selectable].byCategoryKey);
            Assert.Equal(new Dictionary<string, int> {{NoChannel, 231}}, report[Selectable].byChannel);

            Assert.Equal(1, report[NotSelectable].Count);
            Assert.Equal(new Dictionary<string, int> {{NoSection, 1}}, report[NotSelectable].bySection);
            Assert.Equal(new Dictionary<string, int> {{Booked, 1}}, report[NotSelectable].byStatus);
            Assert.Equal(new Dictionary<string, int> {{"9", 1}}, report[NotSelectable].byCategoryKey);
            Assert.Equal(new Dictionary<string, int> {{NoChannel, 1}}, report[NotSelectable].byChannel);
        }    
        
        [Fact]
        public void SummaryByChannel()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            var channels = new Dictionary<string, Channel>()
            {
                {"channelKey1", new Channel("channel 1", "#FFFF00", 1)}
            };
            Client.Events.UpdateChannels(evnt.Key, channels);
            Client.Events.AssignObjectsToChannel(evnt.Key, new
            {
                channelKey1 = new[] {"A-1", "A-2"}
            });
            
            var report = Client.EventReports.SummaryByChannel(evnt.Key);

            Assert.Equal(230, report[NoChannel].Count);
            Assert.Equal(new Dictionary<string, int> {{NoSection, 230}}, report[NoChannel].bySection);
            Assert.Equal(new Dictionary<string, int> {{Free, 230}}, report[NoChannel].byStatus);
            Assert.Equal(new Dictionary<string, int> {{"9", 114}, {"10", 116}}, report[NoChannel].byCategoryKey);
            Assert.Equal(new Dictionary<string, int> {{Selectable, 230}}, report[NoChannel].bySelectability);

            Assert.Equal(2, report["channelKey1"].Count);
            Assert.Equal(new Dictionary<string, int> {{NoSection, 2}}, report["channelKey1"].bySection);
            Assert.Equal(new Dictionary<string, int> {{Free, 2}}, report["channelKey1"].byStatus);
            Assert.Equal(new Dictionary<string, int> {{"9", 2}}, report["channelKey1"].byCategoryKey);
            Assert.Equal(new Dictionary<string, int> {{Selectable, 2}}, report["channelKey1"].bySelectability);
        }
    }
}