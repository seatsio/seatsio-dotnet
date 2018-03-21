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
    }
}