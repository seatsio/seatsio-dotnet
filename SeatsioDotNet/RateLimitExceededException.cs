using System.Collections.Generic;

namespace SeatsioDotNet;

public class RateLimitExceededException : SeatsioException
{
    public RateLimitExceededException(List<SeatsioApiError> errors, string requestId) :
        base(errors, requestId)
    {
    }
}