using SeatsioDotNet.Reports.Usage;
using Xunit;

namespace SeatsioDotNet.Test.Reports.Usage;

public class UsageMonthTest
{
    [Fact]
    public void Test()
    {
        Assert.True(new UsageMonth(2020, 1).Before(2020, 2));
        Assert.True(new UsageMonth(2020, 1).Before(2020, 12));
        Assert.True(new UsageMonth(2020, 1).Before(2021, 11));
        Assert.False(new UsageMonth(2020, 1).Before(2020, 1));
        Assert.False(new UsageMonth(2020, 1).Before(2019, 1));
        Assert.False(new UsageMonth(2020, 1).Before(2019, 12));
    }
}