using System;
using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class DeleteEventTest : SeatsioClientTest
{
    [Fact]
    public async Task Delete()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);

        await Client.Events.DeleteAsync(evnt.Key);

        Exception ex = await Assert.ThrowsAsync<SeatsioException>(async () => await Client.Events.RetrieveAsync(evnt.Key));

        Assert.Equal("Event not found: " + evnt.Key + ".", ex.Message);
    }
}