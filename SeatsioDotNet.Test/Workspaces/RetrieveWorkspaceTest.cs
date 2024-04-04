using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Workspaces;

public class RetrieveWorkspaceTest : SeatsioClientTest
{
    [Fact]
    public async Task RetrieveWorkspace()
    {
        var workspace = await Client.Workspaces.CreateAsync("my workspace");

        var retrievedWorkspace = await Client.Workspaces.RetrieveAsync(workspace.Key);

        Assert.Equal("my workspace", retrievedWorkspace.Name);
        Assert.NotNull(retrievedWorkspace.Key);
        Assert.NotNull(retrievedWorkspace.SecretKey);
        Assert.False(retrievedWorkspace.IsTest);
        Assert.True(retrievedWorkspace.IsActive);
    }
}