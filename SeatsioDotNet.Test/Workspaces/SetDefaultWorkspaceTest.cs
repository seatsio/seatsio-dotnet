using Xunit;

namespace SeatsioDotNet.Test.Workspaces
{
    public class SetDefaultWorkspaceTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var workspace = Client.Workspaces.Create("a ws");
            
            Client.Workspaces.SetDefault(workspace.Key);

            var retrievedWorkspace = Client.Workspaces.Retrieve(workspace.Key);
            Assert.True(retrievedWorkspace.IsDefault);
        }
    }
}