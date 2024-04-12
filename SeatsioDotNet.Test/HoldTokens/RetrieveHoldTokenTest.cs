using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class RetrieveHoldTokenTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var holdToken = await Client.HoldTokens.CreateAsync();
            
        var retrievedHoldToken = await Client.HoldTokens.RetrieveAsync(holdToken.Token);
            
        Assert.Equal(holdToken.Token, retrievedHoldToken.Token);
        Assert.Equal(holdToken.ExpiresAt, retrievedHoldToken.ExpiresAt);
        Assert.InRange(holdToken.ExpiresInSeconds, 14 * 60, 15 * 60);
        Assert.NotNull(holdToken.workspaceKey);
    }
}