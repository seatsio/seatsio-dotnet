using System;
using RestSharp;
using Xunit;

namespace SeatsioDotNet.Test;

public class ExponentialBackoffTest : SeatsioClientTest
{
    [Fact]
    public void AbortsEventuallyIfServerKeepsReturning429()
    {
        var start = DateTimeOffset.Now;
        var client = new SeatsioRestClient(new RestClientOptions("https://httpbin.seatsio.net"));

        var response = client.Execute<object>(new RestRequest("/status/429", Method.Get));

        Assert.Equal(429, (int) response.StatusCode);

        var duration = DateTimeOffset.Now.ToUnixTimeSeconds() - start.ToUnixTimeSeconds();
        Assert.True(duration > 10);
        Assert.True(duration < 30);
    }

    [Fact]
    public void AbortsDirectlyIfServerReturnsOtherErrorThan429()
    {
        var start = DateTimeOffset.Now;
        var client = new SeatsioRestClient(new RestClientOptions("https://httpbin.seatsio.net"));

        var response = client.Execute<object>(new RestRequest("/status/400", Method.Get));

        Assert.Equal(400, (int) response.StatusCode);

        var duration = DateTimeOffset.Now.ToUnixTimeSeconds() - start.ToUnixTimeSeconds();
        Assert.True(duration < 2);
    }   
        
    [Fact]
    public void AbortsDirectlyIfServerReturnsError429ButMaxRetries0()
    {
        var start = DateTimeOffset.Now;
        var client = new SeatsioRestClient(new RestClientOptions("https://httpbin.seatsio.net"), 0);

        var response = client.Execute<object>(new RestRequest("/status/429", Method.Get));

        Assert.Equal(429, (int) response.StatusCode);

        var duration = DateTimeOffset.Now.ToUnixTimeSeconds() - start.ToUnixTimeSeconds();
        Assert.True(duration < 2);
    }

    [Fact]
    public void ReturnsSuccessfullyWhenTheServerSendsA429FirstAndThenASuccessfulResponse()
    {
        var client = new SeatsioRestClient(new RestClientOptions("https://httpbin.seatsio.net"));
        for (var i = 0; i < 10; ++i)
        {
            var response = client.Execute<object>(new RestRequest("/status/429:0.25,204:0.75", Method.Get));
            Assert.Equal(204, (int) response.StatusCode);
        }
    }
}