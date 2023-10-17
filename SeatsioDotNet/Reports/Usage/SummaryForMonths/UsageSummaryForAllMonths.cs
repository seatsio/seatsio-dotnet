using System;
using System.Collections.Generic;

namespace SeatsioDotNet.Reports.Usage.SummaryForMonths;

public class UsageSummaryForAllMonths
{
    public IEnumerable<UsageSummaryForMonth> Usage { get; set; }
    public DateTimeOffset UsageCutoffDate { get; set; }
}