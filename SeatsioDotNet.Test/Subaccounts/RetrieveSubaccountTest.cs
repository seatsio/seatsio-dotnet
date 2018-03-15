using Xunit;

namespace SeatsioDotNet.Test.Subaccounts
{
    public class RetrieveSubaccountTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var subaccount = Client.Subaccounts().Create("joske");

            var retrievedSubaccount = Client.Subaccounts().Retrieve(subaccount.Id);
            
            Assert.Equal(subaccount.Id, retrievedSubaccount.Id);
            Assert.Equal(subaccount.SecretKey, retrievedSubaccount.SecretKey);
            Assert.Equal(subaccount.DesignerKey, retrievedSubaccount.DesignerKey);
            Assert.Equal(subaccount.PublicKey, retrievedSubaccount.PublicKey);
            Assert.Equal("joske", retrievedSubaccount.Name);
            Assert.True(retrievedSubaccount.Active);
        }
    }
}