using System;
using System.Text.Json.Serialization;

namespace SeatsioDotNet.HoldTokens;

public class HoldToken
{
    [JsonPropertyName("holdToken")] public string Token { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    public int ExpiresInSeconds { get; set; }
    public string workspaceKey { get; set; }
}