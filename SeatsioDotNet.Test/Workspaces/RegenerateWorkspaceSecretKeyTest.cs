using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Workspaces;

public class RegenerateWorkspaceSecretKeyTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var workspace = await Client.Workspaces.CreateAsync("a ws");

        var newSecretKey = await Client.Workspaces.RegenerateSecretKeyAsync(workspace.Key);

        Assert.NotNull(newSecretKey);
        Assert.NotEqual(newSecretKey, workspace.SecretKey);
        var retrievedWorkspace = await Client.Workspaces.RetrieveAsync(workspace.Key);
        Assert.Equal(newSecretKey, retrievedWorkspace.SecretKey);
    }
}