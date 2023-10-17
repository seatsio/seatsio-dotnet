using System.Collections.Generic;
using RestSharp;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.Events;

public class Channels
{
    private readonly RestClient _restClient;

    public Channels(RestClient restClient)
    {
        _restClient = restClient;
    }

    public void Replace(string eventKey, List<Channel> channels)
    {
        var requestBody = ReplaceChannelsRequest(channels);
        var restRequest = new RestRequest("/events/{key}/channels/replace", Method.Post)
            .AddUrlSegment("key", eventKey)
            .AddJsonBody(requestBody);
        AssertOk(_restClient.Execute<object>(restRequest));
    }

    private Dictionary<string, object> ReplaceChannelsRequest(List<Channel> channels)
    {
        var request = new Dictionary<string, object>();
        request.Add("channels", channels);
        return request;
    }

    public void Add(string eventKey, string channelKey, string name, string color, int? index, string[] objects)
    {
        var body = new Dictionary<string, object>();
        body.Add("key", channelKey);
        body.Add("name", name);
        body.Add("color", color);
        if (index != null) body.Add("index", index);
        if (objects != null) body.Add("objects", objects);
        var restRequest = new RestRequest("/events/{key}/channels", Method.Post)
            .AddUrlSegment("key", eventKey)
            .AddJsonBody(body);
        AssertOk(_restClient.Execute<object>(restRequest));
    }

    public void Add(string eventKey, ChannelCreationParams[] channelCreationParams)
    {
        var channels = new List<Dictionary<string, object>>();
        foreach (var param in channelCreationParams)
        {
            var channelToCreate = new Dictionary<string, object>();
            if (param.Key != null)
            {
                channelToCreate.Add("key", param.Key);
            }

            if (param.Name != null)
            {
                channelToCreate.Add("name", param.Name);
            }

            if (param.Color != null)
            {
                channelToCreate.Add("color", param.Color);
            }

            if (param.Index != null)
            {
                channelToCreate.Add("index", param.Index);
            }

            if (param.Objects != null)
            {
                channelToCreate.Add("objects", param.Objects);
            }

            channels.Add(channelToCreate);
        }

        var restRequest = new RestRequest("/events/{key}/channels", Method.Post)
            .AddUrlSegment("key", eventKey)
            .AddJsonBody(channels);
        AssertOk(_restClient.Execute<object>(restRequest));
    }

    public void Remove(string eventKey, string channelKey)
    {
        var restRequest = new RestRequest("/events/{eventKey}/channels/{channelKey}", Method.Delete)
            .AddUrlSegment("eventKey", eventKey)
            .AddUrlSegment("channelKey", channelKey);
        AssertOk(_restClient.Execute<object>(restRequest));
    }

    public void AddObjects(string eventKey, string channelKey, string[] objects)
    {
        var body = new Dictionary<string, object>();
        body.Add("objects", objects);
        var restRequest = new RestRequest("/events/{eventKey}/channels/{channelKey}/objects", Method.Post)
            .AddUrlSegment("eventKey", eventKey)
            .AddUrlSegment("channelKey", channelKey)
            .AddJsonBody(body);
        AssertOk(_restClient.Execute<object>(restRequest));
    }

    public void RemoveObjects(string eventKey, string channelKey, string[] objects)
    {
        var body = new Dictionary<string, object>();
        body.Add("objects", objects);
        var restRequest = new RestRequest("/events/{eventKey}/channels/{channelKey}/objects", Method.Delete)
            .AddUrlSegment("eventKey", eventKey)
            .AddUrlSegment("channelKey", channelKey)
            .AddJsonBody(body);
        AssertOk(_restClient.Execute<object>(restRequest));
    }

    public void Update(string eventKey, string channelKey, string channelName, string color, string[] objects)
    {
        var body = new Dictionary<string, object>();
        if (channelName != null) body.Add("name", channelName);
        if (color != null) body.Add("color", color);
        if (objects != null) body.Add("objects", objects);
        var restRequest = new RestRequest($"/events/{{eventKey}}/channels/{channelKey}", Method.Post)
            .AddUrlSegment("eventKey", eventKey)
            .AddUrlSegment("channelKey", channelKey)
            .AddJsonBody(body);
        AssertOk(_restClient.Execute<object>(restRequest));
    }
}