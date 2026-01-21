using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using RestSharp;

namespace SeatsioDotNet;

public class SeatsioException : Exception
{
    public readonly List<SeatsioApiError> Errors;
    public readonly string RequestId;

    protected SeatsioException(List<SeatsioApiError> errors, string requestId) : base(
        ExceptionMessage(errors))
    {
        Errors = errors;
        RequestId = requestId;
    }

    private SeatsioException(RestResponse response) : base(ExceptionMessage(response))
    {
    }

    protected SeatsioException(string message, Exception innerException) : base(message, innerException)
    {
    }

    private static string ExceptionMessage(List<SeatsioApiError> errors)
    {
        return String.Join(", ", errors.Select(x => x.Message)) + ".";
    }

    private static string ExceptionMessage(RestResponse response)
    {
        var request = response.Request;
        return request.Method + " " + response.ResponseUri + " resulted in a " + (int) response.StatusCode + " " + response.StatusDescription + " response. Body: " + response.Content;
    }

    public static SeatsioException From(RestResponse response)
    {
        if (response.ContentType != null && response.ContentType.Contains("application/json"))
        {
            var parsedException = JsonSerializer.Deserialize<SeatsioExceptionTo>(response.Content,
                SeatsioRestClient.SeatsioJsonSerializerOptions());
            if ((int) response.StatusCode == 429)
            {
                return new RateLimitExceededException(parsedException.Errors, parsedException.RequestId);
            }
            else if (IsBestAvailableObjectsNotFound(parsedException))
            {
                return new BestAvailableObjectsNotFoundException(parsedException.Errors, parsedException.RequestId);
            }

            return new SeatsioException(parsedException.Errors, parsedException.RequestId);
        }

        return new SeatsioException(response);
    }

    private static bool IsBestAvailableObjectsNotFound(SeatsioExceptionTo parsedException)
    {
        return parsedException.Errors.Any(e => e.Code == "BEST_AVAILABLE_OBJECTS_NOT_FOUND");
    }

    public struct SeatsioExceptionTo
    {
        public List<SeatsioApiError> Errors { get; set; }
        public string RequestId { get; set; }
    }
}
