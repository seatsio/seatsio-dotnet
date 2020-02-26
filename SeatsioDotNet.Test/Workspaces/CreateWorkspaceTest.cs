using Xunit;

namespace SeatsioDotNet.Test.Workspaces
{
    public class CreateWorkspaceTest : SeatsioClientTest
    {
        [Fact]
        public void CreateWorkspace()
        {
            var workspace = Client.Workspaces.Create("my workspace");

            Assert.Equal("my workspace", workspace.Name);
            Assert.NotNull(workspace.Key);
            Assert.NotNull(workspace.SecretKey);
            Assert.False(workspace.IsTest);
        } 
        
        [Fact]
        public void CreateTestWorkspace()
        {
            var workspace = Client.Workspaces.Create("my workspace", true);

            Assert.Equal("my workspace", workspace.Name);
            Assert.NotNull(workspace.Key);
            Assert.NotNull(workspace.SecretKey);
            Assert.True(workspace.IsTest);
        }
    }
}