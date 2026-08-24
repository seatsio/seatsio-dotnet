using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Workspaces;

public class AddWorkspaceSecretKeyTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var workspace = await Client.Workspaces.CreateAsync("a ws");

        var newSecretKey = await Client.Workspaces.AddSecretKeyAsync(workspace.Key);

        Assert.NotNull(newSecretKey);
        Assert.NotEqual(newSecretKey, workspace.SecretKey);
        var retrievedWorkspace = await Client.Workspaces.RetrieveAsync(workspace.Key);
        CustomAssert.ContainsOnly(retrievedWorkspace.secretKeys, new[]{retrievedWorkspace.SecretKey, newSecretKey});
    }
}