using System.Linq;
using Xunit;

namespace SeatsioDotNet.Test.Subaccounts
{
    public class ListAllSubaccountsTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var subaccount1 = Client.Subaccounts.Create();
            var subaccount2 = Client.Subaccounts.Create();
            var subaccount3 = Client.Subaccounts.Create();

            var subaccounts = Client.Subaccounts.ListAll();

            Assert.Equal(new[] {subaccount3.Id, subaccount2.Id, subaccount1.Id}, subaccounts.Select(s => s.Id));
        }
        
        [Fact]
        public void TestFiltered()
        {
            var subaccount1 = Client.Subaccounts.Create("test-/@/11");
            var subaccount2 = Client.Subaccounts.Create("test-/@/12");
            var subaccount3 = Client.Subaccounts.Create("test-/@/3");

            var subaccounts = Client.Subaccounts.ListAll("test-/@/1");

            Assert.Equal(new[] {subaccount2.Id, subaccount1.Id}, subaccounts.Select(s => s.Id));
        }
        
        [Fact]
        public void FilterAllOnFirstPage()
        {
            var subaccount1 = Client.Subaccounts.Create("test-/@/11");
            var subaccount2 = Client.Subaccounts.Create("test-/@/12");
            var subaccount3 = Client.Subaccounts.Create("test-/@/13");
            var subaccount4 = Client.Subaccounts.Create("test-/@/4");
            var subaccount5 = Client.Subaccounts.Create("test-/@/5");
            var subaccount6 = Client.Subaccounts.Create("test-/@/6");

            var page = Client.Subaccounts.ListFirstPage("test-/@/1", 10);

            Assert.Equal(new[] {subaccount3.Id, subaccount2.Id, subaccount1.Id}, page.Items.Select(s => s.Id));
            Assert.Null(page.NextPageStartsAfter);
            Assert.Null(page.PreviousPageEndsBefore);
        }
        
        [Fact]
        public void FilterWithPreviousPage()
        {
            var subaccount1 = Client.Subaccounts.Create("test-/@/11");
            var subaccount2 = Client.Subaccounts.Create("test-/@/12");
            var subaccount3 = Client.Subaccounts.Create("test-/@/13");
            var subaccount4 = Client.Subaccounts.Create("test-/@/4");
            var subaccount5 = Client.Subaccounts.Create("test-/@/5");
            var subaccount6 = Client.Subaccounts.Create("test-/@/6");
            
            var page = Client.Subaccounts.ListPageAfter(subaccount5.Id, "test-/@/1");

            Assert.Equal(new[] {subaccount3.Id, subaccount2.Id, subaccount1.Id}, page.Items.Select(s => s.Id));
            Assert.Null(page.NextPageStartsAfter);
            Assert.Equal(subaccount3.Id, page.PreviousPageEndsBefore.Value);
        }
        
        [Fact]
        public void FilterWithNextPage()
        {
            var subaccount1 = Client.Subaccounts.Create("test-/@/11");
            var subaccount2 = Client.Subaccounts.Create("test-/@/12");
            var subaccount3 = Client.Subaccounts.Create("test-/@/13");
            var subaccount4 = Client.Subaccounts.Create("test-/@/21");
            var subaccount5 = Client.Subaccounts.Create("test-/@/22");
            var subaccount6 = Client.Subaccounts.Create("test-/@/3");

            var page = Client.Subaccounts.ListPageBefore(subaccount3.Id, "test-/@/2");

            Assert.Equal(new[] {subaccount5.Id, subaccount4.Id}, page.Items.Select(s => s.Id));
            Assert.Equal(subaccount4.Id, page.NextPageStartsAfter);
            Assert.Null(page.PreviousPageEndsBefore);
        }
    }
}