using System.Linq;
using Xunit;

namespace SeatsioDotNet.Test.Subaccounts
{
    public class ListFirstPageOfSubaccountsTest : SeatsioClientTest
    {
        [Fact]
        public void AllOnFirstPage()
        {
            var subaccount1 = Client.Subaccounts().Create();
            var subaccount2 = Client.Subaccounts().Create();
            var subaccount3 = Client.Subaccounts().Create();

            var page = Client.Subaccounts().List().FirstPage();

            Assert.Equal(new[] {subaccount3.Id, subaccount2.Id, subaccount1.Id}, page.Items.Select(s => s.Id));
            Assert.Null(page.NextPageStartsAfter);
            Assert.Null(page.PreviousPageEndsBefore);
        }  
        
        [Fact]
        public void SomeOnFirstPage()
        {
            var subaccount1 = Client.Subaccounts().Create();
            var subaccount2 = Client.Subaccounts().Create();
            var subaccount3 = Client.Subaccounts().Create();

            var page = Client.Subaccounts().List().FirstPage(new Util.ListParams().SetPageSize(2));

            Assert.Equal(new[] {subaccount3.Id, subaccount2.Id}, page.Items.Select(s => s.Id));
            Assert.Equal(subaccount2.Id, page.NextPageStartsAfter.Value);
            Assert.Null(page.PreviousPageEndsBefore);
        }
    }
}