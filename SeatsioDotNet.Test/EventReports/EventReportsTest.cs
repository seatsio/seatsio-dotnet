using System.Collections.Generic;
using System.Linq;
using SeatsioDotNet.Events;
using Xunit;

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

            var report = Client.EventReports.ByLabel(evnt.Key);

            var reportItem = report["A-1"].First();
            Assert.Equal("A-1", reportItem.Label);
            Assert.Equal(ObjectStatus.Booked, reportItem.Status);
            Assert.Equal("Cat1", reportItem.CategoryLabel);
            Assert.Equal(9, reportItem.CategoryKey);
            Assert.Equal("ticketType1", reportItem.TicketType);
            Assert.Equal("order1", reportItem.OrderId);
            Assert.Equal("seat", reportItem.ObjectType);
            Assert.True(reportItem.ForSale);
            Assert.Null(reportItem.Section);
            Assert.Null(reportItem.Entrance);
            Assert.Null(reportItem.NumBooked);
            Assert.Null(reportItem.Capacity);
            Assert.Equal(extraData, reportItem.ExtraData);
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

            var report = Client.EventReports.ByLabel(evnt.Key);

            var reportItem = report["GA1"].First();
            Assert.Equal(5, reportItem.NumBooked);
            Assert.Equal(100, reportItem.Capacity);
            Assert.Equal("generalAdmission", reportItem.ObjectType);
        }

        [Fact]
        public void ByStatus()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.ChangeObjectStatus(evnt.Key, new[] {"A-1", "A-2"}, "lolzor");
            Client.Events.ChangeObjectStatus(evnt.Key, new[] {"A-3"}, ObjectStatus.Booked);

            var report = Client.EventReports.ByStatus(evnt.Key);

            Assert.Equal(2, report["lolzor"].Count());
            Assert.Single(report[ObjectStatus.Booked]);
            Assert.Equal(31, report[ObjectStatus.Free].Count());
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

            Assert.Equal(34, report["NO_SECTION"].Count());
        }

        [Fact]
        public void BySpecificSection()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            var report = Client.EventReports.BySection(evnt.Key, "NO_SECTION");

            Assert.Equal(34, report.Count());
        }
    }
}