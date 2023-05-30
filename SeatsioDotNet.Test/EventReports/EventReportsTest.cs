using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using SeatsioDotNet.Events;
using Xunit;
using static SeatsioDotNet.EventReports.EventObjectInfo;

namespace SeatsioDotNet.Test.EventReports
{
    public class EventReportsTest : SeatsioClientTest
    {
        [Fact]
        public void ReportItemProperties()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            var extraData = new Dictionary<string, object> {{"foo", "bar"}};
            Client.Events.Book(evnt.Key, new[] {new ObjectProperties("A-1", "ticketType1", extraData)}, null, "order1");

            Client.Events.Channels.Replace(evnt.Key, new List<Channel>
            {
                new("channelKey1", "channel 1", "#FFFF00", 1, new[] {"A-1"})
            });

            var report = Client.EventReports.ByLabel(evnt.Key);

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
            Assert.False(reportItem.IsDisabledBySocialDistancing);
            Assert.Equal("channelKey1", reportItem.Channel);
            Assert.Null(reportItem.BookAsAWhole);
            Assert.NotNull(reportItem.DistanceToFocalPoint);
        }

        [Fact]
        public void HoldToken()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            var holdToken = Client.HoldTokens.Create();
            Client.Events.Hold(evnt.Key, new[] {"A-1"}, holdToken.Token);

            var report = Client.EventReports.ByLabel(evnt.Key);

            var reportItem = report["A-1"].First();
            Assert.Equal(holdToken.Token, reportItem.HoldToken);
        }

        [Fact]
        public void ReportItemPropertiesForGA()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.Book(evnt.Key, new[] {new ObjectProperties("GA1", 5)});
            var holdToken = Client.HoldTokens.Create();
            Client.Events.Hold(evnt.Key, new[] {new ObjectProperties("GA1", 3)}, holdToken.Token);

            var report = Client.EventReports.ByLabel(evnt.Key);

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
        public void ReportItemPropertiesForTable()
        {
            var chartKey = CreateTestChartWithTables();
            var evnt = Client.Events.Create(chartKey, null, TableBookingConfig.AllByTable());

            var report = Client.EventReports.ByLabel(evnt.Key);

            var reportItem = report["T1"].First();
            Assert.Equal(6, reportItem.NumSeats);
            Assert.False(reportItem.BookAsAWhole);
        }

        [Fact]
        public void ByStatus()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.ChangeObjectStatus(evnt.Key, new[] {"A-1", "A-2"}, "lolzor");
            Client.Events.ChangeObjectStatus(evnt.Key, new[] {"A-3"}, Booked);

            var report = Client.EventReports.ByStatus(evnt.Key);

            Assert.Equal(2, report["lolzor"].Count());
            Assert.Single(report[Booked]);
            Assert.Equal(31, report[Free].Count());
        }

        [Fact]
        public void ByStatusEmptyChart()
        {
            var chartKey = Client.Charts.Create().Key;
            var evnt = Client.Events.Create(chartKey);

            var report = Client.EventReports.ByStatus(evnt.Key);

            Assert.Empty(report);
        }

        [Fact]
        public void BySpecificStatus()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.ChangeObjectStatus(evnt.Key, new[] {"A-1", "A-2"}, "lolzor");

            var report = Client.EventReports.ByStatus(evnt.Key, "lolzor");

            Assert.Equal(2, report.Count());
        }

        [Fact]
        public void BySpecificNonExistingStatus()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            var report = Client.EventReports.ByStatus(evnt.Key, "lolzor");

            Assert.Empty(report);
        }
        
        [Fact]
        public void ByObjectType()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            var report = Client.EventReports.ByObjectType(evnt.Key);

            Assert.Equal(32, report["seat"].Count());
            Assert.Equal(2, report["generalAdmission"].Count());
            Assert.Equal(0, report["booth"].Count());
            Assert.Equal(0, report["table"].Count());
        }

        [Fact]
        public void BySpecificObjectType()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            var report = Client.EventReports.ByObjectType(evnt.Key, "seat");

            Assert.Equal(32, report.Count());
        }

        [Fact]
        public void ByCategoryLabel()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            var report = Client.EventReports.ByCategoryLabel(evnt.Key);

            Assert.Equal(17, report["Cat1"].Count());
            Assert.Equal(17, report["Cat2"].Count());
        }

        [Fact]
        public void BySpecificCategoryLabel()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            var report = Client.EventReports.ByCategoryLabel(evnt.Key, "Cat1");

            Assert.Equal(17, report.Count());
        }

        [Fact]
        public void ByCategoryKey()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            var report = Client.EventReports.ByCategoryKey(evnt.Key);

            Assert.Equal(17, report["9"].Count());
            Assert.Equal(17, report["10"].Count());
        }

        [Fact]
        public void BySpecificCategoryKey()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            var report = Client.EventReports.ByCategoryKey(evnt.Key, "9");

            Assert.Equal(17, report.Count());
        }

        [Fact]
        public void ByLabel()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            var report = Client.EventReports.ByLabel(evnt.Key);

            Assert.Single(report["A-1"]);
            Assert.Single(report["A-2"]);
        }

        [Fact]
        public void BySpecificLabel()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            var report = Client.EventReports.ByLabel(evnt.Key, "A-1");

            Assert.Single(report);
        }

        [Fact]
        public void ByOrderId()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.Book(evnt.Key, new[] {"A-1", "A-2"}, null, "order1");
            Client.Events.Book(evnt.Key, new[] {"A-3"}, null, "order2");

            var report = Client.EventReports.ByOrderId(evnt.Key);

            Assert.Equal(2, report["order1"].Count());
            Assert.Single(report["order2"]);
            Assert.Equal(31, report["NO_ORDER_ID"].Count());
        }

        [Fact]
        public void BySpecificOrderId()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.Book(evnt.Key, new[] {"A-1", "A-2"}, null, "order1");

            var report = Client.EventReports.ByOrderId(evnt.Key, "order1");

            Assert.Equal(2, report.Count());
        }

        [Fact]
        public void BySection()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            var report = Client.EventReports.BySection(evnt.Key);

            Assert.Equal(34, report[NoSection].Count());
        }

        [Fact]
        public void BySpecificSection()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            var report = Client.EventReports.BySection(evnt.Key, NoSection);

            Assert.Equal(34, report.Count());
        }

        [Fact]
        public void ByAvailability()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.Book(evnt.Key, new[] {"A-1", "A-2"});

            var report = Client.EventReports.ByAvailability(evnt.Key);

            Assert.Equal(32, report[Available].Count());
            Assert.Equal(2, report[NotAvailable].Count());
        }

        [Fact]
        public void BySpecificAvailability()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.Book(evnt.Key, new[] {"A-1", "A-2"});

            var report = Client.EventReports.ByAvailability(evnt.Key, NotAvailable);

            Assert.Equal(2, report.Count());
        }
        
        [Fact]
        public void ByAvailabilityReason()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.Book(evnt.Key, new[] {"A-1", "A-2"});

            var report = Client.EventReports.ByAvailabilityReason(evnt.Key);

            Assert.Equal(32, report[Available].Count());
            Assert.Equal(2, report[Booked].Count());
        }

        [Fact]
        public void BySpecificAvailabilityReason()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.Book(evnt.Key, new[] {"A-1", "A-2"});

            var report = Client.EventReports.ByAvailabilityReason(evnt.Key, Booked);

            Assert.Equal(2, report.Count());
        }

        [Fact]
        public void ByChannel()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.Channels.Replace(evnt.Key, new List<Channel>
            {
                new("channelKey1", "channel 1", "#FFFF00", 1, new[] {"A-1", "A-2"})
            });

            var report = Client.EventReports.ByChannel(evnt.Key);

            Assert.Equal(32, report[NoChannel].Count());
            Assert.Equal(2, report["channelKey1"].Count());
        }

        [Fact]
        public void BySpecificChannel()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.Channels.Replace(evnt.Key, new List<Channel>
            {
                new("channelKey1", "channel 1", "#FFFF00", 1, new[] {"A-1", "A-2"})
            });

            var report = Client.EventReports.ByChannel(evnt.Key, "channelKey1");

            Assert.Equal(2, report.Count());
        }
    }
}
