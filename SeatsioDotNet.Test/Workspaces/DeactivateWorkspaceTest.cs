using Xunit;

namespace SeatsioDotNet.Test.Workspaces;

public class DeactivateWorkspaceTest : SeatsioClientTest
{
    [Fact]
    public void Test()
    {
        var workspace = Client.Workspaces.Create("a ws");
            
        Client.Workspaces.Deactivate(workspace.Key);

        var retrievedWorkspace = Client.Workspaces.Retrieve(workspace.Key);
        Assert.False(retrievedWorkspace.IsActive);
    }
}