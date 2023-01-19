using RestSharp;
using SeatsioDotNet.Util;
using Xunit;

namespace SeatsioDotNet.Test
{
    public class ErrorHandlingTest : SeatsioClientTest
    {
        [Fact]
        public void Test4xx()
        {
            var e = Assert.Throws<SeatsioException>(() =>
                Client.Events.RetrieveObjectInfo("unexistingEvent", "unexistingObject"));

            Assert.Equal("Event not found: unexistingEvent.", e.Message);
            Assert.Contains(new SeatsioApiError("EVENT_NOT_FOUND", "Event not found: unexistingEvent"), e.Errors);
            Assert.NotNull(e.RequestId);
        }

        [Fact]
        public void Test5xx()
        {
            var client = new SeatsioRestClient("https://httpbin.seatsio.net");

            var e = Assert.Throws<SeatsioException>(() => RestUtil.AssertOk(client.Execute<object>(new RestRequest("/status/500"))));
            
            Assert.Equal("Get https://httpbin.seatsio.net/status/500 resulted in a 500 Internal Server Error response. Body: ", e.Message);
        }

        [Fact]
        public void WeirdError()
        {
            var e = Assert.Throws<SeatsioException>(() =>
                new SeatsioClient("", null, "unknownProtocol://").Events.RetrieveObjectInfo("unexistingEvent", "unexistingObject"));

            Assert.Equal("Get  resulted in a 0  response. Body: ", e.Message);
            Assert.Null(e.Errors);
            Assert.Null(e.RequestId);
        }
    }
}