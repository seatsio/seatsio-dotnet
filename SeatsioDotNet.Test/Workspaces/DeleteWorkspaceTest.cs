using System;
using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Workspaces;

public class DeleteWorkspaceTest : SeatsioClientTest
{
    [Fact]
    public async Task TestDeleteInactiveWorkspace()
    {
        var workspace = await Client.Workspaces.CreateAsync("a ws");
        await Client.Workspaces.DeactivateAsync(workspace.Key);

        await Client.Workspaces.DeleteAsync(workspace.Key);

        Exception ex = await Assert.ThrowsAsync<SeatsioException>(async () => await Client.Workspaces.RetrieveAsync(workspace.Key));
        Assert.Equal("No workspace found with public key '" + workspace.Key + "'.", ex.Message);
    }

    [Fact]
    public async Task TestDeleteActiveWorkspace()
    {
        var workspace = await Client.Workspaces.CreateAsync("a ws");
        
        Exception ex = await Assert.ThrowsAsync<SeatsioException>(async () => await Client.Workspaces.DeleteAsync(workspace.Key));
        Assert.Equal("Cannot delete active workspace, please deactivate it first.", ex.Message);
    }
}