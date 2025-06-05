using System;
using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.TicketBuyers;

public class AddTicketBuyerIdsTest : SeatsioClientTest
{
    [Fact]
    public async Task TestCanAddSingleTicketBuyerId()
    {
        var ticketBuyerId = Guid.NewGuid();
        var response = await Client.TicketBuyers.AddAsync(ticketBuyerId);

        Assert.Equal(new[] {ticketBuyerId}, response.Added);
        Assert.Empty(response.AlreadyPresent);
    }

    [Fact]
    public async Task TestCanAddTicketBuyerIds()
    {
        var ticketBuyerId1 = Guid.NewGuid();
        var ticketBuyerId2 = Guid.NewGuid();
        var ticketBuyerId3 = Guid.NewGuid();
        var response = await Client.TicketBuyers.AddAsync(new[] {ticketBuyerId1, ticketBuyerId2, ticketBuyerId3});

        Assert.Equivalent(new[] {ticketBuyerId1, ticketBuyerId2, ticketBuyerId3}, response.Added);
        Assert.Empty(response.AlreadyPresent);
    }

    [Fact]
    public async Task TestCanAddTicketBuyerIds_WithDuplicates()
    {
        var ticketBuyerId1 = Guid.NewGuid();
        var ticketBuyerId2 = Guid.NewGuid();
        var response = await Client.TicketBuyers.AddAsync(new[] {ticketBuyerId1, ticketBuyerId1, ticketBuyerId1, ticketBuyerId2, ticketBuyerId2});

        Assert.Equivalent(new[] {ticketBuyerId1, ticketBuyerId2}, response.Added);
        Assert.Empty(response.AlreadyPresent);
    }

    [Fact]
    public async Task TestSameIdDoesNotGetAddedTwice()
    {
        var ticketBuyerId1 = Guid.NewGuid();
        var ticketBuyerId2 = Guid.NewGuid();
        var response = await Client.TicketBuyers.AddAsync(new[] {ticketBuyerId1, ticketBuyerId2});

        Assert.Equivalent(new[] {ticketBuyerId1, ticketBuyerId2}, response.Added);
        Assert.Empty(response.AlreadyPresent);

        var secondResponse = await Client.TicketBuyers.AddAsync(ticketBuyerId1);

        Assert.Equal(new[] {ticketBuyerId1}, secondResponse.AlreadyPresent);
        Assert.Empty(secondResponse.Added);
    }

    [Fact]
    public async Task TestEmptyTicketIdDoesNotGetAdded()
    {
        var ticketBuyerId1 = Guid.NewGuid();
        var response = await Client.TicketBuyers.AddAsync(new[] {ticketBuyerId1, Guid.Empty});

        Assert.Equal(new[] {ticketBuyerId1}, response.Added);
        Assert.Empty(response.AlreadyPresent);
    }
}