using Xunit;

namespace SeatsioDotNet.Test.Subaccounts
{
    public class RegenerateDesignerKeyTest : SeatsioClientTest
    {
        [Fact]
        public void test()
        {
            var subaccount = client.Subaccounts().Create();
            
            client.Subaccounts().RegenerateDesignerKey(subaccount.Id);

            var retrievedSubaccount = client.Subaccounts().Retrieve(subaccount.Id);
            Assert.NotNull(retrievedSubaccount.DesignerKey);
            Assert.NotEqual(subaccount.DesignerKey, retrievedSubaccount.DesignerKey);
        }
    }
}