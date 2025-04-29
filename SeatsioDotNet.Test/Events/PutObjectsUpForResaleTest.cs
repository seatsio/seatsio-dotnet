using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using SeatsioDotNet.EventReports;
using SeatsioDotNet.Events;
using SeatsioDotNet.HoldTokens;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class PutObjectsUpForResale : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);

        var result = await Client.Events.PutUpForResaleAsync(evnt.Key, new[] {"A-1", "A-2"}, "listing1");

        Assert.Equal(EventObjectInfo.Resale, (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-1")).Status);
        Assert.Equal("listing1", (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-1")).ResaleListingId);
        Assert.Equal(EventObjectInfo.Resale, (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-2")).Status);
        Assert.Equal("listing1", (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-2")).ResaleListingId);
        Assert.Equal(EventObjectInfo.Free, (await Client.Events.RetrieveObjectInfoAsync(evnt.Key, "A-3")).Status);
        CustomAssert.ContainsOnly(new[] {"A-1", "A-2"}, result.Objects.Keys);
    }
}