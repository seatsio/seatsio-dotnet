using Xunit;

namespace SeatsioDotNet.Test
{
    public class WorkspaceKeyAuthenticationTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var subaccount = Client.Subaccounts.Create();

            var subaccountClient = CreateSeatsioClient(User.SecretKey, subaccount.workspace.Key);
            var holdToken = subaccountClient.HoldTokens.Create();

            Assert.Equal(subaccount.workspace.Key, holdToken.workspaceKey);
        }
    }
}