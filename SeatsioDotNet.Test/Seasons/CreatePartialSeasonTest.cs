using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Seasons;

public class CreatePartialSeasonTest : SeatsioClientTest
{
    [Fact]
    public async Task KeyCanBePassedIn()
    {
        var chartKey = CreateTestChart();
        var topLevelSeason = await Client.Seasons.CreateAsync(chartKey);

        var partialSeason = await Client.Seasons.CreatePartialSeasonAsync(topLevelSeason.Key, partialSeasonKey: "aPartialSeason");

        Assert.Equal("aPartialSeason", partialSeason.Key);
        Assert.True(partialSeason.IsPartialSeason);
        Assert.Equal(topLevelSeason.Key, partialSeason.TopLevelSeasonKey);
    }
    
    [Fact]
    public async Task NameCanBePassedIn()
    {
        var chartKey = CreateTestChart();
        var topLevelSeason = await Client.Seasons.CreateAsync(chartKey);

        var partialSeason = await Client.Seasons.CreatePartialSeasonAsync(topLevelSeason.Key, name: "aPartialSeason");

        Assert.Equal("aPartialSeason", partialSeason.Name);
    }

    [Fact]
    public async Task EventKeysCanBePassedIn()
    {
        var chartKey = CreateTestChart();
        var topLevelSeason = await Client.Seasons.CreateAsync(chartKey, eventKeys: new[] {"event1", "event2"});

        var partialSeason = await Client.Seasons.CreatePartialSeasonAsync(topLevelSeason.Key, eventKeys: new[] {"event1", "event2"});

        Assert.Equal(new[] {"event1", "event2"}, partialSeason.Events.Select(e => e.Key));
        Assert.Equal(new[] {partialSeason.Key}, partialSeason.Events[0].PartialSeasonKeysForEvent);
    }
}