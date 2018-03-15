using Xunit;

namespace SeatsioDotNet.Test.Subaccounts
{
    public class DeactivateSubaccountTest : SeatsioClientTest
    {
        [Fact]
        public void test()
        {
            var subaccount = client.Subaccounts().Create("joske");

            client.Subaccounts().Deactivate(subaccount.Id);

            var retrievedSubaccount = client.Subaccounts().Retrieve(subaccount.Id);
            Assert.False(retrievedSubaccount.Active);
        }
    }
}