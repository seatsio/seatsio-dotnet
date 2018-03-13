using Xunit;

namespace SeatsioDotNet.Test.Subaccounts
{
    public class ActivateSubaccountTest : SeatsioClientTest
    {
        [Fact]
        public void test()
        {
            var subaccount = client.subaccounts().Create("joske");
            client.subaccounts().Deactivate(subaccount.Id);

            client.subaccounts().Activate(subaccount.Id);

            var retrievedSubaccount = client.subaccounts().Retrieve(subaccount.Id);
            Assert.True(retrievedSubaccount.Active);
        }
    }
}