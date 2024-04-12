using System;
using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class CreateHoldTokenTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var holdToken = await Client.HoldTokens.CreateAsync();

        Assert.NotNull(holdToken.Token);
        CustomAssert.CloseTo(DateTimeOffset.Now.AddMinutes(15), holdToken.ExpiresAt);
        Assert.InRange(holdToken.ExpiresInSeconds, 14 * 60, 15 * 60);
    }

    [Fact]
    public async Task ExpiresInMinutes()
    {
        var holdToken = await Client.HoldTokens.CreateAsync(5);

        Assert.NotNull(holdToken.Token);
        CustomAssert.CloseTo(DateTimeOffset.Now.AddMinutes(5), holdToken.ExpiresAt);
        Assert.InRange(holdToken.ExpiresInSeconds, 4 * 60, 5 * 60);
    }
}