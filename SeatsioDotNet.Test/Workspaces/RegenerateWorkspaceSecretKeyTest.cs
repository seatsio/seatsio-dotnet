using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Workspaces;

public class RegenerateWorkspaceSecretKeyTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var workspace = await Client.Workspaces.CreateAsync("a ws");

#pragma warning disable CS0618 // testing the obsolete method on purpose
        var newSecretKey = await Client.Workspaces.RegenerateSecretKeyAsync(workspace.Key);
#pragma warning restore CS0618

        Assert.NotNull(newSecretKey);
        Assert.NotEqual(newSecretKey, workspace.SecretKey);
        var retrievedWorkspace = await Client.Workspaces.RetrieveAsync(workspace.Key);
        Assert.Equal(newSecretKey, retrievedWorkspace.SecretKey);
    }
}