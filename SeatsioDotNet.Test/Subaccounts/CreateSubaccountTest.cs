using Xunit;

namespace SeatsioDotNet.Test.Subaccounts
{
    public class CreateSubaccountTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var subaccount = Client.Subaccounts.Create("joske");

            Assert.NotEqual(0, subaccount.Id);
            Assert.NotNull(subaccount.SecretKey);
            Assert.NotNull(subaccount.DesignerKey);
            Assert.NotNull(subaccount.PublicKey);
            Assert.Equal("joske", subaccount.Name);
            Assert.True(subaccount.Active);
        }

        [Fact]
        public void NameIsOptional()
        {
            var subaccount = Client.Subaccounts.Create();

            Assert.Null(subaccount.Name);
        }
    }
}