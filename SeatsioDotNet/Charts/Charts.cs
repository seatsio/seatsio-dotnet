﻿using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using RestSharp;
using SeatsioDotNet.Util;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.Charts
{
    public class Charts
    {
        public Lister<Chart> Archive { get; }

        private readonly RestClient _restClient;

        public Charts(RestClient restClient)
        {
            _restClient = restClient;
            Archive = new Lister<Chart>(new PageFetcher<Chart>(_restClient, "/charts/archive"));
        }

        public Chart Create(string name = null, string venueType = null, IEnumerable<Category> categories = null)
        {
            var requestBody = new Dictionary<string, object>();

            if (name != null)
            {
                requestBody.Add("name", name);
            }

            if (venueType != null)
            {
                requestBody.Add("venueType", venueType);
            }

            if (categories != null)
            {
                requestBody.Add("categories", categories.Select(c => c.AsDictionary()));
            }

            var restRequest = new RestRequest("/charts", Method.Post)
                .AddJsonBody(requestBody);
            return AssertOk(_restClient.Execute<Chart>(restRequest));
        }

        public void Update(string chartKey, string name = null, IEnumerable<Category> categories = null)
        {
            var requestBody = new Dictionary<string, object>();

            if (name != null)
            {
                requestBody.Add("name", name);
            }

            if (categories != null)
            {
                requestBody.Add("categories", categories.Select(c => c.AsDictionary()));
            }

            var restRequest = new RestRequest("/charts/{key}", Method.Post)
                .AddUrlSegment("key", chartKey)
                .AddJsonBody(requestBody);
            AssertOk(_restClient.Execute<object>(restRequest));
        }
        
        public void AddCategory(string chartKey, Category category)
        {
            var restRequest = new RestRequest("/charts/{chartKey}/categories", Method.Post)
                .AddUrlSegment("chartKey", chartKey)
                .AddJsonBody(category.AsDictionary());
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public void RemoveCategory(string chartKey, object categoryKey)
        {
            var restRequest = new RestRequest("/charts/{chartKey}/categories/{categoryKey}", Method.Delete)
                .AddUrlSegment("chartKey", chartKey)
                .AddUrlSegment("categoryKey", categoryKey.ToString());
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public IEnumerable<Category> ListCategories(string chartKey)
        {
            var restRequest = new RestRequest($"/charts/{chartKey}/categories", Method.Get);
            return _restClient.Execute<CategoryList>(restRequest).Data.List;
        }

        private class CategoryList
        {
            [JsonPropertyName("categories")]
            public IEnumerable<Category> List { get; set; }
        }

        public Chart Copy(string chartKey)
        {
            var restRequest = new RestRequest("/charts/{key}/version/published/actions/copy", Method.Post)
                .AddUrlSegment("key", chartKey);
            return AssertOk(_restClient.Execute<Chart>(restRequest));
        }

        public Chart CopyToSubaccount(string chartKey, long subaccountId)
        {
            var restRequest = new RestRequest("/charts/{key}/version/published/actions/copy-to/{subaccountId}", Method.Post)
                .AddUrlSegment("key", chartKey)
                .AddUrlSegment("subaccountId", subaccountId.ToString());
            return AssertOk(_restClient.Execute<Chart>(restRequest));
        }    
        
        public Chart CopyToWorkspace(string chartKey, string toWorkspaceKey)
        {
            var restRequest = new RestRequest("/charts/{key}/version/published/actions/copy-to-workspace/{toWorkspaceKey}", Method.Post)
                .AddUrlSegment("key", chartKey)
                .AddUrlSegment("toWorkspaceKey", toWorkspaceKey);
            return AssertOk(_restClient.Execute<Chart>(restRequest));
        }

        public Chart CopyDraftVersion(string chartKey)
        {
            var restRequest = new RestRequest("/charts/{key}/version/draft/actions/copy", Method.Post)
                .AddUrlSegment("key", chartKey);
            return AssertOk(_restClient.Execute<Chart>(restRequest));
        }

        public void AddTag(string chartKey, string tag)
        {
            var restRequest = new RestRequest("/charts/{key}/tags/{tag}", Method.Post)
                .AddUrlSegment("key", chartKey)
                .AddUrlSegment("tag", tag);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public void RemoveTag(string chartKey, string tag)
        {
            var restRequest = new RestRequest("/charts/{key}/tags/{tag}", Method.Delete)
                .AddUrlSegment("key", chartKey)
                .AddUrlSegment("tag", tag);
            AssertOk(_restClient.Execute<object>(restRequest));
        }
        
        public Chart Retrieve(string chartKey, bool? expandEvents = null)
        {
            var restRequest = new RestRequest("/charts/{key}", Method.Get)
                .AddUrlSegment("key", chartKey);

            if (expandEvents != null && expandEvents.Value)
            {
                restRequest.AddQueryParameter("expand", "events");
            }

            return AssertOk(_restClient.Execute<Chart>(restRequest));
        }

        public IEnumerable<string> ListAllTags()
        {
            var restRequest = new RestRequest("/charts/tags", Method.Get);
            return AssertOk(_restClient.Execute<Tags>(restRequest)).List;
        }

        private class Tags
        {
            [JsonPropertyName("tags")]
            public IEnumerable<string> List { get; set; }
        }

        public void MoveToArchive(string chartKey)
        {
            var restRequest = new RestRequest("/charts/{key}/actions/move-to-archive", Method.Post)
                .AddUrlSegment("key", chartKey);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public void MoveOutOfArchive(string chartKey)
        {
            var restRequest = new RestRequest("/charts/{key}/actions/move-out-of-archive", Method.Post)
                .AddUrlSegment("key", chartKey);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public Drawing RetrievePublishedVersion(string chartKey)
        {
            var restRequest = new RestRequest("/charts/{key}/version/published", Method.Get)
                .AddUrlSegment("key", chartKey);
            return AssertOk(_restClient.Execute<Drawing>(restRequest));
        }

        public byte[] RetrievePublishedVersionThumbnail(string chartKey)
        {
            var restRequest = new RestRequest("/charts/{key}/version/published/thumbnail", Method.Get)
                .AddUrlSegment("key", chartKey);
            var restResponse = _restClient.Execute<object>(restRequest);
            AssertOk(restResponse);
            return restResponse.RawBytes;
        }

        public Drawing RetrieveDraftVersion(string chartKey)
        {
            var restRequest = new RestRequest("/charts/{key}/version/draft", Method.Get)
                .AddUrlSegment("key", chartKey);
            return AssertOk(_restClient.Execute<Drawing>(restRequest));
        }

        public byte[] RetrieveDraftVersionThumbnail(string chartKey)
        {
            var restRequest = new RestRequest("/charts/{key}/version/draft/thumbnail", Method.Get)
                .AddUrlSegment("key", chartKey);
            var restResponse = _restClient.Execute<object>(restRequest);
            AssertOk(restResponse);
            return restResponse.RawBytes;
        }

        public void PublishDraftVersion(string chartKey)
        {
            var restRequest = new RestRequest("/charts/{key}/version/draft/actions/publish", Method.Post)
                .AddUrlSegment("key", chartKey);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public void DiscardDraftVersion(string chartKey)
        {
            var restRequest = new RestRequest("/charts/{key}/version/draft/actions/discard", Method.Post)
                .AddUrlSegment("key", chartKey);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public ChartValidationResult ValidatePublishedVersion(string chartKey)
        {
            var restRequest = new RestRequest("/charts/{key}/version/published/actions/validate", Method.Post)
                .AddUrlSegment("key",chartKey);
            return AssertOk(_restClient.Execute<ChartValidationResult>(restRequest));
        }

        public ChartValidationResult ValidateDraftVersion(string chartKey)
        {
            var restRequest = new RestRequest("/charts/{key}/version/draft/actions/validate", Method.Post)
                .AddUrlSegment("key",chartKey);
            return AssertOk(_restClient.Execute<ChartValidationResult>(restRequest));
        }

        public IEnumerable<Chart> ListAll(string filter = null, string tag = null, bool? expandEvents = null, bool? withValidation = false)
        {
            return List().All(ChartListParams(filter, tag, expandEvents, withValidation));
        }

        public Page<Chart> ListFirstPage(string filter = null, string tag = null, bool? expandEvents = false, int? pageSize = null, bool? withValidation = false)
        {
            return List().FirstPage(ChartListParams(filter, tag, expandEvents, withValidation), pageSize);
        }

        public Page<Chart> ListPageAfter(long id, string filter = null, string tag = null, bool? expandEvents = false, int? pageSize = null, bool? withValidation = false)
        {
            return List().PageAfter(id, ChartListParams(filter, tag, expandEvents, withValidation), pageSize);
        }

        public Page<Chart> ListPageBefore(long id, string filter = null, string tag = null, bool? expandEvents = false, int? pageSize = null, bool? withValidation = false)
        {
            return List().PageBefore(id, ChartListParams(filter, tag, expandEvents, withValidation), pageSize);
        }

        private Dictionary<string, object> ChartListParams(string filter, string tag, bool? expandEvents, bool? withValidation = false)
        {
            var chartListParams = new Dictionary<string, object>();

            if (filter != null)
            {
                chartListParams.Add("filter", filter);
            }

            if (tag != null)
            {
                chartListParams.Add("tag", tag);
            }

            if (expandEvents != null && expandEvents.Value)
            {
                chartListParams.Add("expand", "events");
            }

            if (withValidation == true) {
                chartListParams.Add("validation", true);
            }

            return chartListParams;
        }

        private ParametrizedLister<Chart> List()
        {
            return new ParametrizedLister<Chart>(new PageFetcher<Chart>(_restClient, "/charts"));
        }
    }
}