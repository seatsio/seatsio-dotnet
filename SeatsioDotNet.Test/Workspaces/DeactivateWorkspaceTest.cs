using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Workspaces;

public class DeactivateWorkspaceTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var workspace = await Client.Workspaces.CreateAsync("a ws");
            
        await Client.Workspaces.DeactivateAsync(workspace.Key);

        var retrievedWorkspace = await Client.Workspaces.RetrieveAsync(workspace.Key);
        Assert.False(retrievedWorkspace.IsActive);
    }
}