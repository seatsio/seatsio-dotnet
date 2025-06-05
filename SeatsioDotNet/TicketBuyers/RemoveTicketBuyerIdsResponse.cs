using System;
using System.Collections.Generic;

namespace SeatsioDotNet.TicketBuyers;

public class RemoveTicketBuyerIdsResponse
{
    public IEnumerable<Guid> Removed { get; set; }
    public IEnumerable<Guid> NotPresent { get; set; }
}