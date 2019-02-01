using System;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class UpdateHoldTokenExpirationDateTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var holdToken = Client.HoldTokens.Create();

            var updatedHoldToken = Client.HoldTokens.ExpiresInMinutes(holdToken.Token, 30);
            
            Assert.Equal(updatedHoldToken.Token, holdToken.Token);
            CustomAssert.CloseTo(DateTimeOffset.Now.AddMinutes(30), updatedHoldToken.ExpiresAt);
            Assert.InRange(updatedHoldToken.ExpiresInSeconds, 29 * 60, 30 * 60);
        }
    }
}