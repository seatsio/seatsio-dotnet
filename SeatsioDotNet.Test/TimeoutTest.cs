using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test;

public class TimeoutTest
{
    [Fact]
    public void DefaultTimeoutIs30Seconds()
    {
        Assert.Equal(TimeSpan.FromSeconds(30), SeatsioClient.DefaultTimeout);
    }

    [Fact]
    public async Task TimeoutIsConfigurableWithCustomHttpClient()
    {
        using var httpClient = new HttpClient(new HangingHandler());
        var client = new SeatsioClient(Region.EU(), "secretKey", null, httpClient, TimeSpan.FromMilliseconds(200));

        await Assert.ThrowsAsync<SeatsioTimeoutException>(() => client.Charts.CreateAsync("chart"));
    }

    private class HangingHandler : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await Task.Delay(Timeout.Infinite, cancellationToken);
            return null;
        }
    }
}
