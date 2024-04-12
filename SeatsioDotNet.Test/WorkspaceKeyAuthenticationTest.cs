using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test;

public class WorkspaceKeyAuthenticationTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var workspace = await Client.Workspaces.CreateAsync("some workspace");

        var workspaceClient = CreateSeatsioClient(User.SecretKey, workspace.Key);
        var holdToken = await workspaceClient.HoldTokens.CreateAsync();

        Assert.Equal(workspace.Key, holdToken.workspaceKey);
    }
}