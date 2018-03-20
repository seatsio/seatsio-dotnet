using System;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class RetrieveHoldTokenTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var holdToken = Client.HoldTokens().Create();
            
            var retrievedHoldToken = Client.HoldTokens().Retrieve(holdToken.Token);
            
            Assert.Equal(holdToken.Token, retrievedHoldToken.Token);
            Assert.Equal(holdToken.ExpiresAt, retrievedHoldToken.ExpiresAt);
        }
    }
}