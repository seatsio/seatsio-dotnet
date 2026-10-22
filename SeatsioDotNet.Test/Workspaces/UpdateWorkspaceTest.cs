using System.Threading.Tasks;
using SeatsioDotNet.Workspaces;
using Xunit;

namespace SeatsioDotNet.Test.Workspaces;

public class UpdateWorkspaceTest : SeatsioClientTest
{
    [Fact]
    public async Task UpdateWorkspace()
    {
        var workspace = await Client.Workspaces.CreateAsync("my workspace");

        await Client.Workspaces.UpdateAsync(workspace.Key, "my ws");

        var retrievedWorkspace = await Client.Workspaces.RetrieveAsync(workspace.Key);
        Assert.Equal("my ws", retrievedWorkspace.Name);
        Assert.NotNull(retrievedWorkspace.Key);
        Assert.NotNull(retrievedWorkspace.SecretKey);
        Assert.False(retrievedWorkspace.IsTest);
    }

    [Fact]
    public async Task UpdateWorkspaceWithForSaleMode()
    {
        var workspace = await Client.Workspaces.CreateAsync("my workspace");

        await Client.Workspaces.UpdateAsync(workspace.Key, "my ws", ForSaleMode.UNLIMITED_BILLABLE);

        var retrievedWorkspace = await Client.Workspaces.RetrieveAsync(workspace.Key);
        Assert.Equal("my ws", retrievedWorkspace.Name);
    }
}