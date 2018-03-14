using Xunit;

namespace SeatsioDotNet.Test.Subaccounts
{
    public class RegenerateDesignerKeyTest : SeatsioClientTest
    {
        [Fact]
        public void test()
        {
            var subaccount = client.subaccounts().Create();
            
            client.subaccounts().RegenerateDesignerKey(subaccount.Id);

            var retrievedSubaccount = client.subaccounts().Retrieve(subaccount.Id);
            Assert.NotNull(retrievedSubaccount.DesignerKey);
            Assert.NotEqual(subaccount.DesignerKey, retrievedSubaccount.DesignerKey);
        }
    }
}