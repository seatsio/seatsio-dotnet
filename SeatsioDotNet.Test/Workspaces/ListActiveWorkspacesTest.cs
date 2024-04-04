using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Workspaces;

public class ListActiveWorkspacesTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        await Client.Workspaces.CreateAsync("ws1");

        var ws2 = await Client.Workspaces.CreateAsync("ws2");
        await Client.Workspaces.DeactivateAsync(ws2.Key);

        await Client.Workspaces.CreateAsync("ws3");

        var workspaces = Client.Workspaces.Active.All();

        Assert.Equal(new[] {"ws3", "ws1", "Default workspace"}, workspaces.Select(e => e.Name));
    }

    [Fact]
    public async Task Filter()
    {
        await Client.Workspaces.CreateAsync("someWorkspace");
        await Client.Workspaces.CreateAsync("anotherWorkspace");
        await Client.Workspaces.CreateAsync("anotherAnotherWorkspace");
        var ws = await Client.Workspaces.CreateAsync("anoherAnotherAnotherWorkspace");
        await Client.Workspaces.DeactivateAsync(ws.Key);

        var workspaces = Client.Workspaces.Active.All("another");

        Assert.Equal(new[] {"anotherAnotherWorkspace", "anotherWorkspace"}, workspaces.Select(e => e.Name));
    }
}