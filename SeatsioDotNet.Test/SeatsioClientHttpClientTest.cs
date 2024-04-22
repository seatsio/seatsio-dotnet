using System;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication.ExtendedProtection;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Polly;
using Polly.Retry;
using Xunit;

namespace SeatsioDotNet.Test;

public class SeatsioClientHttpClientTest : SeatsioClientTest
{
    [Fact]
    public async Task ReuseHttpClientAcrossMultipleSeatsIoClients()
    {
        var retryPipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions()
            {
                ShouldHandle = new PredicateBuilder().Handle<RateLimitExceededException>(),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                MaxRetryAttempts = 5,
            }).Build();

        var workspace1 = await Client.Workspaces.CreateAsync("workspace1");
        var workspace2 = await Client.Workspaces.CreateAsync("workspace2");

        using var httpClient = new HttpClient();
        var workspace1Client = CreateSeatsioClient(workspace1.SecretKey, httpClient);
        var workspace2Client = CreateSeatsioClient(workspace2.SecretKey, httpClient);

        var c1w1 = retryPipeline.ExecuteAsync(async _ => await  workspace1Client.Charts.CreateAsync("chart1-workspace1"));
        var c1w2 = retryPipeline.ExecuteAsync(async _ => await workspace2Client.Charts.CreateAsync("chart1-workspace2"));
        var c2w2 = retryPipeline.ExecuteAsync(async _ => await workspace2Client.Charts.CreateAsync("chart2-workspace2"));
        var c2w1 = retryPipeline.ExecuteAsync(async _ => await workspace1Client.Charts.CreateAsync("chart2-workspace1"));
        var c3w1 = retryPipeline.ExecuteAsync(async _ => await workspace2Client.Charts.CreateAsync("chart3-workspace2"));

        await Task.WhenAll(c1w1.AsTask(), c1w2.AsTask(), c2w2.AsTask(), c2w1.AsTask(), c3w1.AsTask());

        var workspace1Charts = await retryPipeline.ExecuteAsync(async _ => await workspace1Client.Charts.ListAllAsync().ToListAsync());
        var workspace2Charts = await retryPipeline.ExecuteAsync(async _ => await workspace2Client.Charts.ListAllAsync().ToListAsync());

        workspace1Charts
            .Select(a => a.Name)
            .Should()
            .BeEquivalentTo("chart1-workspace1", "chart2-workspace1");

        workspace2Charts
            .Select(a => a.Name)
            .Should()
            .BeEquivalentTo("chart1-workspace2", "chart2-workspace2", "chart3-workspace2");
    }
}