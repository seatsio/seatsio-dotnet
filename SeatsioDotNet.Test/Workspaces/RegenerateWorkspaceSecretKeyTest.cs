using Xunit;

namespace SeatsioDotNet.Test.Workspaces;

public class RegenerateWorkspaceSecretKeyTest : SeatsioClientTest
{
    [Fact]
    public void Test()
    {
        var workspace = Client.Workspaces.Create("a ws");
            
        var newSecretKey = Client.Workspaces.RegenerateSecretKey(workspace.Key);

        Assert.NotNull(newSecretKey);
        Assert.NotEqual(newSecretKey, workspace.SecretKey);
        var retrievedWorkspace = Client.Workspaces.Retrieve(workspace.Key);
        Assert.Equal(newSecretKey, retrievedWorkspace.SecretKey);
    }
}