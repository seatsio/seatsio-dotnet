using Xunit;

namespace SeatsioDotNet.Test.Subaccounts
{
    public class RegenerateSecretKeyTest : SeatsioClientTest
    {
        [Fact]
        public void test()
        {
            var subaccount = client.Subaccounts().Create();
            
            client.Subaccounts().RegenerateSecretKey(subaccount.Id);

            var retrievedSubaccount = client.Subaccounts().Retrieve(subaccount.Id);
            Assert.NotNull(retrievedSubaccount.SecretKey);
            Assert.NotEqual(subaccount.SecretKey, retrievedSubaccount.SecretKey);
        }
    }
}