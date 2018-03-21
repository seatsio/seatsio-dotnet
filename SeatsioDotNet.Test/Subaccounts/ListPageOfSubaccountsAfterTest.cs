using System.Linq;
using Xunit;

namespace SeatsioDotNet.Test.Subaccounts
{
    public class ListSubaccountsAfterTest : SeatsioClientTest
    {
        [Fact]
        public void WithPreviousPage()
        {
            var subaccount1 = Client.Subaccounts.Create();
            var subaccount2 = Client.Subaccounts.Create();
            var subaccount3 = Client.Subaccounts.Create();

            var page = Client.Subaccounts.ListPageAfter(subaccount3.Id);

            Assert.Equal(new[] {subaccount2.Id, subaccount1.Id}, page.Items.Select(s => s.Id));
            Assert.Null(page.NextPageStartsAfter);
            Assert.Equal(subaccount2.Id, page.PreviousPageEndsBefore.Value);
        }

        [Fact]
        public void WithNextAndPreviousPages()
        {
            var subaccount1 = Client.Subaccounts.Create();
            var subaccount2 = Client.Subaccounts.Create();
            var subaccount3 = Client.Subaccounts.Create();

            var page = Client.Subaccounts.ListPageAfter(subaccount3.Id, pageSize: 1);

            Assert.Equal(new[] {subaccount2.Id}, page.Items.Select(s => s.Id));
            Assert.Equal(subaccount2.Id, page.NextPageStartsAfter);
            Assert.Equal(subaccount2.Id, page.PreviousPageEndsBefore.Value);
        }
    }
}