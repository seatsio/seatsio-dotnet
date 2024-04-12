using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Workspaces;

public class ActivateWorkspaceTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var workspace = await Client.Workspaces.CreateAsync("a ws");
        await Client.Workspaces.DeactivateAsync(workspace.Key);

        await Client.Workspaces.ActivateAsync(workspace.Key);

        var retrievedWorkspace = await Client.Workspaces.RetrieveAsync(workspace.Key);
        Assert.True(retrievedWorkspace.IsActive);
    }
}