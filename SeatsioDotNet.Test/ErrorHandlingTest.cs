using System;
using System.Threading.Tasks;
using RestSharp;
using SeatsioDotNet.Util;
using Xunit;

namespace SeatsioDotNet.Test;

public class ErrorHandlingTest : SeatsioClientTest
{
    [Fact]
    public async Task Test4xx()
    {
        var e = await Assert.ThrowsAsync<SeatsioException>(async () =>
            await Client.Events.RetrieveObjectInfoAsync("unexistingEvent", "unexistingObject"));

        Assert.StartsWith("Event not found: unexistingEvent was not found in workspace", e.Message);
        Assert.Equal("EVENT_NOT_FOUND", e.Errors[0].Code);
        Assert.StartsWith("Event not found: unexistingEvent was not found in workspace", e.Errors[0].Message);
        Assert.NotNull(e.RequestId);
    }

    [Fact]
    public void Test5xx()
    {
        var client = new SeatsioRestClient(new RestClientOptions("https://httpbin.seatsio.net"));

        var e = Assert.Throws<SeatsioException>(() => RestUtil.AssertOk(client.Execute<object>(new RestRequest("/status/500"))));

        Assert.Equal("Get https://httpbin.seatsio.net/status/500 resulted in a 500 Internal Server Error response. Body: ", e.Message);
    }

    [Fact]
    public async Task WeirdError()
    {
        var e = await Assert.ThrowsAsync<SeatsioException>(() =>
            new SeatsioClient("", null, "unknownProtocol://").Events.RetrieveObjectInfoAsync("unexistingEvent", "unexistingObject"));

        Assert.Equal("Get  resulted in a 0  response. Body: ", e.Message);
        Assert.Null(e.Errors);
        Assert.Null(e.RequestId);
    }

    [Fact]
    public async Task TestTimeout()
    {
        var options = new RestClientOptions("https://httpbin.seatsio.net")
        {
            Timeout = TimeSpan.FromMilliseconds(10)
        };
        var client = new SeatsioRestClient(options, maxRetries: 0);

        var response = await client.ExecuteAsync<object>(new RestRequest("/delay/1"));

        Assert.Equal(0, (int)response.StatusCode);
        Assert.IsType<TaskCanceledException>(response.ErrorException);
    }
}