using Xunit;

namespace SeatsioDotNet.Test.Subaccounts
{
    public class ActivateSubaccountTest : SeatsioClientTest
    {
        [Fact]
        public void test()
        {
            var subaccount = client.Subaccounts().Create("joske");
            client.Subaccounts().Deactivate(subaccount.Id);

            client.Subaccounts().Activate(subaccount.Id);

            var retrievedSubaccount = client.Subaccounts().Retrieve(subaccount.Id);
            Assert.True(retrievedSubaccount.Active);
        }
    }
}