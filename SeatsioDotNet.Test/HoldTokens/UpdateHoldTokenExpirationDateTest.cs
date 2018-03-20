using System;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class UpdateHoldTokenExpirationDateTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var holdToken = Client.HoldTokens().Create();

            var updatedHoldToken = Client.HoldTokens().ExpiresInMinutes(holdToken.Token, 30);
            
            Assert.Equal(updatedHoldToken.Token, holdToken.Token);
            CustomAssert.CloseTo(DateTime.Now.AddMinutes(30), updatedHoldToken.ExpiresAt);
        }
    }
}