using System;
using System.Collections.Generic;

namespace SeatsioDotNet.TicketBuyers;

public class AddTicketBuyerIdsResponse
{
    public IEnumerable<Guid> Added { get; set; }
    public IEnumerable<Guid> AlreadyPresent { get; set; }
}