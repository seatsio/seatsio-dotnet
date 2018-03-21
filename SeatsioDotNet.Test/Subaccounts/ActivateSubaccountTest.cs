using Xunit;

namespace SeatsioDotNet.Test.Subaccounts
{
    public class ActivateSubaccountTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var subaccount = Client.Subaccounts.Create("joske");
            Client.Subaccounts.Deactivate(subaccount.Id);

            Client.Subaccounts.Activate(subaccount.Id);

            var retrievedSubaccount = Client.Subaccounts.Retrieve(subaccount.Id);
            Assert.True(retrievedSubaccount.Active);
        }
    }
}