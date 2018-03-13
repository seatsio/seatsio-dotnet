using Xunit;

namespace SeatsioDotNet.Test
{
    public class DeactivateSubaccountTest : SeatsioClientTest
    {
        [Fact]
        public void test()
        {
            var subaccount = client.subaccounts().Create("joske");

            client.subaccounts().Deactivate(subaccount.Id);

            var retrievedSubaccount = client.subaccounts().Retrieve(subaccount.Id);
            Assert.False(retrievedSubaccount.Active);
        }
    }
}