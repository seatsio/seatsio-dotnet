using System;
using SeatsioDotNet.UsageReports;
using Xunit;

namespace SeatsioDotNet.Test.UsageReports
{
    public class UsageReportsTest : SeatsioClientTest
    {
        [Fact]
        public void SummaryForAllMonths()
        {
            var report = Client.UsageReports.SummaryForAllMonths();
        }  
        
        [Fact]
        public void DetailsForMonth()
        {
            var report = Client.UsageReports.DetailsForMonth(new UsageMonth(2019, 5));
        }
        
        [Fact]
        public void DetailsForEventInMonth()
        {
            var chart = Client.Charts.Create();
            var anEvent = Client.Events.Create(chart.Key);
            var report = Client.UsageReports.DetailsForEventInMonth(anEvent.Id, new UsageMonth(2019, 5));
        }
    }
}