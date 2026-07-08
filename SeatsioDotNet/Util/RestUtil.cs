using System.Threading.Tasks;
using RestSharp;

namespace SeatsioDotNet.Util;

public class RestUtil
{
    public static T AssertOk<T>(RestResponse<T> response)
    {
        AssertSuccess(response);
        return response.Data;
    }

    public static string AssertOkString(RestResponse response)
    {
        AssertSuccess(response);
        return response.Content ?? string.Empty;
    }

    private static void AssertSuccess(RestResponse response)
    {
        if (response.ErrorException is TaskCanceledException)
        {
            throw new SeatsioTimeoutException(response.ErrorException);
        }

        if ((int) response.StatusCode < 200 || (int) response.StatusCode >= 300)
        {
            throw SeatsioException.From(response);
        }
    }
}