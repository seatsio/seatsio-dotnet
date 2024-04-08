using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Workspaces;

public class ListInactiveWorkspacesTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var ws1 = await Client.Workspaces.CreateAsync("ws1");
        await Client.Workspaces.DeactivateAsync(ws1.Key);

        await Client.Workspaces.CreateAsync("ws2");

        var ws3 = await Client.Workspaces.CreateAsync("ws3");
        await Client.Workspaces.DeactivateAsync(ws3.Key);

        var workspaces = Client.Workspaces.Inactive.AllAsync();

        Assert.Equal(new[] {"ws3", "ws1"}, workspaces.Select(e => e.Name));
    }

    [Fact]
    public async Task Filter()
    {
        await Client.Workspaces.CreateAsync("someWorkspace");
            
        var ws1 = await Client.Workspaces.CreateAsync("anotherWorkspace");
        await Client.Workspaces.DeactivateAsync(ws1.Key);
            
        var ws2 = await Client.Workspaces.CreateAsync("anotherAnotherWorkspace");
        await Client.Workspaces.DeactivateAsync(ws2.Key);
            
        var ws = await Client.Workspaces.CreateAsync("anoherAnotherAnotherWorkspace");

        var workspaces = Client.Workspaces.Inactive.AllAsync("another");

        Assert.Equal(new[] {"anotherAnotherWorkspace", "anotherWorkspace"}, workspaces.Select(e => e.Name));
    }
}