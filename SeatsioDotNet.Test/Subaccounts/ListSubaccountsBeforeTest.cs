using System.Linq;
using SeatsioDotNet.Util;
using Xunit;

namespace SeatsioDotNet.Test.Subaccounts
{
    public class ListSubaccountsBeforeTest : SeatsioClientTest
    {
        [Fact]
        public void WithNextPage()
        {
            var subaccount1 = Client.Subaccounts().Create();
            var subaccount2 = Client.Subaccounts().Create();
            var subaccount3 = Client.Subaccounts().Create();

            var page = Client.Subaccounts().List().PageBefore(subaccount1.Id);

            Assert.Equal(new[] {subaccount3.Id, subaccount2.Id}, page.Items.Select(s => s.Id));
            Assert.Equal(subaccount2.Id, page.NextPageStartsAfter);
            Assert.Null(page.PreviousPageEndsBefore);
        }

        [Fact]
        public void WithNextAndPreviousPages()
        {
            var subaccount1 = Client.Subaccounts().Create();
            var subaccount2 = Client.Subaccounts().Create();
            var subaccount3 = Client.Subaccounts().Create();

            var page = Client.Subaccounts().List().PageBefore(subaccount1.Id, new ListParams().SetPageSize(1));

            Assert.Equal(new[] {subaccount2.Id}, page.Items.Select(s => s.Id));
            Assert.Equal(subaccount2.Id, page.NextPageStartsAfter);
            Assert.Equal(subaccount2.Id, page.PreviousPageEndsBefore.Value);
        }
    }
}