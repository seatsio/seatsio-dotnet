using Xunit;

namespace SeatsioDotNet.Test.Subaccounts
{
    public class RegenerateSubaccountDesignerKeyTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var subaccount = Client.Subaccounts.Create();
            
            Client.Subaccounts.RegenerateDesignerKey(subaccount.Id);

            var retrievedSubaccount = Client.Subaccounts.Retrieve(subaccount.Id);
            Assert.NotNull(retrievedSubaccount.DesignerKey);
            Assert.NotEqual(subaccount.DesignerKey, retrievedSubaccount.DesignerKey);
        }
    }
}