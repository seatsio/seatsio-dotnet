using System;
using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.TicketBuyers;

public class RemoveTicketBuyerIdsTest : SeatsioClientTest
{
    [Fact]
    public async Task TestCanRemoveSingleTicketBuyerId()
    {
        var ticketBuyerId1 = Guid.NewGuid();
        var ticketBuyerId2 = Guid.NewGuid();
        var ticketBuyerId3 = Guid.NewGuid();
        await Client.TicketBuyers.AddAsync(new[] {ticketBuyerId1, ticketBuyerId2, ticketBuyerId3});

        var response = await Client.TicketBuyers.RemoveAsync(ticketBuyerId2);
        
        Assert.Equal(new[] {ticketBuyerId2}, response.Removed);
        Assert.Empty(response.NotPresent);
    }

    [Fact]
    public async Task TestCanRemoveTicketBuyerIds()
    {
        var ticketBuyerId1 = Guid.NewGuid();
        var ticketBuyerId2 = Guid.NewGuid();
        var ticketBuyerId3 = Guid.NewGuid();
        await Client.TicketBuyers.AddAsync(new[] {ticketBuyerId1, ticketBuyerId2, ticketBuyerId3});

        var response = await Client.TicketBuyers.RemoveAsync(new[] {ticketBuyerId1, ticketBuyerId2, ticketBuyerId3});
        
        Assert.Equivalent(new[] {ticketBuyerId1, ticketBuyerId2, ticketBuyerId3}, response.Removed);
        Assert.Empty(response.NotPresent);
    }

    [Fact]
    public async Task TestEmptyTicketBuyerIdDoesNotGetRemoved()
    {
        var ticketBuyerId1 = Guid.NewGuid();
        var ticketBuyerId2 = Guid.NewGuid();
        var ticketBuyerId3 = Guid.NewGuid();
        await Client.TicketBuyers.AddAsync(new[] {ticketBuyerId1, ticketBuyerId2, ticketBuyerId3});

        var response = await Client.TicketBuyers.RemoveAsync(new[] {ticketBuyerId1, Guid.Empty});
        
        Assert.Equal(new[] {ticketBuyerId1}, response.Removed);
        Assert.Empty(response.NotPresent);
    }
}