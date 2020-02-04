using SeatsioDotNet.Accounts;
using Xunit;

namespace SeatsioDotNet.Test.Accounts
{
    public class RetrieveMyAccountTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var account = Client.Accounts.RetrieveMyAccount();
            
            Assert.NotNull(account.SecretKey);
            Assert.NotNull(account.DesignerKey);
            Assert.NotNull(account.Email);
            Assert.True(account.Settings.DraftChartDrawingsEnabled);
            Assert.True(account.Settings.HoldOnSelectForGAs);
            Assert.Equal(ChartValidationLevel.OFF, account.Settings.ChartValidation.ValidateDuplicateLabels);
        }
    }
}