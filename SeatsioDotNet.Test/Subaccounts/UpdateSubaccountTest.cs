using Xunit;

namespace SeatsioDotNet.Test.Subaccounts
{
    public class UpdateSubaccountTest : SeatsioClientTest
    {
        [Fact]
        public void test()
        {
            var subaccount = client.Subaccounts().Create("joske");
            
            client.Subaccounts().Update(subaccount.Id, "jefke");

            var retrievedSubaccount = client.Subaccounts().Retrieve(subaccount.Id);
            Assert.Equal("jefke", retrievedSubaccount.Name);
        }
    }
}