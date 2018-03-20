using System.Linq;
using SeatsioDotNet.Util;
using Xunit;

namespace SeatsioDotNet.Test.Subaccounts
{
    public class ListAllSubaccountsTest : SeatsioClientTest
    {
        [Fact]
        public void OnePage()
        {
            var subaccount1 = Client.Subaccounts().Create();
            var subaccount2 = Client.Subaccounts().Create();
            var subaccount3 = Client.Subaccounts().Create();

            var subaccounts = Client.Subaccounts().List().All();

            Assert.Equal(new[] {subaccount3.Id, subaccount2.Id, subaccount1.Id}, subaccounts.Select(s => s.Id));
        }  
        
        [Fact]
        public void MultiplePages()
        {
            var subaccount1 = Client.Subaccounts().Create();
            var subaccount2 = Client.Subaccounts().Create();
            var subaccount3 = Client.Subaccounts().Create();

            var subaccounts = Client.Subaccounts().List().All(new ListParams().SetPageSize(1));

            Assert.Equal(new[] {subaccount3.Id, subaccount2.Id, subaccount1.Id}, subaccounts.Select(s => s.Id));
        }
    }
}