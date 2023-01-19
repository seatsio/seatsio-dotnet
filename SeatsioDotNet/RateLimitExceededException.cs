using System.Collections.Generic;
using RestSharp;

namespace SeatsioDotNet
{
    public class RateLimitExceededException : SeatsioException
    {
        public RateLimitExceededException(List<SeatsioApiError> errors, string requestId) :
            base(errors, requestId)
        {
        }
    }
}