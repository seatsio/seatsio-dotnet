using System;
using RestSharp;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.Util
{
    public class PageFetcher<T>
    {
        private readonly RestClient _restClient;
        private readonly string _path;
        private readonly Func<RestRequest, RestRequest> _requestAdapter;

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

        public Page<T> FetchFirstPage(ListParams listParams, int? pageSize)
        {
            var restRequest = BuildRequest(listParams, pageSize);
            return AssertOk(_restClient.Execute<Page<T>>(restRequest));
        }

        public Page<T> FetchAfter(long id, ListParams listParams, int? pageSize)
        {
            var restRequest = BuildRequest(listParams, pageSize).AddQueryParameter("start_after_id", id.ToString());
            return AssertOk(_restClient.Execute<Page<T>>(restRequest));
        }

        public Page<T> FetchBefore(long id, ListParams listParams, int? pageSize)
        {
            var restRequest = BuildRequest(listParams, pageSize).AddQueryParameter("end_before_id", id.ToString());
            return AssertOk(_restClient.Execute<Page<T>>(restRequest));
        }

        private RestRequest BuildRequest(ListParams listParams, int? pageSize)
        {
            var restRequest = new RestRequest(_path, Method.GET);
            if (_requestAdapter != null)
            {
                restRequest = _requestAdapter.Invoke(restRequest);
            }

            if (listParams != null)
            {
                foreach (var param in listParams.AsDictionary())
                {
                    restRequest.AddQueryParameter(param.Key, param.Value);
                }
            }

            if (pageSize != null)
            {
                restRequest.AddQueryParameter("limit", pageSize.ToString());
            }

            return restRequest;
        }
    }
}