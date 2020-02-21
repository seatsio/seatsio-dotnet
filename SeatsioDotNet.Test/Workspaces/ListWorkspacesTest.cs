using System.Linq;
using Xunit;

namespace SeatsioDotNet.Test.Workspaces

{
    public class ListWorkspacesTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            Client.Workspaces.Create("ws1");
            Client.Workspaces.Create("ws2");
            Client.Workspaces.Create("ws3");

            var workspaces = Client.Workspaces.ListAll();

            Assert.Equal(new[] {"ws3", "ws2", "ws1", "Main workspace"}, workspaces.Select(e => e.Name));
        }  
        
        [Fact]
        public void Filter()
        {
            Client.Workspaces.Create("someWorkspace");
            Client.Workspaces.Create("anotherWorkspace");
            Client.Workspaces.Create("anotherAnotherWorkspace");

            var workspaces = Client.Workspaces.ListAll("another");

            Assert.Equal(new[] {"anotherAnotherWorkspace", "anotherWorkspace"}, workspaces.Select(e => e.Name));
        }
    }
}