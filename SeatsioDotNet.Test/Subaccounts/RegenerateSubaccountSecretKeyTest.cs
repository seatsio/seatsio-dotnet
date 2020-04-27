using Xunit;

namespace SeatsioDotNet.Test.Subaccounts
{
    public class RegenerateSubaccountSecretKeyTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var subaccount = Client.Subaccounts.Create();
            
            Client.Subaccounts.RegenerateSecretKey(subaccount.Id);

            var retrievedSubaccount = Client.Subaccounts.Retrieve(subaccount.Id);
            Assert.NotNull(retrievedSubaccount.SecretKey);
            Assert.NotEqual(subaccount.SecretKey, retrievedSubaccount.SecretKey);
        }
    }
}