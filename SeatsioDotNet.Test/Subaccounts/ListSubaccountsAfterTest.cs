using System.Linq;
using SeatsioDotNet.Util;
using Xunit;

namespace SeatsioDotNet.Test.Subaccounts
{
    public class ListSubaccountsAfterTest : SeatsioClientTest
    {
        [Fact]
        public void WithPreviousPage()
        {
            var subaccount1 = client.Subaccounts().Create();
            var subaccount2 = client.Subaccounts().Create();
            var subaccount3 = client.Subaccounts().Create();

            var page = client.Subaccounts().List().PageAfter(subaccount3.Id);

            Assert.Equal(new[] {subaccount2.Id, subaccount1.Id}, page.Items.Select(s => s.Id));
            Assert.Null(page.NextPageStartsAfter);
            Assert.Equal(subaccount2.Id, page.PreviousPageEndsBefore.Value);
        }

        [Fact]
        public void WithNextAndPreviousPages()
        {
            var subaccount1 = client.Subaccounts().Create();
            var subaccount2 = client.Subaccounts().Create();
            var subaccount3 = client.Subaccounts().Create();

            var page = client.Subaccounts().List().PageAfter(subaccount3.Id, new ListParams().SetPageSize(1));

            Assert.Equal(new[] {subaccount2.Id}, page.Items.Select(s => s.Id));
            Assert.Equal(subaccount2.Id, page.NextPageStartsAfter);
            Assert.Equal(subaccount2.Id, page.PreviousPageEndsBefore.Value);
        }
    }
}