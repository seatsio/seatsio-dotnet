using System;

namespace SeatsioDotNet;

public class SeatsioTimeoutException : SeatsioException
{
    public SeatsioTimeoutException(Exception innerException) :
        base("Request timed out", innerException)
    {
    }
}
