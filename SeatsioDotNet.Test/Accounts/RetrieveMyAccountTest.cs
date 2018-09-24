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
            Assert.NotNull(account.PublicKey);
            Assert.NotNull(account.Email);
            Assert.True(account.Settings.DraftChartDrawingsEnabled);
            Assert.Equal(account.Settings.ChartValidation.ValidateDuplicateLabels, "WARNING");
            Assert.Equal(account.Settings.ChartValidation.ValidateObjectsWithoutCategories, "WARNING");
            Assert.Equal(account.Settings.ChartValidation.ValidateUnlabeledObjects, "WARNING");
        }
    }
}