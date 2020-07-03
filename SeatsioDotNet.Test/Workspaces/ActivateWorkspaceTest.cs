using Xunit;

namespace SeatsioDotNet.Test.Workspaces
{
    public class ActivateWorkspaceTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var workspace = Client.Workspaces.Create("a ws");
            Client.Workspaces.Deactivate(workspace.Key);

            Client.Workspaces.Activate(workspace.Key);

            var retrievedWorkspace = Client.Workspaces.Retrieve(workspace.Key);
            Assert.True(retrievedWorkspace.IsActive);
        }
    }
}