using Xunit;

namespace SeatsioDotNet.Test;

public class WorkspaceKeyAuthenticationTest : SeatsioClientTest
{
    [Fact]
    public void Test()
    {
        var workspace = Client.Workspaces.Create("some workspace");

        var workspaceClient = CreateSeatsioClient(User.SecretKey, workspace.Key);
        var holdToken = workspaceClient.HoldTokens.Create();

        Assert.Equal(workspace.Key, holdToken.workspaceKey);
    }
}