using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;

namespace SeatsioDotNet
{
    public class SeatsioException : Exception
    {
        public readonly List<string> Messages;
        public readonly string RequestId;

        private SeatsioException(List<string> messages, string requestId, IRestResponse response) : base(ExceptionMessage(messages, requestId, response))
        {
            Messages = messages;
            RequestId = requestId;
        }

        private SeatsioException(IRestResponse response) : base(ExceptionMessage(response))
        {
        }

        private static string ExceptionMessage(List<string> messages, string requestId, IRestResponse response)
        {
            string exceptionMessage = ExceptionMessage(response);
            exceptionMessage += " Reason: " + String.Join(", ", messages) + ".";
            exceptionMessage += " Request ID: " + requestId;
            return exceptionMessage;
        }

        private static string ExceptionMessage(IRestResponse response)
        {
            var request = response.Request;
            return request.Method + " " + response.ResponseUri + " resulted in a " + (int) response.StatusCode + " " + response.StatusDescription + " response.";
        }

        public static SeatsioException From(IRestResponse response)
        {
            if (response.ContentType != null && response.ContentType.Contains("application/json"))
            {
                var parsedException = JsonConvert.DeserializeObject<SeatsioExceptionTO>(response.Content);
                return new SeatsioException(parsedException.Messages, parsedException.RequestId, response);
            }

            return new SeatsioException(response);
        }

        public struct SeatsioExceptionTO
        {
            public List<string> Messages { get; set; }
            public string RequestId { get; set; }
        }
    }
}