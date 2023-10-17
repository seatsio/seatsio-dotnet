using System;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class CreateHoldTokenTest : SeatsioClientTest
{
    [Fact]
    public void Test()
    {
        var holdToken = Client.HoldTokens.Create();
            
        Assert.NotNull(holdToken.Token);
        CustomAssert.CloseTo(DateTimeOffset.Now.AddMinutes(15), holdToken.ExpiresAt);
        Assert.InRange(holdToken.ExpiresInSeconds, 14 * 60, 15 * 60);
    }   
        
    [Fact]
    public void ExpiresInMinutes()
    {
        var holdToken = Client.HoldTokens.Create(5);
            
        Assert.NotNull(holdToken.Token);
        CustomAssert.CloseTo(DateTimeOffset.Now.AddMinutes(5), holdToken.ExpiresAt);
        Assert.InRange(holdToken.ExpiresInSeconds, 4 * 60, 5 * 60);
    }
}