using System.Collections.Generic;

namespace SeatsioDotNet.UsageReports
{
    public class UsageDetails
    {
        public UsageWorkspace Workspace { get; set; }
        public IEnumerable<UsageByChart> UsageByChart { get; set; }
    }
}