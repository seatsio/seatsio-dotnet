﻿using System;
using System.Collections.Generic;
using RestSharp;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.Util;

public class PageFetcher<T>
{
    private readonly RestClient _restClient;
    private readonly string _path;
    private readonly Func<RestRequest, RestRequest> _requestAdapter;
    private readonly Dictionary<string, string> _queryParams;

    public PageFetcher(RestClient restClient, String path)
    {
        _restClient = restClient;
        _path = path;
    }

    public PageFetcher(RestClient restClient, String path, Func<RestRequest, RestRequest> requestAdapter)
    {
        _restClient = restClient;
        _path = path;
        _requestAdapter = requestAdapter;
    }

    public PageFetcher(RestClient restClient, String path, Func<RestRequest, RestRequest> requestAdapter, Dictionary<String, String> queryParams)
    {
        _restClient = restClient;
        _path = path;
        _requestAdapter = requestAdapter;
        _queryParams = queryParams;
    }

    public Page<T> FetchFirstPage(Dictionary<string, object> listParams = null, int? pageSize = null)
    {
        var restRequest = BuildRequest(listParams, pageSize);
        return AssertOk(_restClient.Execute<Page<T>>(restRequest));
    }

    public Page<T> FetchAfter(long id, Dictionary<string, object> listParams = null, int? pageSize = null)
    {
        var restRequest = BuildRequest(listParams, pageSize).AddQueryParameter("start_after_id", id.ToString());
        return AssertOk(_restClient.Execute<Page<T>>(restRequest));
    }

    public Page<T> FetchBefore(long id, Dictionary<string, object> listParams = null, int? pageSize = null)
    {
        var restRequest = BuildRequest(listParams, pageSize).AddQueryParameter("end_before_id", id.ToString());
        return AssertOk(_restClient.Execute<Page<T>>(restRequest));
    }

    private RestRequest BuildRequest(Dictionary<string, object> listParams, int? pageSize)
    {
        var restRequest = new RestRequest(_path);
        if (_requestAdapter != null)
        {
            restRequest = _requestAdapter.Invoke(restRequest);
        }

        if (listParams != null)
        {
            foreach (var param in listParams)
            {
                restRequest.AddQueryParameter(param.Key, param.Value.ToString());
            }
        }

        if (pageSize != null)
        {
            restRequest.AddQueryParameter("limit", pageSize.ToString());
        }

        if (_queryParams != null)
        {
            foreach (var queryParam in _queryParams)
            {
                restRequest.AddQueryParameter(queryParam.Key, queryParam.Value);
            }
        }

        return restRequest;
    }
}