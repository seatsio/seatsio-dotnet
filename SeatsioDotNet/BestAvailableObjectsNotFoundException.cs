using System.Collections.Generic;

namespace SeatsioDotNet;

public class BestAvailableObjectsNotFoundException : SeatsioException
{
    public BestAvailableObjectsNotFoundException(List<SeatsioApiError> errors, string requestId) :
        base(errors, requestId)
    {
    }
}
