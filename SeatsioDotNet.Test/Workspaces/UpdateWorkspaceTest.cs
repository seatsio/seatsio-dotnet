using Xunit;

namespace SeatsioDotNet.Test.Workspaces;

public class UpdateWorkspaceTest : SeatsioClientTest
{
    [Fact]
    public void UpdateWorkspace()
    {
        var workspace = Client.Workspaces.Create("my workspace");

        Client.Workspaces.Update(workspace.Key, "my ws");

        var retrievedWorkspace = Client.Workspaces.Retrieve(workspace.Key);
        Assert.Equal("my ws", retrievedWorkspace.Name);
        Assert.NotNull(retrievedWorkspace.Key);
        Assert.NotNull(retrievedWorkspace.SecretKey);
        Assert.False(retrievedWorkspace.IsTest);
    }
}