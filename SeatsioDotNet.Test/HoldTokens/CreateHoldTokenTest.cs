using System;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class CreateHoldTokenTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var holdToken = Client.HoldTokens.Create();
            
            Assert.NotNull(holdToken.Token);
            CustomAssert.CloseTo(DateTime.Now.AddMinutes(15), holdToken.ExpiresAt);
        }   
        
        [Fact]
        public void ExpiresInMinutes()
        {
            var holdToken = Client.HoldTokens.Create(5);
            
            Assert.NotNull(holdToken.Token);
            CustomAssert.CloseTo(DateTime.Now.AddMinutes(5), holdToken.ExpiresAt);
        }
    }
}