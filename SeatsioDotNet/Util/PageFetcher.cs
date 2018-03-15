using System;
using RestSharp;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.Util
{
    public class PageFetcher<T>
    {
        private readonly RestClient _restClient;
        private readonly string _path;

        public PageFetcher(RestClient restClient, String path)
        {
            _restClient = restClient;
            _path = path;
        }

        public Page<T> FetchFirstPage(ListParams listParams)
        {
            var restRequest = BuildRequest(listParams);
            return AssertOk(_restClient.Execute<Page<T>>(restRequest));
        }

        public Page<T> FetchAfter(int id, ListParams listParams)
        {
            var restRequest = BuildRequest(listParams).AddQueryParameter("start_after_id", id.ToString());
            return AssertOk(_restClient.Execute<Page<T>>(restRequest));
        }

        private RestRequest BuildRequest(ListParams listParams)
        {
            var restRequest = new RestRequest(_path, Method.GET);
            if (listParams.GetPageSize() != null)
            {
                restRequest.AddQueryParameter("limit", listParams.GetPageSize().ToString());
            }

            return restRequest;
        }
    }
}