using System;
using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class UpdateHoldTokenExpirationDateTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var holdToken = await Client.HoldTokens.CreateAsync();

        var updatedHoldToken = await Client.HoldTokens.ExpiresInMinutesAsync(holdToken.Token, 30);
            
        Assert.Equal(updatedHoldToken.Token, holdToken.Token);
        CustomAssert.CloseTo(DateTimeOffset.Now.AddMinutes(30), updatedHoldToken.ExpiresAt);
        Assert.InRange(updatedHoldToken.ExpiresInSeconds, 29 * 60, 30 * 60);
    }
}