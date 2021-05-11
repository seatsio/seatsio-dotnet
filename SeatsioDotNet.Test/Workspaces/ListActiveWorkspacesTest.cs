using System.Linq;
using Xunit;

namespace SeatsioDotNet.Test.Workspaces

{
    public class ListActiveWorkspacesTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            Client.Workspaces.Create("ws1");

            var ws2 = Client.Workspaces.Create("ws2");
            Client.Workspaces.Deactivate(ws2.Key);

            Client.Workspaces.Create("ws3");

            var workspaces = Client.Workspaces.Active.All();

            Assert.Equal(new[] {"ws3", "ws1", "Default workspace"}, workspaces.Select(e => e.Name));
        }

        [Fact]
        public void Filter()
        {
            Client.Workspaces.Create("someWorkspace");
            Client.Workspaces.Create("anotherWorkspace");
            Client.Workspaces.Create("anotherAnotherWorkspace");
            var ws = Client.Workspaces.Create("anoherAnotherAnotherWorkspace");
            Client.Workspaces.Deactivate(ws.Key);

            var workspaces = Client.Workspaces.Active.All("another");

            Assert.Equal(new[] {"anotherAnotherWorkspace", "anotherWorkspace"}, workspaces.Select(e => e.Name));
        }
    }
}