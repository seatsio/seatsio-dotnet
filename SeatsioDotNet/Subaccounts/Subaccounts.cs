﻿using System.Collections.Generic;
using RestSharp;
using SeatsioDotNet.Charts;
using SeatsioDotNet.Util;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.Subaccounts
{
    public class Subaccounts
    {
        public Lister<Subaccount> Active { get; }
        public Lister<Subaccount> Inactive { get; }

        private readonly RestClient _restClient;

        public Subaccounts(RestClient restClient)
        {
            _restClient = restClient;
            Active = new Lister<Subaccount>(new PageFetcher<Subaccount>(_restClient, "/subaccounts/active"));
            Inactive = new Lister<Subaccount>(new PageFetcher<Subaccount>(_restClient, "/subaccounts/inactive"));
        }

        public Subaccount Create(string name)
        {
            var requestBody = new Dictionary<string, object>();
            if (name != null)
            {
                requestBody.Add("name", name);
            }

            var restRequest = new RestRequest("/subaccounts", Method.Post)
                .AddJsonBody(requestBody);
            return AssertOk(_restClient.Execute<Subaccount>(restRequest));
        }

        public Subaccount Retrieve(long id)
        {
            var restRequest = new RestRequest("/subaccounts/{id}", Method.Get)
                .AddUrlSegment("id", id);
            return AssertOk(_restClient.Execute<Subaccount>(restRequest));
        }

        public void Update(long id, string name = null)
        {
            var requestBody = new Dictionary<string, object>();
            if (name != null)
            {
                requestBody.Add("name", name);
            }

            var restRequest = new RestRequest("/subaccounts/{id}", Method.Post)
                .AddUrlSegment("id", id)
                .AddJsonBody(requestBody);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public Subaccount Create()
        {
            var restRequest = new RestRequest("/subaccounts", Method.Post);
            return AssertOk(_restClient.Execute<Subaccount>(restRequest));
        }

        public void Activate(long id)
        {
            var restRequest = new RestRequest("/subaccounts/{id}/actions/activate", Method.Post)
                .AddUrlSegment("id", id);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public void Deactivate(long id)
        {
            var restRequest = new RestRequest("/subaccounts/{id}/actions/deactivate", Method.Post)
                .AddUrlSegment("id", id);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public void RegenerateSecretKey(long id)
        {
            var restRequest = new RestRequest("/subaccounts/{id}/secret-key/actions/regenerate", Method.Post)
                .AddUrlSegment("id", id);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public void RegenerateDesignerKey(long id)
        {
            var restRequest = new RestRequest("/subaccounts/{id}/designer-key/actions/regenerate", Method.Post)
                .AddUrlSegment("id", id);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public Chart CopyChartToParent(long id, string chartKey)
        {
            var restRequest = new RestRequest("/subaccounts/{id}/charts/{chartKey}/actions/copy-to/parent", Method.Post)
                .AddUrlSegment("id", id)
                .AddUrlSegment("chartKey", chartKey);
            return AssertOk(_restClient.Execute<Chart>(restRequest));
        }

        public Chart CopyChartToSubaccount(long fromId, long toId, string chartKey)
        {
            var restRequest =
                new RestRequest("/subaccounts/{fromId}/charts/{chartKey}/actions/copy-to/{toId}", Method.Post)
                    .AddUrlSegment("fromId", fromId)
                    .AddUrlSegment("chartKey", chartKey)
                    .AddUrlSegment("toId", toId.ToString());
            return AssertOk(_restClient.Execute<Chart>(restRequest));
        }

        public IEnumerable<Subaccount> ListAll()
        {
            return List().All();
        }

        public IEnumerable<Subaccount> ListAll(string filter)
        {
            return ParametrizedList().All(SubaccountListParams(filter));
        }

        public Page<Subaccount> ListFirstPage(int? pageSize = null)
        {
            return List().FirstPage(pageSize: pageSize);
        }

        public Page<Subaccount> ListFirstPage(string filter, int? pageSize = null)
        {
            return ParametrizedList().FirstPage(listParams: SubaccountListParams(filter), pageSize: pageSize);
        }

        public Page<Subaccount> ListPageAfter(long id, int? pageSize = null)
        {
            return List().PageAfter(id, pageSize: pageSize);
        }

        public Page<Subaccount> ListPageAfter(long id, string filter, int? pageSize = null)
        {
            return ParametrizedList().PageAfter(id, SubaccountListParams(filter), pageSize: pageSize);
        }

        public Page<Subaccount> ListPageBefore(long id, int? pageSize = null)
        {
            return List().PageBefore(id, pageSize: pageSize);
        }

        public Page<Subaccount> ListPageBefore(long id, string filter, int? pageSize = null)
        {
            return ParametrizedList().PageBefore(id, SubaccountListParams(filter), pageSize: pageSize);
        }

        private Lister<Subaccount> List()
        {
            return new Lister<Subaccount>(new PageFetcher<Subaccount>(_restClient, "/subaccounts"));
        }

        private ParametrizedLister<Subaccount> ParametrizedList()
        {
            return new ParametrizedLister<Subaccount>(new PageFetcher<Subaccount>(_restClient, "/subaccounts"));
        }

        private Dictionary<string, object> SubaccountListParams(string filter)
        {
            var chartListParams = new Dictionary<string, object>();

            if (filter != null)
            {
                chartListParams.Add("filter", filter);
            }

            return chartListParams;
        }
    }
}