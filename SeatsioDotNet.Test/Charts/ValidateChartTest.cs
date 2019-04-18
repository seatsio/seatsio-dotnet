using Xunit;

namespace SeatsioDotNet.Test.Charts
{
    public class ValidateChartTest : SeatsioClientTest
    {
        [Fact]
        public void ValidatePublishedVersion()
        {
            Client.Accounts.UpdateSetting("VALIDATE_DUPLICATE_LABELS", "ERROR");
            Client.Accounts.UpdateSetting("VALIDATE_UNLABELED_OBJECTS", "ERROR");
            Client.Accounts.UpdateSetting("VALIDATE_OBJECTS_WITHOUT_CATEGORIES", "WARNING");
            var chartKey = CreateTestChartWithErrors();
            var result = Client.Charts.ValidatePublishedVersion(chartKey);
            CustomAssert.ContainsOnly(
                new[]
                {
                    "VALIDATE_DUPLICATE_LABELS", "VALIDATE_UNLABELED_OBJECTS"
                }, result.Errors);
            CustomAssert.ContainsOnly(
                new[]
                {
                    "VALIDATE_OBJECTS_WITHOUT_CATEGORIES"
                }, result.Warnings);
        }

        [Fact]
        public void ValidateDraftVersion()
        {
            Client.Accounts.UpdateSetting("VALIDATE_DUPLICATE_LABELS", "OFF");
            Client.Accounts.UpdateSetting("VALIDATE_UNLABELED_OBJECTS", "OFF");
            Client.Accounts.UpdateSetting("VALIDATE_OBJECTS_WITHOUT_CATEGORIES", "OFF");
            var chartKey = CreateTestChartWithErrors();
            Client.Events.Create(chartKey);
            Client.Charts.Update(chartKey, "new name");

            Client.Accounts.UpdateSetting("VALIDATE_DUPLICATE_LABELS", "ERROR");
            Client.Accounts.UpdateSetting("VALIDATE_UNLABELED_OBJECTS", "ERROR");
            Client.Accounts.UpdateSetting("VALIDATE_OBJECTS_WITHOUT_CATEGORIES", "WARNING");

            var result = Client.Charts.ValidateDraftVersion(chartKey);
            CustomAssert.ContainsOnly(
                new[]
                {
                    "VALIDATE_DUPLICATE_LABELS", "VALIDATE_UNLABELED_OBJECTS"
                }, result.Errors);
            CustomAssert.ContainsOnly(
                new[]
                {
                    "VALIDATE_OBJECTS_WITHOUT_CATEGORIES"
                }, result.Warnings);
        }
    }
}