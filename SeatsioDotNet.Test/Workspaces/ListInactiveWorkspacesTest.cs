using System.Linq;
using Xunit;

namespace SeatsioDotNet.Test.Workspaces;

public class ListInactiveWorkspacesTest : SeatsioClientTest
{
    [Fact]
    public void Test()
    {
        var ws1 = Client.Workspaces.Create("ws1");
        Client.Workspaces.Deactivate(ws1.Key);

        Client.Workspaces.Create("ws2");

        var ws3 = Client.Workspaces.Create("ws3");
        Client.Workspaces.Deactivate(ws3.Key);

        var workspaces = Client.Workspaces.Inactive.All();

        Assert.Equal(new[] {"ws3", "ws1"}, workspaces.Select(e => e.Name));
    }

    [Fact]
    public void Filter()
    {
        Client.Workspaces.Create("someWorkspace");
            
        var ws1 = Client.Workspaces.Create("anotherWorkspace");
        Client.Workspaces.Deactivate(ws1.Key);
            
        var ws2 = Client.Workspaces.Create("anotherAnotherWorkspace");
        Client.Workspaces.Deactivate(ws2.Key);
            
        var ws = Client.Workspaces.Create("anoherAnotherAnotherWorkspace");

        var workspaces = Client.Workspaces.Inactive.All("another");

        Assert.Equal(new[] {"anotherAnotherWorkspace", "anotherWorkspace"}, workspaces.Select(e => e.Name));
    }
}