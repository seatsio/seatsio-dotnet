using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using RestSharp;

namespace SeatsioDotNet
{
    public class SeatsioException : Exception
    {
        public readonly List<SeatsioApiError> Errors;
        public readonly string RequestId;

        protected SeatsioException(List<SeatsioApiError> errors, string requestId, RestResponse response) : base(ExceptionMessage(errors, requestId, response))
        {
            Errors = errors;
            RequestId = requestId;
        }

        private SeatsioException(RestResponse response) : base(ExceptionMessage(response))
        {
        }

        private static string ExceptionMessage(List<SeatsioApiError> errors, string requestId, RestResponse response)
        {
            string exceptionMessage = ExceptionMessage(response);            
            exceptionMessage += " Reason: " + String.Join(", ", errors.Select(x => x.Message)) + ".";
            exceptionMessage += " Request ID: " + requestId;
            return exceptionMessage;
        }

        private static string ExceptionMessage(RestResponse response)
        {
            var request = response.Request;
            return request.Method + " " + response.ResponseUri + " resulted in a " + (int) response.StatusCode + " " + response.StatusDescription + " response.";
        }

        public static SeatsioException From(RestResponse response)
        {
            if (response.ContentType != null && response.ContentType.Contains("application/json"))
            {
                var parsedException = JsonSerializer.Deserialize<SeatsioExceptionTo>(response.Content, SeatsioRestClient.SeatsioJsonSerializerOptions());
                if ((int) response.StatusCode == 429)
                {
                    return new RateLimitExceededException(parsedException.Errors, parsedException.RequestId, response);
                }
                return new SeatsioException(parsedException.Errors, parsedException.RequestId, response);
            }

            return new SeatsioException(response);
        }

        public struct SeatsioExceptionTo
        {
            public List<SeatsioApiError> Errors { get; set; }
            public string RequestId { get; set; }
        }
    }
}