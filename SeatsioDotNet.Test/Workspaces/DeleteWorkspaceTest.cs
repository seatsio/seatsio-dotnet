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

        SeatsioException ex = await Assert.ThrowsAsync<SeatsioException>(async () => await Client.Workspaces.RetrieveAsync(workspace.Key));
        Assert.Equal("WORKSPACE_NOT_FOUND", ex.Errors[0].Code);
    }
}