using System.Linq;
using Xunit;

namespace SeatsioDotNet.Test.Subaccounts
{
    public class ListActiveSubaccountsTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var subaccount1 = Client.Subaccounts.Create();
            var subaccount2 = Client.Subaccounts.Create();
            Client.Subaccounts.Deactivate(subaccount2.Id);
            var subaccount3 = Client.Subaccounts.Create();

            var subaccounts = Client.Subaccounts.Active.All();

            Assert.Equal(new[] {subaccount3.Id, subaccount1.Id, Subaccount.Id}, subaccounts.Select(s => s.Id));
        }
    }
}