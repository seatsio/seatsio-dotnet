using Xunit;

namespace SeatsioDotNet.Test
{
    public class AccountIdAuthenticationTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var subaccount = Client.Subaccounts.Create();

            var subaccountClient = CreateSeatsioClient(User.SecretKey, subaccount.AccountId);
            var holdToken = subaccountClient.HoldTokens.Create();

            Assert.Equal(subaccount.AccountId, holdToken.AccountId);
        }
    }
}