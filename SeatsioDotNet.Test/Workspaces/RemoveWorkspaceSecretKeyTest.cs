using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Workspaces;

public class RemoveWorkspaceSecretKeyTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var workspace = await Client.Workspaces.CreateAsync("a ws");
        var newSecretKey = await Client.Workspaces.AddSecretKeyAsync(workspace.Key);

        var retrievedWorkspace = await Client.Workspaces.RetrieveAsync(workspace.Key);
        CustomAssert.ContainsOnly(retrievedWorkspace.secretKeys, new[]{retrievedWorkspace.SecretKey, newSecretKey});

        await Client.Workspaces.RemoveSecretKeyAsync(workspace.Key, workspace.SecretKey);

        var finalStateWorkspace = await Client.Workspaces.RetrieveAsync(workspace.Key);
        CustomAssert.ContainsOnly(finalStateWorkspace.secretKeys, new[]{newSecretKey});
    }
}