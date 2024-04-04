using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Seasons;

public class RemoveEventFromPartialSeasonTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chartKey = CreateTestChart();
        var topLevelSeason = await Client.Seasons.CreateAsync(chartKey, eventKeys: new[] {"event1", "event2"});
        var partialSeason = await Client.Seasons.CreatePartialSeasonAsync(topLevelSeason.Key, eventKeys: new[] {"event1", "event2"});

        var updatedPartialSeason = await Client.Seasons.RemoveEventFromPartialSeasonAsync(topLevelSeason.Key, partialSeason.Key, "event2");

        Assert.Equal(new[] {"event1"}, updatedPartialSeason.Events.Select(e => e.Key));
    }
}