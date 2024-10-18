using System.Linq;
using System.Threading.Tasks;
using SeatsioDotNet.Events;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class ListStatusChangesForObjectTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.ChangeObjectStatusAsync(new[]
        {
            new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"A-1"}, status: "s1"),
            new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"A-1"}, status: "s2"),
            new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"A-1"}, status: "s3"),
            new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"A-1"}, status: "s4"),
            new StatusChangeRequest(eventKey: evnt.Key, objects: new[] {"A-2"}, status: "s5")
        });
        await WaitForStatusChanges(Client, evnt, 5);

        var statusChanges = Client.Events.StatusChangesForObject(evnt.Key, "A-1").AllAsync();

        Assert.Equal(new[] {"s4", "s3", "s2", "s1"}, statusChanges.Select(s => s.Status));
    }
}