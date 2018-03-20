using Xunit;

namespace SeatsioDotNet.Test.Charts
{
    public class ListAllTagsTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chart1 = Client.Charts().Create();
            Client.Charts().AddTag(chart1.Key, "tag1");
            Client.Charts().AddTag(chart1.Key, "tag2");

            var chart2 = Client.Charts().Create();
            Client.Charts().AddTag(chart2.Key, "tag3");

            var tags = Client.Charts().ListAllTags();
            CustomAssert.ContainsOnly(new[] {"tag1", "tag2", "tag3"}, tags);
        }
    }
}