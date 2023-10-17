﻿using RestSharp;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.HoldTokens;

public class HoldTokens
{
    private readonly RestClient _restClient;

    public HoldTokens(RestClient restClient)
    {
        _restClient = restClient;
    }

    public HoldToken Create()
    {
        var restRequest = new RestRequest("/hold-tokens", Method.Post);
        return AssertOk(_restClient.Execute<HoldToken>(restRequest));
    }

    public HoldToken Create(int expiresInMinutes)
    {
        var restRequest = new RestRequest("/hold-tokens", Method.Post)
            .AddJsonBody(new {expiresInMinutes});
        return AssertOk(_restClient.Execute<HoldToken>(restRequest));
    }

    public HoldToken ExpiresInMinutes(string token, int expiresInMinutes)
    {
        var restRequest = new RestRequest("/hold-tokens/{token}", Method.Post)
            .AddUrlSegment("token", token)
            .AddJsonBody(new {expiresInMinutes});
        return AssertOk(_restClient.Execute<HoldToken>(restRequest));
    }

    public HoldToken Retrieve(string token)
    {
        var restRequest = new RestRequest("/hold-tokens/{token}", Method.Get)
            .AddUrlSegment("token", token);
        return AssertOk(_restClient.Execute<HoldToken>(restRequest));
    }
}