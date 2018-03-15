using System;
using RestSharp;

namespace SeatsioDotNet.Util
{
    public class RestUtil
    {
        public static T AssertOk<T>(IRestResponse<T> response)
        {
            if ((int) response.StatusCode < 200 || (int) response.StatusCode >= 300)
            {
                throw new Exception(response.StatusCode + "-" + response.Content);
            }

            return response.Data;
        }
    }
}