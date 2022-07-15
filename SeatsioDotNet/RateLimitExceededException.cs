using System.Collections.Generic;
using RestSharp;

namespace SeatsioDotNet
{
    public class RateLimitExceededException : SeatsioException
    {
        public RateLimitExceededException(List<SeatsioApiError> errors, string requestId, RestResponse response) :
            base(errors, requestId, response)
        {
        }
    }
}