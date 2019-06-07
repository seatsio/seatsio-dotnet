using System.Collections.Generic;

namespace SeatsioDotNet.UsageReports
{
    public class UsageDetails
    {
        public UsageSubaccount Subaccount { get; set; }
        public IEnumerable<UsageByChart> UsageByChart { get; set; }
    }
}