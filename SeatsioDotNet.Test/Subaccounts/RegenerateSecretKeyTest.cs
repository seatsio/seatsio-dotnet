using Xunit;

namespace SeatsioDotNet.Test.Subaccounts
{
    public class RegenerateSecretKeyTest : SeatsioClientTest
    {
        [Fact]
        public void test()
        {
            var subaccount = client.subaccounts().Create();
            
            client.subaccounts().RegenerateSecretKey(subaccount.Id);

            var retrievedSubaccount = client.subaccounts().Retrieve(subaccount.Id);
            Assert.NotNull(retrievedSubaccount.SecretKey);
            Assert.NotEqual(subaccount.SecretKey, retrievedSubaccount.SecretKey);
        }
    }
}