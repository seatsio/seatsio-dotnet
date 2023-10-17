using System.Collections.Generic;

namespace SeatsioDotNet.Reports.Usage.DetailsForMonth;

public class UsageDetails
{
    public long Workspace { get; set; }
    public IEnumerable<UsageByChart> UsageByChart { get; set; }
}