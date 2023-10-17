﻿using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.Json;

namespace SeatsioDotNet
{
    public class SeatsioClient
    {
        public Charts.Charts Charts { get; }
        public Events.Events Events { get; }
        public Workspaces.Workspaces Workspaces { get; }
        public HoldTokens.HoldTokens HoldTokens { get; }
        public Reports.Events.EventReports EventReports { get; }
        public Reports.Charts.ChartReports ChartReports { get; }
        public Reports.Usage.UsageReports UsageReports { get; }
        public Seasons.Seasons Seasons { get; }

        private SeatsioRestClient RestClient;

        static SeatsioClient()
        {
            ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;
        }

        public SeatsioClient(string secretKey, string workspaceKey, string baseUrl)
        {
            RestClient = CreateRestClient(secretKey, workspaceKey, baseUrl);
            Charts = new Charts.Charts(RestClient);
            Events = new Events.Events(RestClient);
            Workspaces = new Workspaces.Workspaces(RestClient);
            HoldTokens = new HoldTokens.HoldTokens(RestClient);
            EventReports = new Reports.Events.EventReports(RestClient);
            ChartReports = new Reports.Charts.ChartReports(RestClient);
            UsageReports = new Reports.Usage.UsageReports(RestClient);
            Seasons = new Seasons.Seasons(RestClient, this);
        }

        public SeatsioClient(Region region, string secretKey, string workspaceKey) : this(secretKey, workspaceKey,
            region.Url())
        {
        }

        public SeatsioClient(Region region, string secretKey) : this(region, secretKey, null)
        {
        }

        private static SeatsioRestClient CreateRestClient(string secretKey, string workspaceKey, string baseUrl)
        {
            var options = new RestClientOptions(baseUrl)
            {
                Authenticator = new HttpBasicAuthenticator(secretKey, null)
            };
            var client = new SeatsioRestClient(options);
            client.AddDefaultHeader("X-Client-Lib", ".NET");
            if (workspaceKey != null)
            {
                client.AddDefaultHeader("X-Workspace-Key", workspaceKey.ToString());
            }

            return client;
        }
    }
}

public class SeatsioRestClient : RestClient
{
    public SeatsioRestClient(RestClientOptions restClientOptions, int maxRetries = 5) : base(
        new HttpClient(new SeatsioMessageHandler(maxRetries)),
        restClientOptions,
        configureSerialization: s =>
            s.UseSerializer(() => new SystemTextJsonSerializer(SeatsioJsonSerializerOptions())))
    {
    }

    public static JsonSerializerOptions SeatsioJsonSerializerOptions()
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        options.Converters.Add(new DateOnlyConverter());
        options.Converters.Add(new ObjectToInferredTypesConverter());
        return options;
    }
}

public class SeatsioMessageHandler : HttpClientHandler
{
    private int MaxRetries;

    public SeatsioMessageHandler(int maxRetries)
    {
        MaxRetries = maxRetries;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var retryCount = 0;
        var response = await base.SendAsync(request, cancellationToken);
        while (retryCount < MaxRetries && (int) response.StatusCode == 429)
        {
            var waitTime = (int) Math.Pow(2, retryCount + 2) * 100;
            retryCount++;
            Thread.Sleep(waitTime);
            response = await base.SendAsync(request, cancellationToken);
        }

        return response;
    }
}

public class ObjectToInferredTypesConverter : JsonConverter<object>
{
    public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.TokenType switch
        {
            JsonTokenType.True => true,
            JsonTokenType.False => false,
            JsonTokenType.Number when reader.TryGetInt64(out long l) => l,
            JsonTokenType.Number => reader.GetDouble(),
            JsonTokenType.String => reader.GetString()!,
            _ => JsonDocument.ParseValue(ref reader).RootElement.Clone()
        };

    public override void Write(
        Utf8JsonWriter writer,
        object objectToWrite,
        JsonSerializerOptions options) =>
        JsonSerializer.Serialize(writer, objectToWrite, objectToWrite.GetType(), options);
}

public class DateOnlyConverter : JsonConverter<DateOnly>
{
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return DateOnly.Parse(value!);
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
}