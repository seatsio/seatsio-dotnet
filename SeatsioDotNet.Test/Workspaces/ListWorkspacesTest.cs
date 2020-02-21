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
    }
}