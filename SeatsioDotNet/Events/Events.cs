﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using RestSharp;
using SeatsioDotNet.Charts;
using SeatsioDotNet.EventReports;
using SeatsioDotNet.Util;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.Events
{
    public class Events
    {
        private readonly RestClient _restClient;
        public Channels Channels { get; }

        public Events(RestClient restClient)
        {
            _restClient = restClient;
            Channels = new Channels(restClient);
        }

        public Event Create(string chartKey)
        {
            return Create(chartKey, null, null, null, null);
        }

        public Event Create(string chartKey, string eventKey)
        {
            return Create(chartKey, eventKey, null, null, null);
        }

        public Event Create(string chartKey, string eventKey, TableBookingConfig tableBookingConfig)
        {
            return Create(chartKey, eventKey, tableBookingConfig, null, null);
        }

        public Event Create(string chartKey, string eventKey, TableBookingConfig tableBookingConfig,
            string socialDistancingRulesetKey)
        {
            return Create(chartKey, eventKey, tableBookingConfig, socialDistancingRulesetKey, null);
        }

        public Event Create(string chartKey, string eventKey, TableBookingConfig tableBookingConfig,
            string socialDistancingRulesetKey, Dictionary<string, object> objectCategories)
        {
            return Create(chartKey, eventKey, tableBookingConfig, socialDistancingRulesetKey, objectCategories, null);
        }

        public Event Create(string chartKey, string eventKey, TableBookingConfig tableBookingConfig,
            string socialDistancingRulesetKey, Dictionary<string, object> objectCategories, Category[] categories)
        {
            Dictionary<string, object> requestBody = new Dictionary<string, object>();
            requestBody.Add("chartKey", chartKey);

            if (eventKey != null)
            {
                requestBody.Add("eventKey", eventKey);
            }

            if (tableBookingConfig != null)
            {
                requestBody.Add("tableBookingConfig", tableBookingConfig.AsJsonObject());
            }

            if (socialDistancingRulesetKey != null)
            {
                requestBody.Add("socialDistancingRulesetKey", socialDistancingRulesetKey);
            }

            if (objectCategories != null)
            {
                requestBody.Add("objectCategories", objectCategories);
            }

            if (categories != null)
            {
                requestBody.Add("categories", categories);
            }

            var restRequest = new RestRequest("/events", Method.Post).AddJsonBody(requestBody);
            return AssertOk(_restClient.Execute<Event>(restRequest));
        }

        public Event[] Create(string chartKey, EventCreationParams[] eventCreationParams)
        {
            Dictionary<string, object> requestBody = new Dictionary<string, object>();
            requestBody.Add("chartKey", chartKey);
            var events = new List<Dictionary<string, object>>();
            foreach (var param in eventCreationParams)
            {
                var e = new Dictionary<string, object>();
                if (param.Key != null)
                {
                    e.Add("eventKey", param.Key);
                }

                if (param.TableBookingConfig != null)
                {
                    e.Add("tableBookingConfig", param.TableBookingConfig.AsJsonObject());
                }

                if (param.SocialDistancingRulesetKey != null)
                {
                    e.Add("socialDistancingRulesetKey", param.SocialDistancingRulesetKey);
                }

                if (param.ObjectCategories != null)
                {
                    e.Add("objectCategories", param.ObjectCategories);
                }

                if (param.Categories != null)
                {
                    e.Add("categories", param.Categories);
                }

                events.Add(e);
            }

            requestBody.Add("events", events.ToArray());
            var restRequest = new RestRequest("/events/actions/create-multiple", Method.Post).AddJsonBody(requestBody);
            return AssertOk(_restClient.Execute<MultipleEvents>(restRequest)).events.ToArray();
        }

        public void UpdateChartKey(string eventKey, string newChartKey)
        {
            Update(eventKey, newChartKey, null, null, null, null, null);
        }

        public void UpdateEventKey(string oldEventKey, string newEventKey)
        {
            Update(oldEventKey, null, newEventKey, null, null, null, null);
        }

        public void UpdateTableBookingConfig(string eventKey, TableBookingConfig newTableBookingConfig)
        {
            Update(eventKey, null, null, newTableBookingConfig, null, null, null);
        }

        public void UpdateSocialDistancingRulesetKey(string eventKey, string newSocialDistancingRulesetKey)
        {
            Update(eventKey, null, null, null, newSocialDistancingRulesetKey, null, null);
        }

        public void RemoveSocialDistancingRulesetKey(string eventKey)
        {
            UpdateSocialDistancingRulesetKey(eventKey, "");
        }

        public void UpdateObjectCategories(string eventKey, Dictionary<string, object> newObjectCategories)
        {
            Update(eventKey, null, null, null, null, newObjectCategories, null);
        }
        
        public void RemoveObjectCategories(string eventKey)
        {
            UpdateObjectCategories(eventKey, new Dictionary<string, object>());
        }

        public void UpdateCategories(string eventKey, Category[] categories)
        {
            Update(eventKey, null, null, null, null, null, categories);
        }

        public void RemoveCategories(string eventKey)
        {
            UpdateCategories(eventKey, Array.Empty<Category>());
        }
        
        public void Update(string eventKey, string chartKey, string newEventKey, TableBookingConfig tableBookingConfig,
            string socialDistancingRulesetKey, Dictionary<string, object> objectCategories, Category[] categories)
        {
            Dictionary<string, object> requestBody = new Dictionary<string, object>();

            if (chartKey != null)
            {
                requestBody.Add("chartKey", chartKey);
            }

            if (newEventKey != null)
            {
                requestBody.Add("eventKey", newEventKey);
            }

            if (tableBookingConfig != null)
            {
                requestBody.Add("tableBookingConfig", tableBookingConfig.AsJsonObject());
            }

            if (socialDistancingRulesetKey != null)
            {
                requestBody.Add("socialDistancingRulesetKey", socialDistancingRulesetKey);
            }

            if (objectCategories != null)
            {
                requestBody.Add("objectCategories", objectCategories);
            }

            if (categories != null)
            {
                requestBody.Add("categories", categories);
            }

            var restRequest = new RestRequest("/events/{key}", Method.Post)
                .AddUrlSegment("key", eventKey)
                .AddJsonBody(requestBody);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public void Delete(string eventKey)
        {
            var restRequest = new RestRequest("/events/{key}", Method.Delete)
                .AddUrlSegment("key", eventKey);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public Event Retrieve(string eventKey)
        {
            var restRequest = new RestRequest("/events/{key}")
                .AddUrlSegment("key", eventKey);
            return AssertOk(_restClient.Execute<Event>(restRequest));
        }

        public EventObjectInfo RetrieveObjectInfo(string eventKey, string objectLabel)
        {
            var result = RetrieveObjectInfos(eventKey, new[] {objectLabel});
            return result[objectLabel];
        }

        public Dictionary<string, EventObjectInfo> RetrieveObjectInfos(string eventKey, string[] objectLabels)
        {
            var restRequest = new RestRequest("/events/{key}/objects")
                .AddUrlSegment("key", eventKey);

            foreach (var objectLabel in objectLabels)
            {
                restRequest.AddQueryParameter("label", objectLabel);
            }

            return AssertOk(_restClient.Execute<Dictionary<string, EventObjectInfo>>(restRequest));
        }

        public ChangeObjectStatusResult Book(string eventKey, IEnumerable<string> objects, string holdToken = null,
            string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null, string[] channelKeys = null,
            bool? ignoreSocialDistancing = null)
        {
            return ChangeObjectStatus(eventKey, objects, EventObjectInfo.Booked, holdToken, orderId, keepExtraData,
                ignoreChannels, channelKeys, ignoreSocialDistancing);
        }

        public ChangeObjectStatusResult Book(string[] eventKeys, IEnumerable<string> objects, string holdToken = null,
            string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null, string[] channelKeys = null,
            bool? ignoreSocialDistancing = null)
        {
            return ChangeObjectStatus(eventKeys, objects, EventObjectInfo.Booked, holdToken, orderId, keepExtraData,
                ignoreChannels, channelKeys, ignoreSocialDistancing);
        }

        public ChangeObjectStatusResult Book(string eventKey, IEnumerable<ObjectProperties> objects,
            string holdToken = null, string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null,
            string[] channelKeys = null, bool? ignoreSocialDistancing = null)
        {
            return ChangeObjectStatus(eventKey, objects, EventObjectInfo.Booked, holdToken, orderId, keepExtraData,
                ignoreChannels, channelKeys, ignoreSocialDistancing);
        }

        public ChangeObjectStatusResult Book(string[] eventKeys, IEnumerable<ObjectProperties> objects,
            string holdToken = null, string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null,
            string[] channelKeys = null, bool? ignoreSocialDistancing = null)
        {
            return ChangeObjectStatus(eventKeys, objects, EventObjectInfo.Booked, holdToken, orderId, keepExtraData,
                ignoreChannels, channelKeys, ignoreSocialDistancing);
        }

        public BestAvailableResult Book(string eventKey, BestAvailable bestAvailable, string holdToken = null,
            string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null, string[] channelKeys = null)
        {
            return ChangeObjectStatus(eventKey, bestAvailable, EventObjectInfo.Booked, holdToken, orderId,
                keepExtraData, ignoreChannels, channelKeys);
        }

        public ChangeObjectStatusResult Release(string eventKey, IEnumerable<string> objects, string holdToken = null,
            string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null, string[] channelKeys = null)
        {
            return ChangeObjectStatus(eventKey, objects, EventObjectInfo.Free, holdToken, orderId, keepExtraData,
                ignoreChannels, channelKeys);
        }

        public ChangeObjectStatusResult Release(string[] eventKeys, IEnumerable<string> objects,
            string holdToken = null, string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null,
            string[] channelKeys = null)
        {
            return ChangeObjectStatus(eventKeys, objects, EventObjectInfo.Free, holdToken, orderId, keepExtraData,
                ignoreChannels, channelKeys);
        }

        public ChangeObjectStatusResult Release(string eventKey, IEnumerable<ObjectProperties> objects,
            string holdToken = null, string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null,
            string[] channelKeys = null)
        {
            return ChangeObjectStatus(eventKey, objects, EventObjectInfo.Free, holdToken, orderId, keepExtraData,
                ignoreChannels, channelKeys);
        }

        public ChangeObjectStatusResult Release(string[] eventKeys, IEnumerable<ObjectProperties> objects,
            string holdToken = null, string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null,
            string[] channelKeys = null)
        {
            return ChangeObjectStatus(eventKeys, objects, EventObjectInfo.Free, holdToken, orderId, keepExtraData,
                ignoreChannels, channelKeys);
        }

        public ChangeObjectStatusResult Hold(string eventKey, IEnumerable<string> objects, string holdToken,
            string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null, string[] channelKeys = null,
            bool? ignoreSocialDistancing = null)
        {
            return ChangeObjectStatus(eventKey, objects, EventObjectInfo.Held, holdToken, orderId, keepExtraData,
                ignoreChannels, channelKeys, ignoreSocialDistancing);
        }

        public ChangeObjectStatusResult Hold(string[] eventKeys, IEnumerable<string> objects, string holdToken,
            string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null, string[] channelKeys = null,
            bool? ignoreSocialDistancing = null)
        {
            return ChangeObjectStatus(eventKeys, objects, EventObjectInfo.Held, holdToken, orderId, keepExtraData,
                ignoreChannels, channelKeys, ignoreSocialDistancing);
        }

        public ChangeObjectStatusResult Hold(string eventKey, IEnumerable<ObjectProperties> objects, string holdToken,
            string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null, string[] channelKeys = null,
            bool? ignoreSocialDistancing = null)
        {
            return ChangeObjectStatus(eventKey, objects, EventObjectInfo.Held, holdToken, orderId, keepExtraData,
                ignoreChannels, channelKeys, ignoreSocialDistancing);
        }

        public ChangeObjectStatusResult Hold(string[] eventKeys, IEnumerable<ObjectProperties> objects,
            string holdToken, string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null,
            string[] channelKeys = null, bool? ignoreSocialDistancing = null)
        {
            return ChangeObjectStatus(eventKeys, objects, EventObjectInfo.Held, holdToken, orderId, keepExtraData,
                ignoreChannels, channelKeys, ignoreSocialDistancing);
        }

        public BestAvailableResult Hold(string eventKey, BestAvailable bestAvailable, string holdToken,
            string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null, string[] channelKeys = null)
        {
            return ChangeObjectStatus(eventKey, bestAvailable, EventObjectInfo.Held, holdToken, orderId, keepExtraData,
                ignoreChannels, channelKeys);
        }

        public ChangeObjectStatusResult ChangeObjectStatus(string eventKey, IEnumerable<string> objects, string status,
            string holdToken = null, string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null,
            string[] channelKeys = null, bool? ignoreSocialDistancing = null, string[] allowedPreviousStatuses = null,
            string[] rejectedPreviousStatuses = null)
        {
            return ChangeObjectStatus(new[] {eventKey}, objects.Select(o => new ObjectProperties(o)), status, holdToken,
                orderId, keepExtraData, ignoreChannels, channelKeys, ignoreSocialDistancing, allowedPreviousStatuses,
                rejectedPreviousStatuses);
        }

        public ChangeObjectStatusResult ChangeObjectStatus(string eventKey, IEnumerable<ObjectProperties> objects,
            string status, string holdToken = null, string orderId = null, bool? keepExtraData = null,
            bool? ignoreChannels = null, string[] channelKeys = null, bool? ignoreSocialDistancing = null)
        {
            return ChangeObjectStatus(new[] {eventKey}, objects, status, holdToken, orderId, keepExtraData,
                ignoreChannels, channelKeys, ignoreSocialDistancing);
        }

        public ChangeObjectStatusResult ChangeObjectStatus(IEnumerable<string> events, IEnumerable<string> objects,
            string status, string holdToken = null, string orderId = null, bool? keepExtraData = null,
            bool? ignoreChannels = null, string[] channelKeys = null, bool? ignoreSocialDistancing = null)
        {
            return ChangeObjectStatus(events, objects.Select(o => new ObjectProperties(o)), status, holdToken, orderId,
                keepExtraData, ignoreChannels, channelKeys, ignoreSocialDistancing);
        }

        public ChangeObjectStatusResult ChangeObjectStatus(IEnumerable<string> events,
            IEnumerable<ObjectProperties> objects, string status, string holdToken = null, string orderId = null,
            bool? keepExtraData = null, bool? ignoreChannels = null, string[] channelKeys = null,
            bool? ignoreSocialDistancing = null, string[] allowedPreviousStatuses = null,
            string[] rejectedPreviousStatuses = null)
        {
            var requestBody = ChangeObjectStatusRequest(events, objects, status, holdToken, orderId, keepExtraData,
                ignoreChannels, channelKeys, ignoreSocialDistancing, allowedPreviousStatuses, rejectedPreviousStatuses);
            var restRequest = new RestRequest("/events/groups/actions/change-object-status", Method.Post)
                .AddQueryParameter("expand", "objects")
                .AddJsonBody(requestBody);
            return AssertOk(_restClient.Execute<ChangeObjectStatusResult>(restRequest));
        }

        public List<ChangeObjectStatusResult> ChangeObjectStatus(StatusChangeRequest[] requests)
        {
            var serializedRequests = requests.Select(r => ChangeObjectStatusRequest(r.EventKey, r.Objects, r.Status,
                r.HoldToken, r.OrderId, r.KeepExtraData, r.IgnoreChannels, r.ChannelKeys, null,
                r.AllowedPreviousStatuses, r.RejectedPreviousStatuses));
            var restRequest = new RestRequest("/events/actions/change-object-status", Method.Post)
                .AddQueryParameter("expand", "objects")
                .AddJsonBody(new Dictionary<string, object> {{"statusChanges", serializedRequests}});
            return AssertOk(_restClient.Execute<ChangeObjectStatusInBatchResult>(restRequest)).Results;
        }

        private Dictionary<string, object> ChangeObjectStatusRequest(string evnt, IEnumerable<ObjectProperties> objects,
            string status, string holdToken, string orderId, bool? keepExtraData, bool? ignoreChannels = null,
            string[] channelKeys = null, bool? ignoreSocialDistancing = null, string[] allowedPreviousStatuses = null,
            string[] rejectedPreviousStatuses = null)
        {
            var request = ChangeObjectStatusRequest(objects, status, holdToken, orderId, keepExtraData, ignoreChannels,
                channelKeys, ignoreSocialDistancing, allowedPreviousStatuses, rejectedPreviousStatuses);
            request.Add("event", evnt);
            return request;
        }

        private Dictionary<string, object> ChangeObjectStatusRequest(IEnumerable<string> events,
            IEnumerable<ObjectProperties> objects, string status, string holdToken, string orderId, bool? keepExtraData,
            bool? ignoreChannels = null, string[] channelKeys = null, bool? ignoreSocialDistancing = null,
            string[] allowedPreviousStatuses = null, string[] rejectedPreviousStatuses = null)
        {
            var request = ChangeObjectStatusRequest(objects, status, holdToken, orderId, keepExtraData, ignoreChannels,
                channelKeys, ignoreSocialDistancing, allowedPreviousStatuses, rejectedPreviousStatuses);
            request.Add("events", events);
            return request;
        }

        private Dictionary<string, object> ChangeObjectStatusRequest(IEnumerable<ObjectProperties> objects,
            string status, string holdToken, string orderId, bool? keepExtraData, bool? ignoreChannels = null,
            string[] channelKeys = null, bool? ignoreSocialDistancing = null, string[] allowedPreviousStatuses = null,
            string[] rejectedPreviousStatuses = null)
        {
            var requestBody = new Dictionary<string, object>()
            {
                {"status", status},
                {"objects", objects.Select(o => o.AsDictionary())},
            };

            if (holdToken != null)
            {
                requestBody.Add("holdToken", holdToken);
            }

            if (orderId != null)
            {
                requestBody.Add("orderId", orderId);
            }

            if (keepExtraData != null)
            {
                requestBody.Add("keepExtraData", keepExtraData);
            }

            if (ignoreChannels != null)
            {
                requestBody.Add("ignoreChannels", ignoreChannels);
            }

            if (channelKeys != null)
            {
                requestBody.Add("channelKeys", channelKeys);
            }

            if (ignoreSocialDistancing != null)
            {
                requestBody.Add("ignoreSocialDistancing", ignoreSocialDistancing);
            }

            if (allowedPreviousStatuses != null)
            {
                requestBody.Add("allowedPreviousStatuses", allowedPreviousStatuses);
            }

            if (rejectedPreviousStatuses != null)
            {
                requestBody.Add("rejectedPreviousStatuses", rejectedPreviousStatuses);
            }

            return requestBody;
        }

        public BestAvailableResult ChangeObjectStatus(string eventKey, BestAvailable bestAvailable, string status,
            string holdToken = null, string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null,
            string[] channelKeys = null)
        {
            var requestBody = new Dictionary<string, object>()
            {
                {"status", status},
                {"bestAvailable", bestAvailable.AsDictionary()}
            };

            if (holdToken != null)
            {
                requestBody.Add("holdToken", holdToken);
            }

            if (orderId != null)
            {
                requestBody.Add("orderId", orderId);
            }

            if (keepExtraData != null)
            {
                requestBody.Add("keepExtraData", keepExtraData);
            }

            if (ignoreChannels != null)
            {
                requestBody.Add("ignoreChannels", ignoreChannels);
            }

            if (channelKeys != null)
            {
                requestBody.Add("channelKeys", channelKeys);
            }

            var restRequest = new RestRequest("/events/{key}/actions/change-object-status", Method.Post)
                .AddUrlSegment("key", eventKey)
                .AddJsonBody(requestBody);
            return AssertOk(_restClient.Execute<BestAvailableResult>(restRequest));
        }

        public void UpdateExtraData(string eventKey, string objectLabel, Dictionary<string, object> extraData)
        {
            var restRequest = new RestRequest("/events/{key}/objects/{object}/actions/update-extra-data", Method.Post)
                .AddUrlSegment("key", eventKey)
                .AddUrlSegment("object", objectLabel)
                .AddJsonBody(new {extraData});
            AssertOk(_restClient.Execute<BestAvailableResult>(restRequest));
        }

        public void UpdateExtraDatas(string eventKey, Dictionary<string, Dictionary<string, object>> extraData)
        {
            var restRequest = new RestRequest("/events/{key}/actions/update-extra-data", Method.Post)
                .AddUrlSegment("key", eventKey)
                .AddParameter("application/json", JsonSerializer.Serialize(new {extraData}),
                    ParameterType.RequestBody); // default serializer doesn't convert extraData to JSON properly
            AssertOk(_restClient.Execute<BestAvailableResult>(restRequest));
        }

        public void MarkAsForSale(string eventKey, IEnumerable<string> objects, Dictionary<string, int> areaPlaces, IEnumerable<string> categories)
        {
            var requestBody = ForSaleRequest(objects, areaPlaces, categories);
            var restRequest = new RestRequest("/events/{key}/actions/mark-as-for-sale", Method.Post)
                .AddUrlSegment("key", eventKey)
                .AddJsonBody(requestBody);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public void MarkAsNotForSale(string eventKey, IEnumerable<string> objects, Dictionary<string, int> areaPlaces, IEnumerable<string> categories)
        {
            var requestBody = ForSaleRequest(objects, areaPlaces, categories);
            var restRequest = new RestRequest("/events/{key}/actions/mark-as-not-for-sale", Method.Post)
                .AddUrlSegment("key", eventKey)
                .AddJsonBody(requestBody);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public void MarkEverythingAsForSale(string eventKey)
        {
            var restRequest = new RestRequest("/events/{key}/actions/mark-everything-as-for-sale", Method.Post)
                .AddUrlSegment("key", eventKey);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        private Dictionary<string, object> ForSaleRequest(IEnumerable<string> objects, Dictionary<string, int> areaPlaces, IEnumerable<string> categories)
        {
            var request = new Dictionary<string, object>();

            if (objects != null)
            {
                request.Add("objects", objects);
            }     
            
            if (areaPlaces != null)
            {
                request.Add("areaPlaces", areaPlaces);
            }

            if (categories != null)
            {
                request.Add("categories", categories);
            }

            return request;
        }

        public IEnumerable<Event> ListAll()
        {
            return List().All();
        }

        public Page<Event> ListFirstPage(int? pageSize = null)
        {
            return List().FirstPage(pageSize: pageSize);
        }

        public Page<Event> ListPageAfter(long id, int? pageSize = null)
        {
            return List().PageAfter(id, pageSize: pageSize);
        }

        public Page<Event> ListPageBefore(long id, int? pageSize = null)
        {
            return List().PageBefore(id, pageSize: pageSize);
        }

        private Lister<Event> List()
        {
            return new Lister<Event>(new PageFetcher<Event>(_restClient, "/events"));
        }

        public Lister<StatusChange> StatusChanges(string eventKey, string filter = null, string sortField = null,
            string sortDirection = null)
        {
            return new Lister<StatusChange>(new PageFetcher<StatusChange>(
                _restClient,
                "/events/{key}/status-changes",
                request => request.AddUrlSegment("key", eventKey),
                new() {{"filter", filter}, {"sort", ToSort(sortField, sortDirection)}}
            ));
        }

        private static string ToSort(string sortField, string sortDirection)
        {
            if (sortField == null)
            {
                return null;
            }

            if (sortDirection == null)
            {
                return sortField;
            }

            return sortField + ":" + sortDirection;
        }

        public Lister<StatusChange> StatusChangesForObject(string eventKey, string objectLabel)
        {
            return new Lister<StatusChange>(new PageFetcher<StatusChange>(
                _restClient,
                "/events/{key}/objects/{objectId}/status-changes",
                request => request.AddUrlSegment("key", eventKey).AddUrlSegment("objectId", objectLabel)
            ));
        }

        private class ChangeObjectStatusInBatchResult
        {
            public List<ChangeObjectStatusResult> Results { get; set; }
        }
    }
}