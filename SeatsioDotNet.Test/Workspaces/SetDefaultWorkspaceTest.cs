using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Workspaces;

public class SetDefaultWorkspaceTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var workspace = await Client.Workspaces.CreateAsync("a ws");

        await Client.Workspaces.SetDefaultAsync(workspace.Key);

        var retrievedWorkspace = await Client.Workspaces.RetrieveAsync(workspace.Key);
        Assert.True(retrievedWorkspace.IsDefault);
    }
}