using Xunit;

namespace SeatsioDotNet.Test.Subaccounts
{
    public class UpdateSubaccountTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var subaccount = Client.Subaccounts().Create("joske");
            
            Client.Subaccounts().Update(subaccount.Id, "jefke");

            var retrievedSubaccount = Client.Subaccounts().Retrieve(subaccount.Id);
            Assert.Equal("jefke", retrievedSubaccount.Name);
        }
    }
}