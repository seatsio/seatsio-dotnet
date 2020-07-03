using Xunit;

namespace SeatsioDotNet.Test.Workspaces
{
    public class RetrieveWorkspaceTest : SeatsioClientTest
    {
        [Fact]
        public void RetrieveWorkspace()
        {
            var workspace = Client.Workspaces.Create("my workspace");

            var retrievedWorkspace = Client.Workspaces.Retrieve(workspace.Key);

            Assert.Equal("my workspace", retrievedWorkspace.Name);
            Assert.NotNull(retrievedWorkspace.Key);
            Assert.NotNull(retrievedWorkspace.SecretKey);
            Assert.False(retrievedWorkspace.IsTest);
            Assert.True(retrievedWorkspace.IsActive);
        }
    }
}