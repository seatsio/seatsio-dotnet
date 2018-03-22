using System.Linq;
using Xunit;

namespace SeatsioDotNet.Test.Subaccounts
{
    public class ListInactiveSubaccountsTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var subaccount1 = Client.Subaccounts.Create();
            Client.Subaccounts.Deactivate(subaccount1.Id);
            var subaccount2 = Client.Subaccounts.Create();
            var subaccount3 = Client.Subaccounts.Create();
            Client.Subaccounts.Deactivate(subaccount3.Id);

            var subaccounts = Client.Subaccounts.Inactive.All();

            Assert.Equal(new[] {subaccount3.Id, subaccount1.Id}, subaccounts.Select(s => s.Id));
        }
    }
}