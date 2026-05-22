using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Events.Channels;

public class AreaPartitionLabelTest : SeatsioClientTest
{
    [Fact]
    public async Task AreaPartitionLabel()
    {
        var event1 = await Client.Events.CreateAsync(CreateTestChart());

        await Client.Events.Channels.AddAsync(event1.Key, "channelKey1", "channel 1", "#FFFF98", 1, new[] {"A-1"});

        var retrievedEvent = await Client.Events.RetrieveAsync(event1.Key);
        var channel1 = retrievedEvent.Channels[0];
        Assert.Equal($"GA1##{channel1.Id}", channel1.AreaPartitionLabel("GA1"));
    }
}

