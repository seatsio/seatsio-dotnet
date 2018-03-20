using System.Linq;
using Xunit;

namespace SeatsioDotNet.Test.Subaccounts
{
    public class ListAllSubaccountsTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var subaccount1 = Client.Subaccounts().Create();
            var subaccount2 = Client.Subaccounts().Create();
            var subaccount3 = Client.Subaccounts().Create();

            var subaccounts = Client.Subaccounts().ListAll();

            Assert.Equal(new[] {subaccount3.Id, subaccount2.Id, subaccount1.Id}, subaccounts.Select(s => s.Id));
        }
    }
}