using System;
using RestSharp;
using Xunit;

namespace SeatsioDotNet.Test
{
    public class ExponentialBackoffTest : SeatsioClientTest
    {
        [Fact]
        public void AbortsEventuallyIfServerKeepsReturning429()
        {
            var start = DateTimeOffset.Now;
            var client = new SeatsioRestClient("https://mockbin.org");

            var response = client.Execute<object>(new RestRequest("/bin/0381d6f4-0155-4b8c-937b-73d3d88b2a3f", Method.GET));

            Assert.Equal(429, (int) response.StatusCode);

            var duration = DateTimeOffset.Now.ToUnixTimeSeconds() - start.ToUnixTimeSeconds();
            Assert.True(duration > 10);
            Assert.True(duration < 20);
        }

        [Fact]
        public void AbortsDirectlyIfServerReturnsOtherErrorThan429()
        {
            var start = DateTimeOffset.Now;
            var client = new SeatsioRestClient("https://mockbin.org");

            var response = client.Execute<object>(new RestRequest("/bin/1eea3aab-2bb2-4f92-99c2-50d942fb6294", Method.GET));

            Assert.Equal(400, (int) response.StatusCode);

            var duration = DateTimeOffset.Now.ToUnixTimeSeconds() - start.ToUnixTimeSeconds();
            Assert.True(duration < 2);
        }   
        
        [Fact]
        public void AbortsDirectlyIfServerReturnsError429ButMaxRetries0()
        {
            var start = DateTimeOffset.Now;
            var client = new SeatsioRestClient("https://mockbin.org").SetMaxRetries(0);

            var response = client.Execute<object>(new RestRequest("/bin/0381d6f4-0155-4b8c-937b-73d3d88b2a3f", Method.GET));

            Assert.Equal(429, (int) response.StatusCode);

            var duration = DateTimeOffset.Now.ToUnixTimeSeconds() - start.ToUnixTimeSeconds();
            Assert.True(duration < 2);
        }

        [Fact]
        public void ReturnsSuccessfullyWhenTheServerSendsA429FirstAndThenASuccessfulResponse()
        {
            var client = new SeatsioRestClient("https://httpbin.org");
            for (var i = 0; i < 20; ++i)
            {
                var response = client.Execute<object>(new RestRequest("/status/429:0.25,204:0.75", Method.GET));
                Assert.Equal(204, (int) response.StatusCode);
            }
        }
    }
}