using Xunit;

namespace SeatsioDotNet.Test.Subaccounts
{
    public class UpdateSubaccountTest : SeatsioClientTest
    {
        [Fact]
        public void test()
        {
            var subaccount = client.subaccounts().Create("joske");
            
            client.subaccounts().Update(subaccount.Id, "jefke");

            var retrievedSubaccount = client.subaccounts().Retrieve(subaccount.Id);
            Assert.Equal("jefke", retrievedSubaccount.Name);
        }
    }
}