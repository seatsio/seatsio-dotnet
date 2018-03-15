using Xunit;

namespace SeatsioDotNet.Test.Subaccounts
{
    public class CreateSubaccountTest : SeatsioClientTest
    {
        [Fact]
        public void test()
        {
            var subaccount = client.Subaccounts().Create("joske");

            Assert.NotEqual(0, subaccount.Id);
            Assert.NotNull(subaccount.SecretKey);
            Assert.NotNull(subaccount.DesignerKey);
            Assert.NotNull(subaccount.PublicKey);
            Assert.Equal("joske", subaccount.Name);
            Assert.True(subaccount.Active);
        }

        [Fact]
        public void nameIsOptional()
        {
            var subaccount = client.Subaccounts().Create();

            Assert.Null(subaccount.Name);
        }
    }
}