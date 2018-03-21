using Xunit;

namespace SeatsioDotNet.Test.Subaccounts
{
    public class DeactivateSubaccountTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var subaccount = Client.Subaccounts.Create("joske");

            Client.Subaccounts.Deactivate(subaccount.Id);

            var retrievedSubaccount = Client.Subaccounts.Retrieve(subaccount.Id);
            Assert.False(retrievedSubaccount.Active);
        }
    }
}