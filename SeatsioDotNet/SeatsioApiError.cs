using System;

namespace SeatsioDotNet;

public class SeatsioApiError
{
    public string Code { get; }
    public string Message { get; }

    public SeatsioApiError(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public override bool Equals(Object obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (!(obj is SeatsioApiError p))
        {
            return false;
        }

        return (Code.Equals(p.Code)) && (Message.Equals(p.Message));
    }

    public override int GetHashCode()
    {
        return Code.GetHashCode() ^ Message.GetHashCode();
    }
}