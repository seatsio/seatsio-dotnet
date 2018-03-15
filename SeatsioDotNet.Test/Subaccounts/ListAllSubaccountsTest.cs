using System.Linq;
using Xunit;

namespace SeatsioDotNet.Test.Subaccounts
{
    public class ListAllSubaccountsTest : SeatsioClientTest
    {
        [Fact]
        public void OnePage()
        {
            var subaccount1 = client.Subaccounts().Create();
            var subaccount2 = client.Subaccounts().Create();
            var subaccount3 = client.Subaccounts().Create();

            var subaccounts = client.Subaccounts().List().All();

            Assert.Equal(new[] {subaccount3.Id, subaccount2.Id, subaccount1.Id}, subaccounts.Select(s => s.Id));
        }  
        
        [Fact]
        public void MultiplePages()
        {
            var subaccount1 = client.Subaccounts().Create();
            var subaccount2 = client.Subaccounts().Create();
            var subaccount3 = client.Subaccounts().Create();

            var subaccounts = client.Subaccounts().List().All(new Util.ListParams().SetPageSize(1));

            Assert.Equal(new[] {subaccount3.Id, subaccount2.Id, subaccount1.Id}, subaccounts.Select(s => s.Id));
        }
    }
}