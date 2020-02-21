using Xunit;

namespace SeatsioDotNet.Test
{
    public class WorkspaceKeyAuthenticationTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var subaccount = Client.Subaccounts.Create();

            var subaccountClient = CreateSeatsioClient(User.SecretKey, subaccount.PublicKey);
            var holdToken = subaccountClient.HoldTokens.Create();

            Assert.Equal(subaccount.PublicKey, holdToken.workspaceKey);
        }
    }
}