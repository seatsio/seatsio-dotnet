﻿using SeatsioDotNet.Charts;
using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class RemoveTagTest : SeatsioClientTest
{
    [Fact]
    public void Test()
    {
        var chart = Client.Charts.Create();
        Client.Charts.AddTag(chart.Key, "tag1");
        Client.Charts.AddTag(chart.Key, "tag2");

        Client.Charts.RemoveTag(chart.Key, "tag2");

        Chart retrievedChart = Client.Charts.Retrieve(chart.Key);
        CustomAssert.ContainsOnly(new[] {"tag1"}, retrievedChart.Tags);
    }
}