using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.TicketBuyers;

public class ListTicketBuyerIdsTest : SeatsioClientTest
{
    [Fact]
    public async Task TestCanAddTicketBuyerIds()
    {
        var ticketBuyerId1 = Guid.NewGuid();
        var ticketBuyerId2 = Guid.NewGuid();
        var ticketBuyerId3 = Guid.NewGuid();
        await Client.TicketBuyers.AddAsync(new[] {ticketBuyerId1, ticketBuyerId2, ticketBuyerId3});

        var response = await Client.TicketBuyers.ListAllAsync().ToListAsync();
        
        Assert.Equivalent(new[] {ticketBuyerId1, ticketBuyerId2, ticketBuyerId3}, response);
    }
}