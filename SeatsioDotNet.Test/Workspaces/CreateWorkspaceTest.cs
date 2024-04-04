using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Workspaces;

public class CreateWorkspaceTest : SeatsioClientTest
{
    [Fact]
    public async Task CreateWorkspace()
    {
        var workspace = await Client.Workspaces.CreateAsync("my workspace");

        Assert.Equal("my workspace", workspace.Name);
        Assert.NotNull(workspace.Key);
        Assert.NotNull(workspace.SecretKey);
        Assert.False(workspace.IsTest);
        Assert.True(workspace.IsActive);
    } 
        
    [Fact]
    public async Task CreateTestWorkspace()
    {
        var workspace = await Client.Workspaces.CreateAsync("my workspace", true);

        Assert.Equal("my workspace", workspace.Name);
        Assert.NotNull(workspace.Key);
        Assert.NotNull(workspace.SecretKey);
        Assert.True(workspace.IsTest);
        Assert.True(workspace.IsActive);
    }
}